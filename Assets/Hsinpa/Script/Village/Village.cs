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

        public void OnTravelerArrive(Traveler traveler) {
            if (traveler == null) return;
            population += traveler.population;
        }

        private void EffectFromInfect() { 
        
        }

        private void MoveToConnectVillage()
        {
            var connectedVillages = _villageManager.FindAllConnectVillage(ID);
            DiseaseSO randomDisease = null;

            if (diseases.Count > 0)
            {
                randomDisease = diseases[Random.Range(0, diseases.Count)];
            }

            foreach (var c_village in connectedVillages) {
                int travelerCount = Mathf.RoundToInt(population * travalerRate);

                _villageManager.CreateTravler(this, c_village, travelerCount, randomDisease);

                population -= travelerCount;
            }
        }
    }
}
