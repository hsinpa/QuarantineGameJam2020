using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private VillageManager villageManager;

    private DiseaseManager diseaseManager;

    private int turn_count = 1;

    private void Awake()
    {
        diseaseManager = new DiseaseManager();
    }

    private void Start()
    {
        GameInit();
    }

    public void GameInit() {
        turn_count = 1;
        villageManager.SetUp();
        //villageManager.ProceedToNextState();
    }

    public void NextTurn() {
        turn_count++;

        bool isDieaseExplode = diseaseManager.IsExplodeDisease();
        Village newInfectVillage = diseaseManager.GetExplodeVillage(villageManager.villages);

        if (newInfectVillage != null && isDieaseExplode) { 
            
        }

        if (villageManager != null)
            villageManager.ProceedToNextState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }
    }

}
