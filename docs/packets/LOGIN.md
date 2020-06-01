# Login Server packets

## CERTIFY


```c#
[string] buildVersion <- 32 bits
[string] username
[string] password
```

## PING

```c#
[int] time
```

## ERROR

```c#
[int] errorCode
```

## SRVR_LIST

```c#
[int] 0
[byte] 1
[string] username
[int] serverCount (= clusterCount + worldCount)

for (int i = 0; i < serverCount; ++i)
{
    [int] clusterId // (-1 when it's a ClusterServer)
    [int] serverId
    [string] serverName
    [string] host ip
    [int] 0 // b18, need to figure out what this is
    [int] 0 // number of people connected
    [int] 1 // enabled
    [int] worldCapacity // Just for world servers ??
}
```