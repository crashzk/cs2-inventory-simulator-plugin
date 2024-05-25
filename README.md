# [cs2.zkservidores.com](https://cs2.zkservidores.com)
A simple CounterStrikeSharp plugin for integration with [cs2.zkservidores.com](https://cs2.zkservidores.com).

##### Fork: [CS2 Inventory Simulator Plugin](https://github.com/ianlucas/cs2-inventory-simulator-plugin)

> [!CAUTION]  
> Your server can be banned by Valve for using this plugin (see their [server guidelines](https://blog.counter-strike.net/index.php/server_guidelines)). Use at your own risk.

## Current Features
- Weapon
  - Paint Kit, Wear, Seed, Name tag, StatTrak (with increment), and Stickers.
- Knife
  - Paint Kit, Wear, Seed, Name tag, and StatTrak (with increment).
- Gloves
  - Paint Kit, Wear, Seed.
- Agent
  - Patches.
- Music Kit
  - StatTrak (with increment). 
- Pin

## Configuration
#### `invsim_hostname`
Inventory Simulator API's hostname.
- **Type:** `string`
- **Default:** `cs2.zkservidores.com`

#### `invsim_apikey`
Inventory Simulator API's key.
- **Type:** `string`
- **Default:** _empty_

#### `invsim_stattrak_ignore_bots`
Whether to ignore StatTrak increments for bot kills.
- **Type:** `bool`
- **Default:** `true`

#### `invsim_minmodels`
Allows agents or use specific models for each team.
- **Type:** `int`
- **Default:** `0`
- **Values:**
	- `0` - All agents allowed.
	- `1` - Default agents for the current map. **Note:** Same as `2` as Valve has not yet added them back.
	- `2` - Only SAS and Phoenix agents allowed.

#### `invsim_ws_enabled`
Whether players can refresh their inventory using `!ws` command.
- **Type:** `bool`
- **Default:** `false`

#### `invsim_ws_cooldown`
Cooldown in seconds between player inventory refreshes.
- **Type:** `int`
- **Default:** `30`

## Commands
- `!ws` - Prints Inventory Simulator's website and refreshes player's inventory if `invsim_ws_enabled` ConVar is `true`.