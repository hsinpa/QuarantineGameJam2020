using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private VillageManager villageManager;

    private void Awake()
    {
        
    }

    private void Start()
    {
        GameInit();
    }

    public void GameInit() {
        villageManager.SetUp();
        //villageManager.ProceedToNextState();
    }

    public void NextTurn() {
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
