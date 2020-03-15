using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utility;

namespace JAM.Village
{
    public class Traveler : MonoBehaviour
    {

        public delegate void OnReachDestiny(Traveler traveler, Village destination);

        [SerializeField]
        private DiseaseSO possibleDisease;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Village destination;
        private Village originate;

        private int timecost;
        private int timeSpent;

        public int health_population;
        public int infect_population;
        public float infectRatio => infect_population / (float) health_population;

        private OnReachDestiny reachCallback;

        public void SetTraveler(int health_population, int infect_population, Sprite sprite, Village destination, Village originate, DiseaseSO disease, int timecost, OnReachDestiny reachCallback) {
            spriteRenderer.sprite = sprite;
            this.health_population = health_population;
            this.infect_population = infect_population;

            this.destination = destination;
            this.originate = originate;
            this.timecost = timecost;
            this.timeSpent = 0;

            possibleDisease = disease;

            this.reachCallback = reachCallback;

            transform.position = originate.transform.position;
        }

        public void ProceedToNextState() {
            CaculateInfectPerTurn();

            timeSpent++;

            //Make route look messy
            float distReachRdn = 1;
            if (timeSpent < timecost) 
                distReachRdn = Mathf.Clamp(1 + Random.Range(-0.2f, 0.2f), 0, 1.5f);

            Vector3 distVector = destination.transform.position - originate.transform.position;
            Vector3 partialDest = (timeSpent / (float)timecost) * distReachRdn * distVector ;

            float timeReachRdn = Mathf.Clamp(0.5f + Random.Range(-0.3f, 0.3f), 0, 1);

            if (timeSpent <= timecost) {
                transform.DOKill();
                transform.DOMove(originate.transform.position + partialDest, timeReachRdn).onComplete = CheckReactDestination;
            }
        }

        private void CaculateInfectPerTurn() {
            if (possibleDisease == null) return;

            //Calculate death
            int deathCount = Mathf.RoundToInt(infect_population * possibleDisease.GetRndDeathRate());
            this.infect_population -= deathCount;

            int newInfectPatient = InfectionMethod.CalculateInfectPeople(infectRatio, infect_population, possibleDisease.GetRndInfectRate());

            this.health_population -= newInfectPatient;
            this.infect_population += newInfectPatient;
        }

        private void CheckReactDestination() {
            if (timeSpent >= timecost)
            {
                reachCallback(this, destination);

                Utility.UtilityMethod.SafeDestroyGameObject<Transform>(this.transform);
            }
        }
    }
}