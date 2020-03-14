using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utility;

namespace JAM.Village {
    public class VillageManager : MonoBehaviour
    {
        private List<Village> villages;
        private List<Traveler> travelers;

        [SerializeField]
        private ConnectVillageSO connectVillageSO;

        [SerializeField]
        private SpritePackerSO spritePackerSo;

        [SerializeField]
        private Traveler travelerPrefab;

        [SerializeField]
        private GameObject travelerHolders;


        public void SetUp() {
            villages = transform.GetComponentsInChildren<Village>().ToList();
            travelers = new List<Traveler>();

            int vLens = villages.Count;
            for (int i = 0; i < vLens; i++)
                villages[i].SetUp(this);
        }

        public void ProceedToNextState() {
            //Traveler
            int tLens = travelers.Count;
            for (int i = 0; i < tLens; i++)
                travelers[i].ProceedToNextState();

            //Village
            int vLens = villages.Count;
            for (int i = 0; i < vLens; i++)
                villages[i].ProceedToNextState();
        }

        public void CreateTravler(Village originate, Village desitination, int health_population, int infect_population, DiseaseSO carryDisease) {
            if (spritePackerSo == null) return;

            var travelObject = UtilityMethod.CreateObjectToParent(travelerHolders.transform, travelerPrefab.gameObject);
            Traveler traveler = travelObject.GetComponent<Traveler>();
            Sprite RandomSprite  = spritePackerSo.FindSpriteByRandom();

            traveler.SetTraveler(health_population, infect_population, RandomSprite, desitination, originate, carryDisease, 2, OnTravelersReachDestination);

            travelers.Add(traveler);
        }

        public List<Village> FindAllConnectVillage(string fromVillageID) {
            var rawPairs = connectVillageSO.FindConnectionPairID(fromVillageID);
            return rawPairs.Select(x => FindVillageByID(x)).ToList();
        }

        private void OnTravelersReachDestination(Traveler traveler, Village desitination) {
            desitination.OnTravelerArrive(traveler);

            travelers.Remove(traveler);
        }

        private Village FindVillageByID(string village_id) {
            return villages.Find(x => x.ID == village_id);
        }

    }
}