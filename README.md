# About!
Allows client devs to send udon events via vrc

###### SendNetworkEventTargeted:
```cs
UdonBehaviour Example = GameObject.Find("Object")?.GetComponent<UdonBehaviour>();
CustomNetworkEventApi.SendNetworkEventTargeted(Example, "eventName", selectedPlayer);
```

###### SendNetworkEventAll:
```cs
UdonBehaviour Example = GameObject.Find("Object")?.GetComponent<UdonBehaviour>();
CustomNetworkEventApi.SendNetworkEventTargeted(Example, "eventName");
```

###### SendByEventByNameTargeted:
```cs
CustomNetworkEventApi.SendByEventByNameTargeted("eventName", selectedPlayer);
```

###### SendByEventByNameAll:
```cs
CustomNetworkEventApi.SendByEventByNameAll("eventName");
```

###### SendByEventByObjectTargeted:
```cs
CustomNetworkEventApi.SendByEventByObjectTargeted("eventName", "objectName", selectedPlayer);
```

###### SendByEventByObjectAll:
```cs
CustomNetworkEventApi.SendByEventByObjectAll("eventName", "objectName");
```

## Jar Based Games:

###### SendJarGamesEventTargeted:
```cs
 CustomNetworkEventApi.SendJarGamesEventTargeted("eventName", selectedPlayer);
```

###### SendJarGamesEventAll:
```cs
CustomNetworkEventApi.SendJarGamesEventAll("eventName");
```

###### BringObjectToPlayer:
```cs
CustomNetworkEventApi.BringObjectToPlayer("objectName", selectedPlayer);
```

###### BringObjectToSelf:
```cs
CustomNetworkEventApi.BringObjectToSelf("objectName");
```


## Get Object / Events / Network Events

###### GetGameObjects:
```cs
var objects = CustomNetworkEventApi.GetGameObjects();
```

###### GetEvents:
```cs
var events = CustomNetworkEventApi.GetEvents();
```

###### GetNetworkedEvents:
```cs
MelonCoroutines.Start(CustomNetworkEventApi.GetNetworkedEvents());
```

