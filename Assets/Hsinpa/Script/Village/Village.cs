using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

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

        public StatFlag.Facility facility;

        private int _healthpopulation;
        public int healthPopulation => _healthpopulation;

        private int _infectPopulation;
        public int infectPopulation => _infectPopulation;

        private int _deadPopulation;
        public int deadPopulation => _deadPopulation;


        public int totalPopulation => _infectPopulation + _healthpopulation;
        public float infectRate => _infectPopulation / (float)_healthpopulation;
        public bool isDiseaseExist => diseases.Count > 0;

        private VillageManager _villageManager;

        public void SetUp(VillageManager villageManager) {
            _villageManager = villageManager;
            diseases = new List<DiseaseSO>();

            Reset();
        }

        public void SetDisease() {
            if (defaultDiseases != null)
                diseases.Add(defaultDiseases);

            int infectPeople = Mathf.FloorToInt(_healthpopulation * defaultDiseases.GetRndExplosionRate());

            AffectPopulationWithInfectNum(infectPeople);
        }

        public void ProceedToNextState() {
            EffectFromInfect();
            MoveToConnectVillage();
        }

        public void OnTravelerArrive(Traveler traveler) {
            if (traveler == null) return;
            _healthpopulation += traveler.health_population;
            _infectPopulation += traveler.infect_population;
        }

        private void EffectFromInfect() {
            //Calculate death
            int deathCount = Mathf.RoundToInt(_infectPopulation * defaultDiseases.GetRndDeathRate());
            _infectPopulation -= deathCount;
            _deadPopulation += deathCount;

            //Calculate new Infect
            AffectPopulationWithInfectNum( InfectionMethod.CalculateInfectPeople(infectRate, _infectPopulation, defaultDiseases.GetRndInfectRate()));

            //Calculate cured
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
                float travelerSpawnRate = 0.35f;
                bool spawnTraveler = Random.Range(0, 1f) < travelerSpawnRate;
                if (!spawnTraveler) continue;

                int totalLeavePopulation = Mathf.RoundToInt(totalPopulation * travalerRate);
                int infectLeavePopulation = Mathf.RoundToInt(totalLeavePopulation * Mathf.Clamp(infectRate + Random.Range(-0.05f, 0), 0, 1 ));

                _villageManager.CreateTravler(this, c_village, totalLeavePopulation, infectLeavePopulation, randomDisease);

                _healthpopulation -= totalLeavePopulation - infectLeavePopulation;
                _infectPopulation -= infectLeavePopulation;
            }
        }

        private void AffectPopulationWithInfectNum(int newInfectNum) {
            _healthpopulation -= newInfectNum;
            _infectPopulation += newInfectNum;

        }

        public void Reset()
        {
            diseases.Clear();
            _healthpopulation = Random.Range(6000,10000);
            _infectPopulation = 0;
            _deadPopulation = 0;
        }
    }
}
