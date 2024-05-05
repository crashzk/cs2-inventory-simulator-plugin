# [cs2.zkservidores.com](https://cs2.zkservidores.com)
A simple plugin for integrating with [cs2.zkservidores.com](https://cs2.zkservidores.com).

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

## Configuration
#### `css_inventory_simulator` 
Host of Inventory Simulator's API.
- **Type:** `string`
- **Default:** `cs2.zkservidores.com`

#### `css_inventory_simulator_apikey`
API Key for Inventory Simulator.
- **Type:** `string`
- **Default:** _empty_

#### `css_stattrak_ignore_bots`
Determines whether to ignore StatTrak increments for bot kills.
- **Type:** `bool`
- **Default:** `true`

#### `css_minmodels`
Limits the number of custom models allowed in-game.
- **Type:** `int`
- **Default:** `0`
- **Values:**
	- `0` - All agents allowed.
	- `1` - Default agents for the current map. **Note:** Same as `2` as Valve has not yet added them back.
	- `2` - Only SAS and Phoenix agents allowed.
	
#### `css_give_custom_music_kit`
Give custom Music Kit to players.
- **Type:** `bool`
- **Default:** `true`

#### `css_give_custom_pin`
Give custom Pin to players.
- **Type:** `bool`
- **Default:** `true`

#### `css_give_custom_gloves`
Give custom Gloves to players.
- **Type:** `bool`
- **Default:** `true`

#### `css_give_custom_agent`
Give custom Agent to players.
- **Type:** `bool`
- **Default:** `true`

#### `css_give_custom_weapon`
Give custom Weapon to players.
- **Type:** `bool`
- **Default:** `true`

#### `css_give_weapon_stattrak_increase`
- **Type:** `bool`
- **Default:** `true`

#### `css_give_music_kit_stattrak_increase`
Give Music Kit StatTrak increase.
- **Type:** `bool`
- **Default:** `true`