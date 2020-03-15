using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;
using System.Linq;

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

    private int _turn_count = 1;
    public int turn_count => _turn_count;

    public TechModel techModel;
    public TechViewPresenter techViewPresenter;

    private ActionHandler actionHandler;

    public int investigationPower {
        get {
            int baseValue = 1;
            int universityCount = villageManager.villages.Count(x => x.facility == StatFlag.Facility.University && !x.isVillageDestroy);

            int universityPlus = villageManager.villages.Sum(x =>  (int)actionHandler.GetValue(StatFlag.ActionStat.Lab, x.ID) * ((x.isVillageDestroy) ? 0 : 1) );

            return baseValue + universityCount + universityPlus;
        }
    }

    private void Awake()
    {
        skillJsonBase = JsonUtility.FromJson<SkillJsonBase>(skillStatJson.text);

        villageInputCtrl = new VillageInputCtrl(Camera.main);
        diseaseManager = new DiseaseManager();
    }

    private void Start()
    {
        actionHandler = new ActionHandler(villageManager, this);

        RegisterBottomViewEvent();
        GameInit();
    }

    public void GameInit() {
        _turn_count = 1;
        villageManager.SetUp(villageInputCtrl, overallUIView, actionHandler);

        UpdateHeaderUIView();
        //villageManager.ProceedToNextState();
    }

    public void NextTurn() {
        _turn_count++;

        actionHandler.ProceedToNextState();

        bool isDieaseExplode = diseaseManager.IsExplodeDisease();
        Village newInfectVillage = diseaseManager.GetExplodeVillage(villageManager.villages);

        if (newInfectVillage != null && isDieaseExplode) {
            newInfectVillage.SetDisease();
        }

        if (villageManager != null)
            villageManager.ProceedToNextState();

        techViewPresenter?.OnNextTurn();

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

        statsInfo = string.Format(statsInfo, _turn_count);

        overallUIView.headerUIView.statText.text = statsInfo;

        string displayInfo = "Kingdom : <color=red>{1}</color> / <color=green>{0}</color>\nDead Population : {2}\nRegime Decline Rate : {3}%";

        displayInfo = string.Format(displayInfo, villageManager.wholePopulation, villageManager.wholeInfectPopulation, villageManager.wholedeadPopulation, 
                                    System.Math.Round((villageManager.wholedeadPopulation) / (float)villageManager.initialPopulation, 2) * 100);

        overallUIView.headerUIView.infoText.text = displayInfo;
    }

    private void RegisterBottomViewEvent() {
        overallUIView.baseButtonView.nextTurnBt.onClick.AddListener(() => NextTurn());
    }
}
