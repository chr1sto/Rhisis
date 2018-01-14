# Cluster Server Packets

## GETPLAYERLIST

```c#
[int] header: ClusterHeaders.Incoming.CharacterListRequest (0xF6)
[string] buildDate
[int] authKey
[string] username
[string] password
[int] serverId
```

## CREATE_PLAYER

```c#
[string] username
[string] password
[int] slot
[string] characterName
[byte] faceId
[byte] costumeId
[byte] skinSetId
[byte] hairMeshId
[uint] hairColor
[byte] gender
[byte] job
[byte] headMeshId
[int] bankPassword
[int] authKey
```

## DEL_PLAYER

```c#
[string] username
[string] password
[string] passwordVerification
[int] characterId
[int] authKey
```

## PRE_JOIN

```c#
[string] username
[int] characterId
[string] characterName
[int] bankCode
[int] authKey
```

---