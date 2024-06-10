﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public void LoadPlayerInventories()
    {
        try
        {
            var path = Path.Combine(Server.GameDirectory, InventoryFilePath);
            if (!File.Exists(path))
                return;

            string json = File.ReadAllText(path);
            var inventories = JsonConvert.DeserializeObject<Dictionary<ulong, PlayerInventory>>(json);
            if (inventories != null)
            {
                foreach (var pair in inventories)
                {
                    LoadedPlayerInventory.Add(pair.Key);
                    AddPlayerInventory(pair.Key, pair.Value);
                }
            }
        }
        catch
        {
            Logger.LogError($"Error when processing \"inventories.json\".");
        }
    }

    public void AddPlayerInventory(ulong steamId, PlayerInventory inventory)
    {
        PlayerInventoryManager[steamId] = inventory;
        if (inventory.MusicKit != null)
            PlayerMusicKitManager[steamId] = inventory.MusicKit;
        else PlayerMusicKitManager.Remove(steamId);
    }

    public void ClearInventoryManager()
    {
        var connected = Utilities.GetPlayers().Select(player => player.SteamID).ToHashSet();
        var disconnected = PlayerInventoryManager.Keys.Except(connected).ToList();
        foreach (var steamId in disconnected)
        {
            RemovePlayerInventory(steamId);
        }
    }

    public void RemovePlayerInventory(ulong steamId)
    {
        if (!LoadedPlayerInventory.Contains(steamId))
        {
            PlayerInventoryManager.Remove(steamId);
            PlayerMusicKitManager.Remove(steamId);
        }
    }

    public PlayerInventory GetPlayerInventory(CCSPlayerController player)
    {
        if (PlayerInventoryManager.TryGetValue(player.SteamID, out var inventory))
        {
            return inventory;
        }
        return EmptyInventory;
    }
}
