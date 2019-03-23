# Rhisis packets

In this documentation, you will find every FlyFF packet structure used in Rhisis emulator.

## Packet list

### Login

| Packet Name | Packet Value | From | To | Description |
| ----------- | ------------ | ----------- | ----------- | ----------- |
| [CERTIFY](/docs/packets/LOGIN.md#certify) | `0x000000FC` | Client | Login Server |The client sends to the server a login request. |
| [PING](/docs/packets/LOGIN.md#ping) | `0x00000014` | Client | Login Server | The client sends a ping request to the Login Server. |
| [SRVR_LIST](/docs/packets/LOGIN.md#srvr_list) | `0x000000FD` | Login Server  | Client | Send the list of available servers to the client. |
| [ERROR](/docs/packets/LOGIN.md#error) | `0x000000FE` | Login Server  | Client | Send an error message to the client. |


### Cluster

| Packet Name | Packet Value | From | To | Description |
| ----------- | ------------ | ----------- | ----------- | ----------- |
| [PING](/docs/packets/Cluster.md#ping) | `0x00000014` | Client | Cluster Server | The client sends a ping request to the Cluster Server. |
| [GETPLAYERLIST](/docs/packets/CLUSTER.md#getplayerlist) | `0x000000F6` | Client | Cluster Server | The client sends a request to get the player list. | 
| [CREATE_PLAYER](/docs/packets/CLUSTER.md#create_player) | `0x000000F4` | Client | Cluster Server | The clients sends a request to create a character. |
| [DEL_PLAYER](/docs/packets/CLUSTER.md#del_player) | `0x000000F5` | Client | Cluster Server | The clients sends a request to delete a character. |
| [PRE_JOIN](/docs/packets/CLUSTER.md#pre_join) | `0x0000FF05` | Client | Cluster Server | The clients sends a request that he wants to join the game. |


### World

| Packet Name | Packet Value | From | To | Description |
| ----------- | ------------ | ----------- | ----------- | ----------- |