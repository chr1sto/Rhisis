# Rhisis

[![forthebadge](http://forthebadge.com/images/badges/made-with-c-sharp.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)

[![Build Status](https://travis-ci.org/Eastrall/Rhisis.svg?branch=develop)](https://travis-ci.org/Eastrall/Rhisis)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/500148ec8bdd4f2e954f11c682c39f3c)](https://www.codacy.com/app/Eastrall/Rhisis?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Eastrall/Rhisis&amp;utm_campaign=Badge_Grade)
[![discord](https://discordapp.com/api/guilds/294405146300121088/widget.png)](https://discord.gg/zAT6Az2)

Rhisis is a FlyForFun V15 emulator built with C# 7 and the .NET Core Framework 2.0.

This project has been created for learning purposes about the network and game logic problematics on the server-side. Also, this is a rework of the Hellion emulator.

We choose to use the [Ether.Network][ethernetwork] because it provides a clients management system and also a robust packet management system entirely customisable.

## Details

- Language: `C#` 7
- Framework: `.NET Core 2.0`
- Application type: `Console`
- Database type: `MsSQL Express` or `MySQL`
- Configuration files type: `JSON`
- External libraries used:
	- [Ether.Network][ethernetwork]
	- Entity Framework Core
- Environment: Visual Studio 2017

## Features

### Common
- Logger
- Rijndael cryptography algorithm
- Custom exceptions
- Packet handler

### Database
- Multi-DB support (MySQL and MsSQL)

### Login
- Inter-Server authentication process (ISC)
- Client authentication process
- Send server list to connected client

### Cluster
- Inter-Server authentication (ISC)
- Character list
- Create character
- Delete character
- 2nd password verification
- Pre join

### World
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


## How to setup Rhisis (from `develop` branch) (Windows platform)

1. Download or Clone the `develop` branch
2. Install the .NET Core SDK 2.0 : https://www.microsoft.com/net/download/windows
3. Install `MsSQL Express` or `MySQL Server`
4. Edit the files in `bin/config/` (`database.json`, `login.json`, `cluster.json`, `world.json`)
5. Create an account in your database (tool available soon)
6. Start the emulator
- Start `login.bat`
- Start `cluster.bat`
- Start `world.bat`

## Contributors

- Eastrall
- Steve-Nzr
- Freezeraid
- Skeatwin
- Yothri

## Supporters

- Ukiyo
- Kinami
- Sauce

[ethernetwork]: https://github.com/Eastrall/Ether.Network
