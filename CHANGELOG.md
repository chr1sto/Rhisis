# CHANGELOG

This is the changelog of the Rhisis project. All notable changes to this project will be documented in this file.

## [Unreleased]

## [0.5.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.5) - 2020-05-30

## [Released]

## [0.4.2](https://github.com/Eastrall/Rhisis/releases/tag/v0.4.2) - 2020-05-20

### Fixes

- ![world] Fix string encoding ([#389](https://github.com/Eastrall/Rhisis/pull/389))
- ![world] Fix consumable items ([#392](https://github.com/Eastrall/Rhisis/pull/392))
- ![world] Fix sell item at NPC shops ([#392](https://github.com/Eastrall/Rhisis/pull/392))
- ![world] Sold items doesn't come back to inventory anymore ([#392](https://github.com/Eastrall/Rhisis/pull/392))
- ![world] Fix item equip/unequip process when equiping a dropped item armor ([#392](https://github.com/Eastrall/Rhisis/pull/392))
- ![world] Fix item drop from inventory ([#393](https://github.com/Eastrall/Rhisis/pull/393))

## [0.4.1](https://github.com/Eastrall/Rhisis/releases/tag/v0.4.1) - 2020-05-18

### Fixes

- ![login] Fix server crash when a client logs in with invalid credentials ([#358](https://github.com/Eastrall/Rhisis/pull/358))
- ![world] Fix NPC shops crash ([#374](https://github.com/Eastrall/Rhisis/pull/374))
- ![world] Fix NPC oral dialog texts ([#365](https://github.com/Eastrall/Rhisis/pull/365))
- ![world] Fix NPC shop purchase item with error "User ID already exists" ([#382](https://github.com/Eastrall/Rhisis/pull/382))
- ![world] Fix inventory saving issue ([#372](https://github.com/Eastrall/Rhisis/issues/372))
- ![cluster] Fix character with dual weapons on character selection screen ([#373](https://github.com/Eastrall/Rhisis/issues/373))


## [0.4.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.4) - 2020-04-29

### Fixes

- ![login] Optimize authentication SQL request ([#259](https://github.com/Eastrall/Rhisis/pull/259))
- ![world] Fix inventory decrease item ([#278](https://github.com/Eastrall/Rhisis/pull/278))
- ![world] Fix teleport bug for visible entities ([#283](https://github.com/Eastrall/Rhisis/pull/283))
- ![world] Rework inventory system ([#]())
- ![common] Update WorldServer default port ([#282](https://github.com/Eastrall/Rhisis/pull/282))

### Features

- ![world] Delete item from inventory ([#278](https://github.com/Eastrall/Rhisis/pull/278))
- ![world] Add create monster admin command ([#280](https://github.com/Eastrall/Rhisis/pull/280))
- ![world] Gives to himself exp via Debug Panel as a GM/Admin ([#281](https://github.com/Eastrall/Rhisis/pull/281))
- ![world] Quest system ([#292](https://github.com/Eastrall/Rhisis/pull/292))
- ![world] Skill sytem ([#333](https://github.com/Eastrall/Rhisis/pull/333)) ([#335](https://github.com/Eastrall/Rhisis/pull/335))
- ![world] Job System with job change ([#342](https://github.com/Eastrall/Rhisis/pull/342))
- ![world] Add new GM commands
  - OneKill / NoOneKill command ([#322](https://github.com/Eastrall/Rhisis/pull/322))
  - Around Kill ([#323](https://github.com/Eastrall/Rhisis/pull/323)
  - Summon command ([#324](https://github.com/Eastrall/Rhisis/pull/324))
  - Undying (God Mode) command ([#325](https://github.com/Eastrall/Rhisis/pull/325))
  - Freeze command ([#328](https://github.com/Eastrall/Rhisis/pull/328))
  - System message command (([#329](https://github.com/Eastrall/Rhisis/pull/329))
  - Count command (counts the connected players) ([#330](https://github.com/Eastrall/Rhisis/pull/330))
  - Invisible command ([#331](https://github.com/Eastrall/Rhisis/pull/331))
  - Exp Up stop command ([#332](https://github.com/Eastrall/Rhisis/pull/332))

### BREAKING CHANGES

- ![login] Refactoring of the `LoginServer` with a [`HostBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=aspnetcore-2.2) and add `HandlerInvoker` system. ([#258](https://github.com/Eastrall/Rhisis/pull/258))
- ![cluster] Refactoring of the `ClusterServer` with a [`HostBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=aspnetcore-2.2) and add `HandlerInvoker` system. ([#259](https://github.com/Eastrall/Rhisis/pull/259))
- ![world] Refactoring of the `WorldServer` with a [`HostBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuilder?view=aspnetcore-2.2) and add `HandlerInvoker` system. ([#263](https://github.com/Eastrall/Rhisis/pull/263))
	- Refactoring systems
	- Refactoring packet factories
	- Refactoring resource loaders
- ![database] Remove repository pattern ([#347](https://github.com/Eastrall/Rhisis/pull/347))
  - `IDatabase` renamed into `IRhisisDatabase`
  - Related data should be loaded manually when needed using the `IQueryable<TEntity>.Include()` method from Entity Framework Core : https://docs.microsoft.com/en-us/ef/core/querying/related-data

## [0.3.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.3) - 2019-06-30

### Fixes

- ![world] Fix equipement bug for female gender ([#247](https://github.com/Eastrall/Rhisis/pull/247))
- ![world] Fix item drop and moving in inventory ([#248](https://github.com/Eastrall/Rhisis/pull/248))
- ![world] Fix player death bug ([#249](https://github.com/Eastrall/Rhisis/pull/249))
- ![tools] Fix CLI `setup` and `database update` commands ([#250](https://github.com/Eastrall/Rhisis/pull/250))

### Features

- ![world] Blinkwing item usage ([#251](https://github.com/Eastrall/Rhisis/pull/251), [#253](https://github.com/Eastrall/Rhisis/pull/253))
- ![world] Delayer system: Delay actions with a given amount of time. ([#253](https://github.com/Eastrall/Rhisis/pull/253))
- ![world] Wrapzone system ([#254](https://github.com/Eastrall/Rhisis/pull/254))

## [0.2.1](https://github.com/Eastrall/Rhisis/releases/tag/v0.2.1) - 2019-06-18

### Fixes

- ![world] Fix `NullReferenceException` when a player attacks a monster without a weapon ([#231](https://github.com/Eastrall/Rhisis/pull/231))
- ![cluster] Fix ClusterServer weapon display ([#239](https://github.com/Eastrall/Rhisis/pull/239))
- ![world] Fix Respawn system ([#242](https://github.com/Eastrall/Rhisis/pull/242))
- ![world] Fix dropped items bonuses ([#243](https://github.com/Eastrall/Rhisis/pull/243)

### Features

- ![world] Attribute system ([#237](https://github.com/Eastrall/Rhisis/pull/237)
- ![world] Food, potions and refreshers items usage ([#241](https://github.com/Eastrall/Rhisis/pull/241))

### Resources

- Add Flarine dialogs ([#233](https://github.com/Eastrall/Rhisis/pull/233))

### Changes

- Remove SQL Server support ([#232](https://github.com/Eastrall/Rhisis/pull/232))


## [0.2.0](https://github.com/Eastrall/Rhisis/releases/tag/v0.2) - 2019-05-29

### World Server

#### Fixes

- Fix real-time position calculation ([#200](https://github.com/Eastrall/Rhisis/pull/200))
- Prevent dialogs from having duplicate links id. ([#213](https://github.com/Eastrall/Rhisis/pull/213))
- Fix NPC loading ([#220](https://github.com/Eastrall/Rhisis/pull/220))

#### Features

- Pick-up drop items ([#203](https://github.com/Eastrall/Rhisis/pull/203))
- Drop items from inventory ([#203](https://github.com/Eastrall/Rhisis/pull/203))
- WSAD movements and Jump behavior ([#204](https://github.com/Eastrall/Rhisis/pull/204))
- Add multiple dialog texts on DialogLinks. Allow next button on game ([#207](https://github.com/Eastrall/Rhisis/pull/207))
- Add multiple dialog texts on introduction text. ([#215](https://github.com/Eastrall/Rhisis/pull/215))
- Death System and resurection ([#206](https://github.com/Eastrall/Rhisis/pull/206))
- Experience and Level up ([#214](https://github.com/Eastrall/Rhisis/pull/214), [#218](https://github.com/Eastrall/Rhisis/pull/218))
- Experience loss on death ([#216](https://github.com/Eastrall/Rhisis/pull/216))
- Teleport system ([#222](https://github.com/Eastrall/Rhisis/pull/222))

#### Resources

- Add Flarine dialogs ([#197](https://github.com/Eastrall/Rhisis/pull/197), [#199](https://github.com/Eastrall/Rhisis/pull/199), [#202](https://github.com/Eastrall/Rhisis/pull/202), [#209](https://github.com/Eastrall/Rhisis/pull/209), [#225](https://github.com/Eastrall/Rhisis/pull/225))

#### Changes

- `MailShippingCost` configuration is now inside the `MailConfiguration` structure.
- Review of database assembly ([#208](https://github.com/Eastrall/Rhisis/pull/208))
- CLI code refactoring ([#217](https://github.com/Eastrall/Rhisis/pull/217))

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

[world]: https://img.shields.io/badge/-world-brightgreen.svg "world-component"
[cluster]: https://img.shields.io/badge/-cluster-brightgreen.svg "cluster-component"
[login]: https://img.shields.io/badge/-login-brightgreen.svg "login-component"
[tools]: https://img.shields.io/badge/-tools-brightgreen.svg "tools-component"
[common]: https://img.shields.io/badge/-common-brightgreen.svg "common-components"
[database]: https://img.shields.io/badge/-database-brightgreen.svg "database-component"
