<h1 align="center">
  Rhisis - Fly For Fun V15 Emulator
  <br>
</h1>

<p align="center">
  <a href="http://forthebadge.com"><img src="http://forthebadge.com/images/badges/made-with-c-sharp.svg" alt="Made With C#"></a>
  <a href="http://forthebadge.com"><img src="http://forthebadge.com/images/badges/built-with-love.svg"></a><br>
</p>

<h4>Built with C# 8 and the <a href="https://dotnet.microsoft.com/download/dotnet-core" target="_blank">.NET Core Framework 3.0</a>.</h4>

<p>This project has been created for learning purposes about the network and game logic problematics on the server-side.<br>
We choose to use the <a href="https://github.com/Eastrall/Sylver.Network">Sylver.Network</a> to manage our server connecitions because it provides a clients management system and also a robust packet management system entirely customisable.</p>

<h4 align="center">:warning: This project is not affiliated with Gala Lab. :warning:</h4><br>

<p align="center">
  <a href="https://travis-ci.org/Eastrall/Rhisis"><img src="https://travis-ci.org/Eastrall/Rhisis.svg?branch=develop"></a>
  <a href="https://www.codacy.com/app/Eastrall/Rhisis?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Eastrall/Rhisis&amp;utm_campaign=Badge_Grade"><img src="https://api.codacy.com/project/badge/Grade/500148ec8bdd4f2e954f11c682c39f3c"></a>
  <a href="https://codecov.io/gh/Eastrall/Rhisis"><img src="https://codecov.io/gh/Eastrall/Rhisis/branch/develop/graph/badge.svg" /></a>
  <a href="https://github.com/Eastrall/Rhisis/commits/develop"><img src="https://img.shields.io/github/last-commit/Eastrall/Rhisis.svg?style=flat-square&logo=github&logoColor=white" alt="GitHub last commit"></a>
  <a href="https://discord.gg/zAT6Az2"><img src="https://discordapp.com/api/guilds/294405146300121088/widget.png"></a>
</p>
	    
<p align="center">
  <a href="#technical-information">Technical information</a> •
  <a href="#features">Features</a> •
  <a href="https://github.com/Eastrall/Rhisis/tree/develop/docs/howtos">How To's</a> •
  <a href="#contributing">Contributing</a> •
  <a href="#contributors">Contributors</a> •
  <a href="#supporters">Supporters</a> •
  <a href="https://github.com/Eastrall/Rhisis/blob/develop/LICENSE">License</a>
</p>

<p align="center"><img src="https://i.imgur.com/wpfB1VZ.gif"></p>

## Technical information

- Language: `C#` 8 (latest)
- Framework: `.NET Core 3.0`
- Application type: `Console`
- Database type: `MySQL`
- Configuration files type: `JSON`
- External libraries used:
	- [Sylver.Network][sylvernetwork]
	- [Sylver.HandlerInvoker](https://github.com/Eastrall/Sylver.HandlerInvoker)
	- [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore)
	- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
	- [NLog](https://github.com/NLog/NLog)
- Environment: Visual Studio 2019

## Features

### Common
- [x] Logger
- [x] Rijndael cryptography algorithm
- [x] Custom exceptions
- [x] Packet handler

### Database
- [x] MySQL database support

### Login
- [x] Inter-Server authentication process (CoreServer)
- [x] Client authentication process
- [x] Send server list to connected client

### Cluster
- [x] Inter-Server authentication (CoreClient)
- [x] Character list
- [x] Create character
- [x] Delete character
- [x] 2nd password verification
- [x] Pre join

### World
- [x] Inter-Server authentication (CoreClient)
- [x] Entity Component System architecture
- [x] Connect to the world
- [x] Load resources
   - [x] Defines & texts
   - [x] Monsters
   - [x] Maps
   - [x] Items
   - [x] NPC Data/Shops/Dialogs
   - [x] Job Data
   - [x] Exp table
   - [x] Behaviors (AI)
- [x] Spawn monsters and NPC
- [x] Visibility System
- [x] Mobility System
- [x] Respawn System
- [x] Chat System
	- [x] Chat commands:
		- [x] Create item : `/ci` or `/createitem`
		- [x] Get gold : `/getgold`
		- [x] Teleport : `/teleport`
- [x] Inventory System
	- [x] Move items
	- [x] Equip/Unequip items
	- [x] Save inventory
	- [x] Drop items on the ground
	- [x] Item usage (food, potion, refreshers)
- [x] Shop System
	- [x] Buy items
	- [x] Sell items
- [x] Trade System
- [x] NPC Dialog System
- [x] Drop System
	- [x] Pickup Gold / Items
- [x] Battle System
	- [x] Melee Attack
		- [x] Player VS Monster
	- [x] Monster death
	- [x] Monster item/gold drop
- [x] Character customization system
- [x] Attribute System
- [x] Quest System
- [ ] [Bank System](https://github.com/Eastrall/Rhisis/issues/309)
- [ ] [Friend System](https://github.com/Eastrall/Rhisis/issues/37)
- [ ] [Motion System](https://github.com/Eastrall/Rhisis/issues/82)
- [ ] [Buff Pang System](https://github.com/Eastrall/Rhisis/issues/39)
- [ ] [Mailbox System](https://github.com/Eastrall/Rhisis/issues/38)
- [ ] [Guild System](https://github.com/Eastrall/Rhisis/issues/36)
- [ ] [Skill System](https://github.com/Eastrall/Rhisis/issues/35)
- [ ] [Item Bonus System](https://github.com/Eastrall/Rhisis/issues/34)
- [ ] [Party System](https://github.com/Eastrall/Rhisis/issues/33)
- [ ] [Job System](https://github.com/Eastrall/Rhisis/issues/31)

## Contributing

Please take a look at our [contributing](https://github.com/Eastrall/Rhisis/blob/develop/CONTRIBUTING.md) guidelines, if you're interested in helping!

## Contributors

- [Eastrall](https://github.com/Eastrall)
- [Steve-Nzr](https://github.com/Steve-Nzr)
- [Freezeraid](https://github.com/Freezeraid)
- [Skeatwin](https://github.com/Skeatwin)
- [johmarjac](https://github.com/johmarjac)
- [Kaev](https://github.com/Kaev)
- [YarinNet](https://github.com/YarinNet)
- [Almewty](https://github.com/Almewty)
- [Anjuts](https://github.com/Anjuts)
- [MarkWilds](https://github.com/MarkWilds)
- [tekinomikata](https://github.com/tekinomikata)

## Supporters

- Ukiyo
- Kinami
- Sauce

[sylvernetwork]: https://github.com/Eastrall/Sylver.Network