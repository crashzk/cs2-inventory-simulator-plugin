﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API;
using System.Runtime.InteropServices;

namespace InventorySimulator;

[MinimumApiVersion(215)]
public partial class InventorySimulator : BasePlugin
{
    public override string ModuleAuthor => "Ian Lucas & crashzk";
    public override string ModuleDescription => "Inventory Simulator (cs2.zkservidores.com)";
    public override string ModuleName => "InventorySimulator";
    public override string ModuleVersion => "1.0.0-beta.22";

    private readonly string InventoryFilePath = "csgo/css_inventories.json";
    private readonly Dictionary<ulong, PlayerInventory> InventoryManager = new();
    private readonly HashSet<ulong> LoadedSteamIds = new();
    private static readonly ulong MinimumCustomItemID = 68719476736;
    private ulong NextItemId = MinimumCustomItemID;

    private readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public FakeConVar<string> InvSimProtocolCvar = new("css_inventory_simulator_protocol", "Protocol used by Inventory Simulator to consume its API.", "https");
    public FakeConVar<string> InvSimCvar = new("css_inventory_simulator", "Host of Inventory Simulator's API.", "cs2.zkservidores.com");
    public FakeConVar<string> InvSimApiKeyCvar = new("css_inventory_simulator_apikey", "API Key for Inventory Simulator.", "");
    public FakeConVar<bool> StatTrakIgnoreBotsCvar = new("css_stattrak_ignore_bots", "Determines whether to ignore StatTrak increments for bot kills.", true);
    public FakeConVar<int> MinModelsCvar = new("css_minmodels", "Limits the number of custom models allowed in-game.", 0, flags: ConVarFlags.FCVAR_NONE, new RangeValidator<int>(0, 2));

    public readonly FakeConVar<bool> StatTrakIgnoreBotsCvar = new("css_stattrak_ignore_bots", "Determines whether to ignore StatTrak increments for bot kills.", true);

    public readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public override void Load(bool hotReload)
    {
        LoadPlayerInventories();

        if (!IsWindows)
        {
            // GiveNamedItemFunc hooking is not working on Windows due an issue with CounterStrikeSharp's
            // DynamicHooks. See: https://github.com/roflmuffin/CounterStrikeSharp/issues/377
            VirtualFunctions.GiveNamedItemFunc.Hook(OnGiveNamedItemPost, HookMode.Post);
        }

        RegisterListener<Listeners.OnEntityCreated>(OnEntityCreated);
        RegisterListener<Listeners.OnTick>(OnTick);
    }

    [GameEventHandler]
    public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo _)
    {
        var player = @event.Userid;
        if (!IsPlayerHumanAndValid(player))
            return HookResult.Continue;

        var steamId = player.SteamID;
        FetchPlayerInventory(steamId);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo _)
    {
        var player = @event.Userid;
        if (!IsPlayerHumanAndValid(player))
            return HookResult.Continue;

        var steamId = player.SteamID;
        FetchPlayerInventory(steamId);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo _)
    {
        var player = @event.Userid;
        if (!IsPlayerHumanAndValid(player) || !IsPlayerPawnValid(player))
            return HookResult.Continue;

        var inventory = GetPlayerInventory(player);
        GivePlayerAgent(player, inventory);
        GivePlayerGloves(player, inventory);
        GivePlayerPin(player, inventory);

        return HookResult.Continue;
    }

    [GameEventHandler(HookMode.Pre)]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
    {
        var attacker = @event.Attacker;
        if (!IsPlayerHumanAndValid(attacker) || !IsPlayerPawnValid(attacker))
            return HookResult.Continue;

        var victim = @event.Userid;
        if ((StatTrakIgnoreBotsCvar.Value ? !IsPlayerHumanAndValid(victim) : !IsPlayerValid(victim)) || !IsPlayerPawnValid(victim))
            return HookResult.Continue;

        GivePlayerWeaponStatTrakIncrease(attacker, @event.Weapon, @event.WeaponItemid);

        return HookResult.Continue;
    }

    [GameEventHandler(HookMode.Pre)]
    public HookResult OnRoundMvp(EventRoundMvp @event, GameEventInfo _)
    {
        var player = @event.Userid;
        if (!IsPlayerHumanAndValid(player) || !IsPlayerPawnValid(player))
            return HookResult.Continue;

        GivePlayerMusicKitStatTrakIncrease(player);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo _)
    {
        var player = @event.Userid;
        if (IsPlayerHumanAndValid(player))
        {
            RemovePlayerInventory(player.SteamID);
            ClearInventoryManager();
        }

        return HookResult.Continue;
    }

    public void OnTick()
    {
        // According to @bklol the right way to change the Music Kit is to update the player's inventory, I'm
        // pretty sure that's the best way to change anything inventory-related, but that's not something
        // public and we brute force the setting of the Music Kit here.
        foreach (var player in Utilities.GetPlayers())
            GivePlayerMusicKit(player);
    }

    public void OnEntityCreated(CEntityInstance entity)
    {
        var designerName = entity.DesignerName;
        if (designerName.Contains("weapon"))
        {
            Server.NextFrame(() =>
            {
                var weapon = new CBasePlayerWeapon(entity.Handle);
                if (!weapon.IsValid || weapon.OriginalOwnerXuidLow == 0) return;

                var player = Utilities.GetPlayerFromSteamId((ulong)weapon.OriginalOwnerXuidLow);
                if (player == null || !IsPlayerHumanAndValid(player)) return;

                GivePlayerWeaponSkin(player, weapon);
            });
        }
    }

    public HookResult OnGiveNamedItemPost(DynamicHook hook)
    {
        var className = hook.GetParam<string>(1);
        if (!className.Contains("weapon"))
            return HookResult.Continue;

        var itemServices = hook.GetParam<CCSPlayer_ItemServices>(0);
        var weapon = hook.GetReturn<CBasePlayerWeapon>();
        var player = GetPlayerFromItemServices(itemServices);

        if (player != null)
            GivePlayerWeaponSkin(player, weapon);

        return HookResult.Continue;
    }
}
