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

        [SerializeField]
        private SpriteRenderer villageHealthSprite;

        [SerializeField, Range(0, 0.1f)]
        private float travalerRate = 0.05f;

        public StatFlag.Facility facility;

        private int _healthpopulation;
        public int healthPopulation => _healthpopulation;

        private int _infectPopulation;
        public int infectPopulation => _infectPopulation;

        private int _deadPopulation;
        public int deadPopulation => _deadPopulation;

        public bool isVillageDestroy = false;

        public int totalPopulation => _infectPopulation + _healthpopulation;
        public float infectRate => _infectPopulation / (float)totalPopulation;
        public bool isDiseaseExist => diseases.Count > 0;

        private VillageManager _villageManager;
        private ActionHandler actionHandler;

        public void SetUp(VillageManager villageManager, ActionHandler actionHandler) {
            _villageManager = villageManager;
            this.actionHandler = actionHandler;
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
            if (isVillageDestroy) return;

            EffectFromInfect();
            MoveToConnectVillage();

            SetVillageHealthSprite();
            SetVillageStatus();
        }

        public void OnTravelerArrive(Traveler traveler) {
            if (traveler == null) return;
            _healthpopulation += traveler.health_population;
            _infectPopulation += traveler.infect_population;
        }

        public void Cure() {
            int cureCount = Mathf.RoundToInt( _infectPopulation * Random.Range(StatFlag.BaseModifier.baseCureRate - StatFlag.BaseModifier.baseErrorRate,
                                                            StatFlag.BaseModifier.baseCureRate + StatFlag.BaseModifier.baseErrorRate));
            
            _healthpopulation += cureCount;
            _infectPopulation -= cureCount;
        }

        private void EffectFromInfect() {
            //Calculate death
            int deathCount = Mathf.RoundToInt(_infectPopulation * defaultDiseases.GetRndDeathRate());
            _infectPopulation -= deathCount;
            _deadPopulation += deathCount;

            //Calculate new Infect
            float rdnInfectRate = defaultDiseases.GetRndInfectRate();

            if (actionHandler.GetValue(StatFlag.ActionStat.Quarantine, ID) > 0)
                rdnInfectRate = 0.001f;

            AffectPopulationWithInfectNum( InfectionMethod.CalculateInfectPeople(infectRate, _infectPopulation, rdnInfectRate));

            //Calculate cured
        }

        private void MoveToConnectVillage()
        {
            var connectedVillages = _villageManager.FindAllConnectVillage(ID);
            DiseaseSO randomDisease = null;

            //Apply Quarantine Skill
            if (actionHandler.GetValue(StatFlag.ActionStat.Quarantine, ID) > 0)
                return;

            if (diseases.Count > 0)
            {
                randomDisease = diseases[Random.Range(0, diseases.Count)];
            }

            foreach (var c_village in connectedVillages) {
                float travelerSpawnRate = 0.35f * ActionHandler.travelerRate;
                float rdnValue = Random.Range(0, 1f);

                bool spawnTraveler = rdnValue < travelerSpawnRate;
                if (!spawnTraveler || c_village.isVillageDestroy) continue;

                int totalLeavePopulation = Mathf.RoundToInt(totalPopulation * travalerRate);
                int infectLeavePopulation = Mathf.RoundToInt(totalLeavePopulation * infectRate);

                Debug.Log(ActionHandler.infectedDetectionRate);
                //Tech Effect
                if (ActionHandler.infectedDetectionRate > rdnValue && infectLeavePopulation > 0)
                    return;

                _villageManager.CreateTravler(this, c_village, totalLeavePopulation, infectLeavePopulation, randomDisease);

                _healthpopulation -= totalLeavePopulation - infectLeavePopulation;
                _infectPopulation -= infectLeavePopulation;
            }
        }

        private void AffectPopulationWithInfectNum(int newInfectNum) {
            _healthpopulation -= newInfectNum;
            _infectPopulation += newInfectNum;
        }

        private void SetVillageHealthSprite() {
            int spriteID = Mathf.FloorToInt( infectRate * 7);

            Sprite sprite = spritePacker.FindSpriteByName(StatFlag.Other.VillageHealthSprite + spriteID);
             
            if (spriteID >= 7)
                sprite = spritePacker.FindSpriteByName(StatFlag.Other.VillageHealthSpriteDead);

            if (sprite != null && villageHealthSprite != null)
                villageHealthSprite.sprite = sprite;
        }

        private void SetVillageStatus() {
            if (infectRate >= 1)
                isVillageDestroy = true;
        }

        public void ActDeathByTraveler(int travelerDeath) {
            _deadPopulation += travelerDeath;
        }

        public void Reset()
        {
            diseases.Clear();
            _healthpopulation = Random.Range(6000,10000);
            _infectPopulation = 0;
            _deadPopulation = 0;
            isVillageDestroy = false;

            SetVillageHealthSprite();
        }
    }
}
