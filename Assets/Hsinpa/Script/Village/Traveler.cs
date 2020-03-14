using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        public int population;

        private OnReachDestiny reachCallback;

        public void SetTraveler(int population, Sprite sprite, Village destination, Village originate, DiseaseSO disease, int timecost, OnReachDestiny reachCallback) {
            spriteRenderer.sprite = sprite;
            this.population = population;
            this.destination = destination;
            this.originate = originate;
            this.timecost = timecost;
            this.timeSpent = 0;

            possibleDisease = disease;

            this.reachCallback = reachCallback;

            transform.position = originate.transform.position;
        }

        public void ProceedToNextState() {
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

        private void CheckReactDestination() {
            if (timeSpent >= timecost)
            {
                reachCallback(this, destination);

                Utility.UtilityMethod.SafeDestroyGameObject<Transform>(this.transform);
            }
        }
    }
}