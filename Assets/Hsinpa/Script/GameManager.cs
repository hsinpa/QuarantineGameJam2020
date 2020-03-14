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
    
    }

    public void NextTurn() {
        if (villageManager != null)
            villageManager.ProceedToNextState();
    }

}
