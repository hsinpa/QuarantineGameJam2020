using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM.Village
{
    public class Traveler : MonoBehaviour
    {
        [SerializeField]
        private DiseaseSO possibleDisease;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Village destination;
        private Village originate;

        private int timecost;
        private int timeleft;

        private int population;

        public void SetTraveler(int population, Sprite sprite, Village destination, Village originate, DiseaseSO disease, int timecost) {
            spriteRenderer.sprite = sprite;
            this.population = population;
            this.destination = destination;
            this.originate = originate;
            this.timecost = timecost;
            this.timeleft = timecost;

            possibleDisease = disease;
        }

        public void ProceedToNextState() {

        }

    }
}