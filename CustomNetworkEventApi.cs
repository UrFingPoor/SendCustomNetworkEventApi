using VRC;
using VRC.Udon;
using VRC.SDKBase;
using MelonLoader;
using UnityEngine;
using VRC.Udon.Common.Interfaces;
using Il2CppSystem.Collections.Generic;
using System.Collections;

namespace YourClient
{
    static class CustomNetworkEventApi
    {
        public static GameObject[] GameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        public static UdonBehaviour[] udonBehaviours = Object.FindObjectsOfType<UdonBehaviour>();

        /// <summary>
        /// Gets a list of all the GameObjects
        /// </summary>
        public static List<GameObject> GetUdonBehaviourGameObjects()
        {
            List<GameObject> udonBehaviours = new List<GameObject>();
            GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
            udonBehaviours.Clear();
            foreach (GameObject gameObject in gameObjects) if (gameObject.GetComponent<UdonBehaviour>() != null) udonBehaviours.Add(gameObject);
            return udonBehaviours;
        }

        /// <summary>
        /// Gets a list of UDON events.
        /// </summary>
        public static List<KeyValuePair<string, List<uint>>> GetEvents(UdonBehaviour udonBehaviour)
        {
            List<KeyValuePair<string, List<uint>>> events = new List<KeyValuePair<string, List<uint>>>();
            if (udonBehaviour == null) return null;
            events.Clear();
            foreach (KeyValuePair<string, List<uint>> udonEvent in udonBehaviour?._eventTable) events.Add(udonEvent);
            return events;
        }

        /// <summary>
        /// Gets a list of all networked GameObjects
        /// </summary>
        public static IEnumerator GetNetworkedEvents()
        {
            for (int i = 0; i < udonBehaviours.Length; i++)
            {
                foreach (string EventName in udonBehaviours[i]._eventTable.Keys)
                {
                    if (!EventName.Contains("_")) MelonLogger.Log($"Event: {EventName} | Object: {udonBehaviours[i].gameObject.name}");
                }
                yield return new WaitForSeconds(0.12f);
            }
            MelonLogger.Log($"===== End OF {RoomManager.field_Internal_Static_ApiWorld_0.name} ====="); yield break;
        }

        /// <summary>
        /// Sets ownership of a gameobject
        /// </summary>
        public static void SetOwnership(this GameObject gameObject, Player target)
        {
            if (GrabOwner(gameObject) != target) Networking.SetOwner(target.field_Private_VRCPlayerApi_0, gameObject);
        }

        /// <summary>
        /// Checks if owner then returns the player object. 
        /// </summary>
        public static Player GrabOwner(this GameObject gameObject)
        {
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                if (player.field_Private_VRCPlayerApi_0.IsOwner(gameObject)) return player;
            }
            return null;
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent
        /// </summary>
        public static void SendNetworkEventAll(UdonBehaviour udonBehaviour, string eventName)
        {
            udonBehaviour?.SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent
        /// </summary>
        public static void SendNetworkEventTargeted(UdonBehaviour udonBehaviour, string eventName, Player target)
        {
            SetOwnership(udonBehaviour.gameObject, target);
            udonBehaviour?.SendCustomNetworkEvent(NetworkEventTarget.Owner, eventName);
            MelonLogger.Log($"Making {target.prop_APIUser_0.displayName} Call Event: {eventName}");
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent By eventname To All 
        /// </summary>
        public static void SendByEventByNameAll(string eventName)
        {         
            for (int i = 0; i < udonBehaviours.Length; i++)
            {
                foreach (string name in udonBehaviours[i]._eventTable.Keys)
                {
                    if (eventName == name) 
                    {
                        udonBehaviours[i].SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
                        MelonLogger.Log($"Calling Event: {eventName} | From Object: {udonBehaviours[i].gameObject.name}");
                    }
                }
            }
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent By eventname To Target
        /// </summary>
        public static void SendByEventByNameTargeted(string eventName, Player target)
        {
            for (int i = 0; i < udonBehaviours.Length; i++)
            {
                foreach (var Events in udonBehaviours[i]._eventTable.Keys)
                {
                    if (eventName == Events)
                    {
                        SetOwnership(udonBehaviours[i].gameObject, target);
                        udonBehaviours[i].SendCustomNetworkEvent(NetworkEventTarget.Owner, eventName);
                        MelonLogger.Log($"Making {target.prop_APIUser_0.displayName} Call Event: {eventName} | From Object: {udonBehaviours[i].gameObject.name}");
                    }
                }
            }
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent By Object To All
        /// </summary>
        public static void SendByEventByObjectAll(string eventName, string objectName)
        {
            for (int i = 0; i < GameObjects.Length; i++)
            {
                if (objectName == GameObjects[i].gameObject.name)
                {
                    UdonBehaviour UdonBehaviour = GameObjects[i].transform.gameObject.GetComponentInChildren<UdonBehaviour>(GameObjects[i].gameObject.TryCast<GameObject>());
                    UdonBehaviour.SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
                }
            }
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent By Object To Target
        /// </summary>
        public static void SendByEventByObjectTargeted(string eventName, string objectName, Player target)
        {
            for (int i = 0; i < GameObjects.Length; i++)
            {
                if (objectName == GameObjects[i].gameObject.name)
                {
                    SetOwnership(udonBehaviours[i].gameObject.gameObject, target);
                    UdonBehaviour UdonBehaviour = GameObjects[i].transform.gameObject.GetComponentInChildren<UdonBehaviour>(GameObjects[i].gameObject.TryCast<GameObject>());
                    UdonBehaviour.SendCustomNetworkEvent(NetworkEventTarget.Owner, eventName);
                    MelonLogger.Log($"Making {target.prop_APIUser_0.displayName} Call Event: {eventName} | From Object: {udonBehaviours[i].gameObject.name}");
                }
            }
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent Targeted Jar Created GameModes
        /// </summary>
        public static void SendJarGamesEventTargeted(string eventName, Player target)
        {
            foreach (Transform player in GameObject.Find("Player Nodes").GetComponentsInChildren<Transform>())
            {
                if (player.name != GameObject.Find("Player Nodes").name)
                {
                    SetOwnership(player.gameObject, target);
                    player.gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.Owner, eventName);                    
                }
                MelonLogger.Log($"Making {target.prop_APIUser_0.displayName} Call Event: {eventName} | From Object: {player.gameObject.name}"); break;
            }
        }

        /// <summary>
        /// Calls UDON Event via SendCustomNetworkEvent All Jar Created GameModes
        /// </summary>
        public static void SendJarGamesEventAll(string eventName)
        {
            foreach (Transform player in GameObject.Find("Player Nodes").GetComponentsInChildren<Transform>())
            {
                if (player.name != GameObject.Find("Player Nodes").name)
                {
                    player.gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(NetworkEventTarget.All, eventName);                 
                }
                MelonLogger.Log($"Making Everyone Call Event: {eventName} | From Object: {player.gameObject.name}");
            }
        }

        /// <summary>
        ///  Teleports Objects to self
        /// </summary>
        public static void BringObjectToSelf(string ObjectName)
        {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject.name.Contains(ObjectName))
                {
                    Networking.SetOwner(VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0, gameObject);
                    gameObject.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + new Vector3(0f, 0.1f, 0f);
                }
            }
        }

        /// <summary>
        ///  Teleports Objects to target
        /// </summary>
        public static void BringObjectToPlayer(string ObjectName, Player target)
        {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject.name.Contains(ObjectName))
                {
                    SetOwnership(gameObject, target);
                    gameObject.transform.position = target.transform.position + new Vector3(0f, 0.1f, 0f);
                }
            }
        }
    }
}
