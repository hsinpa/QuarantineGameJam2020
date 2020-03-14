using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM.Village
{
    public class Village : MonoBehaviour
    {
        public string ID;

        [SerializeField]
        private SpritePackerSO spritePacker;

        [SerializeField]
        private List<DiseaseSO> diseases = new List<DiseaseSO>();

        [SerializeField, Range(0, 0.1f)]
        private float travalerRate = 0.05f;

        private int population;
        private float infectPopulation;

        private VillageManager _villageManager;

        public void SetUp(VillageManager villageManager) {
            _villageManager = villageManager;
        }

        public void ProceedToNextState() {
            EffectFromInfect();
            MoveToConnectVillage();
        }

        public void OnTravelerArrive(List<Traveler> travelers) {
            if (travelers == null) return;
            foreach (var t in travelers) { 
                
            }
        }

        private void EffectFromInfect() { 
        
        }

        private void MoveToConnectVillage()
        {
            DiseaseSO randomDisease = null;
            if (diseases.Count > 0) {
                randomDisease = diseases[Random.Range(0, diseases.Count)];
            }

            int travelerCount = Mathf.RoundToInt( population * travalerRate);

            population -= travelerCount;
        }
    }
}
