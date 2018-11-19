using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCallbacks : Bolt.GlobalEventListener
{

    public override void SceneLoadLocalDone(string map)
    {
        if (BoltNetwork.isServer)
        {
            BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(43, 1, 11), Quaternion.identity);
            entity.TakeControl();
        }
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        if (BoltNetwork.isServer)
        {
            BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(43, 1, 11), Quaternion.identity);
            entity.AssignControl(connection);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
