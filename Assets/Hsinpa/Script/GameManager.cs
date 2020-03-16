using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private VillageManager villageManager;
    public VillageManager VillageManager { get { return villageManager; } }

    [SerializeField]
    private OverallUIView overallUIView;

    [SerializeField]
    public TextAsset skillStatJson;

    [SerializeField]
    private GameOverTipsView gameOverTipsView;

    private DiseaseManager diseaseManager;

    private VillageInputCtrl villageInputCtrl;

    public static SkillJsonBase skillJsonBase;

    private int _turn_count = 1;
    public int turn_count => _turn_count;

    public TechModel techModel;
    public TechViewPresenter techViewPresenter;
    public AudioManager audioManager;
    public RectTransform APDisplay;
    public GameObject APSpriteObject;

    private ActionHandler actionHandler;
    public ActionHandler ActionHandler { get { return this.actionHandler; } }

    public int investigationPower {
        get {
            int baseValue = 1;
            int universityCount = villageManager.villages.Count(x => x.facility == StatFlag.Facility.University && !x.isVillageDestroy);

            int universityPlus = villageManager.villages.Sum(x =>  (int)actionHandler.GetValue(StatFlag.ActionStat.Lab, x.ID) * ((x.isVillageDestroy) ? 0 : 1) );

            return baseValue + universityCount + universityPlus + (int)ActionHandler.ResearchPowerOffset;
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
        actionHandler = new ActionHandler(villageManager, this, techModel);

        techModel.OnTechCompleteEvent.AddListener(OnTechComplete);

        RegisterBottomViewEvent();
        GameInit();
    }

    public void GameInit() {
        _turn_count = 1;
        villageManager.SetUp(villageInputCtrl, overallUIView, actionHandler);

        UpdateHeaderUIView();
        //villageManager.ProceedToNextState();
        techModel._GameManager = this;
        audioManager = FindObjectOfType<AudioManager>();

        //techViewPresenter.gameObject.SetActive(false);
    }

    public void NextTurn() {
        _turn_count++;
        techViewPresenter?.OnNextTurn();
        actionHandler.ProceedToNextState();

        bool isDieaseExplode = diseaseManager.IsExplodeDisease();
        Village newInfectVillage = diseaseManager.GetExplodeVillage(villageManager.villages);

        if (newInfectVillage != null && isDieaseExplode) {
            newInfectVillage.SetDisease();
        }

        if (villageManager != null)
            villageManager.ProceedToNextState();

        UpdateHeaderUIView();

        if (villageManager.villageAliveCount <= 0)
            gameOverTipsView.SetTitle("Kingdom Fall");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }

        villageInputCtrl.OnUpdate();
    }


    public void UpdateHeaderUIView()
    {
        //string statsInfo = "Turn {0}\nAP : {1}";

        //statsInfo = string.Format(statsInfo, _turn_count, actionHandler.currentAP);

        string statsInfo = "Turn {0}\nAP :";

        statsInfo = string.Format(statsInfo, _turn_count);
        
        for (int i = 0; i < APDisplay.transform.childCount; i++)
            Destroy(APDisplay.transform.GetChild(i).gameObject);
        
        for (int i = 0; i < actionHandler.currentAP; i++)
            Instantiate(APSpriteObject, APDisplay, false);

        overallUIView.headerUIView.statText.text = statsInfo;

        string displayInfo = "Kingdom : <color=red>{1}</color> / <color=green>{0}</color>\nDead Population : {2}\nRegime Decline Rate : {3}%\nResearch Power : {4}";

        displayInfo = string.Format(displayInfo, villageManager.wholePopulation, villageManager.wholeInfectPopulation, villageManager.wholedeadPopulation,
                                    System.Math.Round((villageManager.wholedeadPopulation) / (float)villageManager.initialPopulation, 2) * 100, investigationPower);
        if (techModel.currentTech != null)
        {
            displayInfo = displayInfo + "\nOngoing Research:" + techModel.currentTech.techName;
        }
        else
        {
            displayInfo = displayInfo + "\nOngoing Research:" + "None";
        }

        overallUIView.headerUIView.infoText.text = displayInfo;
    }

    private void RegisterBottomViewEvent() {
        overallUIView.baseButtonView.nextTurnBt.onClick.AddListener(() => NextTurn());
    }

    private void OnTechComplete(Tech tech) {
        SimpleAudioScript.instance.PlayAudio(StatFlag.Audio.Tech_Done);

        if (tech.isCure)
            gameOverTipsView.SetTitle("Game Completed!");

        if (tech.isComplete && !tech.isCure) {
            techViewPresenter.gameObject.SetActive(true);
        }

    }
}
