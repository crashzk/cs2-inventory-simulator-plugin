# [cs2.zkservidores.com](https://cs2.zkservidores.com)
A simple plugin for integrating with [cs2.zkservidores.com](https://cs2.zkservidores.com).

##### Fork: [CS2 Inventory Simulator Plugin](https://github.com/ianlucas/cs2-inventory-simulator-plugin)

> [!CAUTION]
> This plugin has not been fully and thoroughly tested. Compatibility with other plugins has also not been tested. Your server can be banned by Valve for using this plugin, [see the server guidelines](https://blog.counter-strike.net/index.php/server_guidelines). Use at your own risk.

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
- Pins

## Installation
1. Make sure `FollowCS2ServerGuidelines` is `false` in `addons/counterstrikesharp/configs/core.json`.
2. [Download the latest release](https://github.com/ianlucas/cs2-inventory-simulator-plugin/releases) of CS2 Inventory Simulator Plugin.
3. Extract the ZIP file contents into `addons/counterstrikesharp`.

## Configuration
#### `css_inventory_simulator` ConVar
* Host of Inventory Simulator's API.
* **Type:** `string`
* **Default:** `cs2.zkservidores.com`

#### `css_inventory_simulator_apikey` ConVar
* API Key for Inventory Simulator.
* **Type:** `string`
* **Default:** _empty_

#### `css_stattrak_ignore_bots` ConVar
* Determines whether to ignore StatTrak increments for bot kills.
* **Type:** `bool`
* **Default:** `true`

#### `css_minmodels` ConVar
* Limits the number of custom models allowed in-game.
* **Type:** `int`
* **Default:** `0`
* **Values:**
	- `0` - All agents allowed.
	- `1` - Default agents for the current map. **Note:** Currently the same as `2` as Valve has not yet added them back.
	- `2` - Only SAS and Phoenix agents allowed.
	