# [cs2.zkservidores.com](https://cs2.zkservidores.com)
A simple CounterStrikeSharp plugin for integration with [cs2.zkservidores.com](https://cs2.zkservidores.com).

##### Fork: [CS2 Inventory Simulator Plugin](https://github.com/ianlucas/cs2-inventory-simulator-plugin)

> [!WARNING]
> Your server can be banned by Valve for using this plugin, [see the server guidelines](https://blog.counter-strike.net/index.php/server_guidelines). Use at your own risk.

## Current Features
- Weapon
  - Paint Kit, Wear, Seed, Name tag, StatTrak (with increment), and Stickers.
- Knife
  - Paint Kit, Wear, Seed, Name tag, and StatTrak (with increment).
- Gloves
  - Paint Kit, Wear, Seed.
- Agents
  - Patches.
- Music Kit
  - StatTrak (with increment). 
- Pins

## Installation
1. Make sure **`FollowCS2ServerGuidelines`** is **`false`** in **`addons/counterstrikesharp/configs/core.json`**.
2. [Download the latest release](https://github.com/ianlucas/cs2-inventory-simulator-plugin/releases) of CS2 Inventory Simulator Plugin.
3. Extract the ZIP file contents into **`addons/counterstrikesharp`**.

## Configuration
#### `invsim_hostname`
Inventory Simulator API's domain.
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