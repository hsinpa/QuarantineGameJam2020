using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;
using Utility;
public class ActionHandler
{
    public int currentAP = 0;

    VillageManager villageManager;
    GameManager gameManager;

    private Dictionary<string, float> ActionDict;

    private List<ActionStat> actionStatList;

    public ActionHandler(VillageManager villageManager, GameManager gameManager) {
        ActionDict = new Dictionary<string, float>();
        this.gameManager = gameManager;
        this.villageManager = villageManager;
        actionStatList = new List<ActionStat>();
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Quarantine, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Lab, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Cure, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Investigate, apCost = 2 });

        Reset();
    }

    public bool CheckActionValid(string action_id, string village_id = "") {
        string combineID = CombineID(action_id, village_id);
        ActionStat pickAction = actionStatList.Find(x => x.ID == action_id);
        if (pickAction.ID == null) return false;

        if (action_id == StatFlag.ActionStat.Cure)
            return gameManager.turn_count - GetValue(action_id, village_id) > 1 && currentAP >= pickAction.apCost;

        return currentAP >= pickAction.apCost;
    }

    public void ExecuteAction(string action_id, string village_id = null) {
        ActionStat pickAction = actionStatList.Find(x => x.ID == action_id);
        if (pickAction.ID == null) return;

        string combineID = CombineID(action_id, village_id);
        switch (action_id) {
            case StatFlag.ActionStat.Quarantine:
            {
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, 1);
            }
            break;

            case StatFlag.ActionStat.Cure:
            {
                    ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, gameManager.turn_count);

                    Village village = villageManager.villages.Find(x => x.ID == village_id);
                    if (village != null)
                        village.Cure();
            }
            break;

            case StatFlag.ActionStat.Investigate:
            {
                    ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, 1);
            }
            break;

            case StatFlag.ActionStat.Lab:
            {
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, GetValue(combineID) + 1);
            }
            break;
        }

        currentAP -= pickAction.apCost;
    }

    public float GetValue(string action_id, string village_id = "") {

        if (ActionDict.TryGetValue(CombineID(action_id, village_id), out float p_value)) {
            return p_value;
        }

        return 0;
    }

    public void ProceedToNextState()
    {
        //Update AP
        currentAP = StatFlag.BaseModifier.baseAP;

    }

    private string CombineID(string action_id, string village_id) {
        return (string.IsNullOrEmpty(village_id)) ? action_id : village_id + "." + action_id;
    }

    public void Reset() {
        currentAP = StatFlag.BaseModifier.baseAP;
    }

    public struct ActionStat {
        public string ID;
        public int apCost;
    }
}
