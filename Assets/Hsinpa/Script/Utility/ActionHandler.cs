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
    private TechModel techModel;

    public static float infectRate;
    public static float deathRate;
    public static float spreadRate;
    public static float travelerRate;
    public static float ResearchPowerOffset;
    public static float infectedDetectionRate;
    public static int APMOD;
    public static bool isMobileHospital;
    public static bool isCure;

    public ActionHandler(VillageManager villageManager, GameManager gameManager, TechModel techModel) {
        this.techModel = techModel;
        ActionDict = new Dictionary<string, float>();
        this.gameManager = gameManager;
        this.villageManager = villageManager;
        actionStatList = new List<ActionStat>();
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Quarantine, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Lab, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Cure, apCost = 1 });
        actionStatList.Add(new ActionStat { ID = StatFlag.ActionStat.Investigate, apCost = 0 });

        Reset();
    }

    public bool CheckActionValid(string action_id, string village_id = "") {
        string combineID = CombineID(action_id, village_id);
        ActionStat pickAction = actionStatList.Find(x => x.ID == action_id);
        if (pickAction.ID == null) return false;

        if (action_id == StatFlag.ActionStat.Cure)
            return gameManager.turn_count - GetValue(action_id, village_id) > 1 && currentAP >= pickAction.apCost;

        if (action_id == StatFlag.ActionStat.Quarantine)
            return GetValue(action_id, village_id) <= 0 && currentAP >= pickAction.apCost;

        if (action_id == StatFlag.ActionStat.Investigate)
            return currentAP >= 2;

        return currentAP >= pickAction.apCost;
    }

    public void ExecuteAction(string action_id, string village_id = null) {
        ActionStat pickAction = actionStatList.Find(x => x.ID == action_id);
        if (pickAction.ID == null) return;

        string combineID = CombineID(action_id, village_id);
        switch (action_id) {
            case StatFlag.ActionStat.Quarantine:
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, 1);
                break;

            case StatFlag.ActionStat.Cure:
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, gameManager.turn_count);

                Village village = villageManager.villages.Find(x => x.ID == village_id);
                if (village != null)
                    village.Cure();
                break;

            case StatFlag.ActionStat.Investigate:
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, 1);
                gameManager.techViewPresenter.gameObject.SetActive(true);
                break;

            case StatFlag.ActionStat.Lab:
                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, combineID, GetValue(combineID) + 1);
            break;
        }

        currentAP -= pickAction.apCost;
        gameManager.UpdateHeaderUIView();
    }

    public float GetValue(string action_id, string village_id = "") {

        if (ActionDict.TryGetValue(CombineID(action_id, village_id), out float p_value)) {
            return p_value;
        }

        return 0;
    }

    public void ProceedToNextState()
    {
        Reset();
        UpdateRateModifier();

        //Update AP
        currentAP = StatFlag.BaseModifier.baseAP + APMOD;
    }


    private void UpdateRateModifier() {
        if (this.techModel.techTree != null && this.techModel.techTree.allTechs != null) {
            foreach (Tech tech in this.techModel.techTree.allTechs) {

                if (!tech.isComplete) continue;

                infectRate += tech.infectRate;
                deathRate += tech.deathRate;
                spreadRate += tech.spreadRate;
                if(!Mathf.Approximately(tech.travelerRateModifier, 0f))
                travelerRate = tech.travelerRateModifier;
                infectedDetectionRate += tech.infectedDetectionRate;
                APMOD += tech.APMOD;
                //Debug.LogError(tech.researchPowerMod);
                if (tech.isMobileHospital)
                    isMobileHospital = true;
            }
        }
    }

    private string CombineID(string action_id, string village_id) {
        return (string.IsNullOrEmpty(village_id)) ? action_id : village_id + "." + action_id;
    }

    public void Reset() {
        currentAP = StatFlag.BaseModifier.baseAP;

        infectRate = 1;
        deathRate = 1;
        spreadRate = 0;
        travelerRate = 1;
        ResearchPowerOffset = 0;
        infectedDetectionRate = 0;
        APMOD = 0;
        isMobileHospital = false;

        if (villageManager.villages != null) {
            foreach (var village in villageManager.villages)
            {
                string quarantineID = CombineID(StatFlag.ActionStat.Quarantine, village.ID);

                ActionDict = UtilityMethod.EditDictionary<float>(ActionDict, quarantineID, 0);
            }
        }

    }

    public struct ActionStat {
        public string ID;
        public int apCost;
    }
}
