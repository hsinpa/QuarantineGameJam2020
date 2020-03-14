using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConnectVillageSO : ScriptableObject
{
    [SerializeField]
    private List<ConnectionPair> connections = new List<ConnectionPair>();

    public List<string> FindConnectionPairID(string p_self_villageID) {

        List<string> villagePair = new List<string>();

        foreach (var connection in connections) {
            if (connection.village_left_id == p_self_villageID)
                villagePair.Add(connection.village_right_id);

            if (connection.village_right_id == p_self_villageID)
                villagePair.Add(connection.village_left_id);
        }

        return villagePair;
    }

    [System.Serializable]
     public struct ConnectionPair {
        public string village_left_id;
        public string village_right_id;
    }
}
