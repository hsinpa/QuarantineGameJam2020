using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private VillageManager villageManager;

    [SerializeField]
    private OverallUIView overallUIView;

    [SerializeField]
    public TextAsset skillStatJson;

    private DiseaseManager diseaseManager;

    private VillageInputCtrl villageInputCtrl;

    public static SkillJsonBase skillJsonBase;
    private int turn_count = 1;

    private void Awake()
    {

        skillJsonBase = JsonUtility.FromJson<SkillJsonBase>(skillStatJson.text);

        villageInputCtrl = new VillageInputCtrl(Camera.main);
        diseaseManager = new DiseaseManager();
    }

    private void Start()
    {
        RegisterBottomViewEvent();
        GameInit();
    }

    public void GameInit() {
        turn_count = 1;
        villageManager.SetUp(villageInputCtrl, overallUIView);

        UpdateHeaderUIView();
        //villageManager.ProceedToNextState();
    }

    public void NextTurn() {
        turn_count++;

        bool isDieaseExplode = diseaseManager.IsExplodeDisease();
        Village newInfectVillage = diseaseManager.GetExplodeVillage(villageManager.villages);

        if (newInfectVillage != null && isDieaseExplode) {
            newInfectVillage.SetDisease();
        }

        if (villageManager != null)
            villageManager.ProceedToNextState();

        UpdateHeaderUIView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }

        villageInputCtrl.OnUpdate();
    }


    private void UpdateHeaderUIView()
    {
        string statsInfo = "Turn {0}";

        statsInfo = string.Format(statsInfo, turn_count);

        overallUIView.headerUIView.statText.text = statsInfo;

        string displayInfo = "Kingdom : <color=red>{1}</color> / <color=green>{0}</color>\nDead Population : {2}\nRegime Decline Rate : {3}%";

        displayInfo = string.Format(displayInfo, villageManager.wholePopulation, villageManager.wholeInfectPopulation, villageManager.wholedeadPopulation, 
                                    System.Math.Round(villageManager.wholedeadPopulation / (float)villageManager.initialPopulation, 2) * 100);

        overallUIView.headerUIView.infoText.text = displayInfo;
    }

    private void RegisterBottomViewEvent() {
        overallUIView.baseButtonView.nextTurnBt.onClick.AddListener(() => NextTurn());
    }
}
