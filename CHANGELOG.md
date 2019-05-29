# CHANGELOG

This is the changelog of the Rhisis project. All notable changes to this project will be documented in this file.

## [Unreleased]

## 0.3.0 - 2019-06-30

...

## [Released]

## [0.2.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.2) - 2019-05-31

### World Server

#### Fixes

- Fix real-time position calculation (PR [#200](https://github.com/Eastrall/Rhisis/pull/200))
- Prevent dialogs from having duplicate links id. (PR [#213](https://github.com/Eastrall/Rhisis/pull/213))
- Fix NPC loading (PR [#220](https://github.com/Eastrall/Rhisis/pull/220))

#### Features

- Pick-up drop items (PR [#203](https://github.com/Eastrall/Rhisis/pull/203))
- Drop items from inventory (PR [#203](https://github.com/Eastrall/Rhisis/pull/203))
- WSAD movements and Jump behavior (PR [#204](https://github.com/Eastrall/Rhisis/pull/204))
- Add multiple dialog texts on DialogLinks. Allow next button on game (PR [#207](https://github.com/Eastrall/Rhisis/pull/207))
- Add multiple dialog texts on introduction text. (PR [#215](https://github.com/Eastrall/Rhisis/pull/215))
- Death System and resurection (PR [#206](https://github.com/Eastrall/Rhisis/pull/206))
- Experience and Level up (PR [#214](https://github.com/Eastrall/Rhisis/pull/214), [#218](https://github.com/Eastrall/Rhisis/pull/218))
- Experience loss on death (PR [#216](https://github.com/Eastrall/Rhisis/pull/216))
- Teleport system (PR [#222](https://github.com/Eastrall/Rhisis/pull/222))

#### Resources

- Add Flarine dialogs (PR [#197](https://github.com/Eastrall/Rhisis/pull/197), [#199](https://github.com/Eastrall/Rhisis/pull/199), [#202](https://github.com/Eastrall/Rhisis/pull/202), [#209](https://github.com/Eastrall/Rhisis/pull/209))

#### Changes

- `MailShippingCost` configuration is now inside the `MailConfiguration` structure.
- Review of database assembly (PR [#208](https://github.com/Eastrall/Rhisis/pull/208))
- CLI code refactoring (PR [#217](https://github.com/Eastrall/Rhisis/pull/217))

## [0.1.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.1) - 2019-03-23

### Added

[Core]
- Configuration structures
- Rijndael cryptography algorithm
- Custom exceptions
- Logger
- FlyFF Network packet handler
- ISC structures and packet headers

[Database]
- Multi-DB support (MySQL and MsSQL)

[Login]
- ISC authentication process
- Client authentication process
- Send server list to connected client

[Cluster]
- Inter-Server authentication (ISC)
- Character list
- Create character
- Delete character
- 2nd password verification
- Pre join

[World]
- Inter-Server authentication (ISC)
- Entity Component System architecture
- Connect to the world
- Load resources
   - Defines & texts
   - Movers
   - Maps
   - NPC Data/Shops/Dialogs
- Spawn monsters and NPC
- Visibility System
- Mobility System
- Chat System
	- Chat commands:
		- Create item : `/ci` or `/createitem`
		- Get gold : `/getgold`
		- Teleport : `/teleport`
- Inventory System
	- Move items
	- Equip/Unequip items
	- Save inventory
- Shop System
	- Buy items
	- Sell items
- Trade System
- NPC Dialog System
- MailBox System
- Drop System
- Battle System
	- Melee Attack
		- Player VS Monster
	- Monster death
	- Monster item/gold drop
- Character customization system
