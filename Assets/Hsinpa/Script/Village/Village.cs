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

        [SerializeField]
        private DiseaseSO defaultDiseases;

        [SerializeField, Range(0, 0.1f)]
        private float travalerRate = 0.05f;

        private int healthpopulation;
        private int infectPopulation;

        public int totalPopulation => infectPopulation + healthpopulation;
        public float infectRate => infectPopulation / (float)healthpopulation;
        public bool isDiseaseExist => diseases.Count > 0;

        private VillageManager _villageManager;

        public void SetUp(VillageManager villageManager) {
            _villageManager = villageManager;
            diseases = new List<DiseaseSO>();
        }

        public void SetDisease() {
            if (defaultDiseases != null)
                diseases.Add(defaultDiseases);
        }

        public void ProceedToNextState() {
            EffectFromInfect();
            MoveToConnectVillage();
        }

        public void OnTravelerArrive(Traveler traveler) {
            if (traveler == null) return;
            healthpopulation += traveler.health_population;
            infectPopulation += traveler.infect_population;
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

                int totalLeavePopulation = Mathf.RoundToInt(totalPopulation * travalerRate);
                int infectLeavePopulation = Mathf.RoundToInt(totalLeavePopulation * Mathf.Clamp(infectRate + Random.Range(-0.05f, 0), 0, 1 ));

                _villageManager.CreateTravler(this, c_village, totalLeavePopulation, infectLeavePopulation, randomDisease);

                healthpopulation -= totalLeavePopulation - infectLeavePopulation;
                infectPopulation -= infectLeavePopulation;
            }
        }
    }
}
