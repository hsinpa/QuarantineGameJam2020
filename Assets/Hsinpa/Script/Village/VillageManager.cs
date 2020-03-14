using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utility;

namespace JAM.Village {
    public class VillageManager : MonoBehaviour
    {
        private List<Village> _villages;
        public List<Village> villages => _villages;
        
        private List<Traveler> _travelers;
        public List<Traveler> travelers => _travelers;

        [SerializeField]
        private ConnectVillageSO connectVillageSO;

        [SerializeField]
        private SpritePackerSO spritePackerSo;

        [SerializeField]
        private Traveler travelerPrefab;

        [SerializeField]
        private GameObject travelerHolders;


        public void SetUp() {
            _villages = transform.GetComponentsInChildren<Village>().ToList();
            _travelers = new List<Traveler>();

            int vLens = _villages.Count;
            for (int i = 0; i < vLens; i++)
                _villages[i].SetUp(this);
        }

        public void ProceedToNextState() {
            //Traveler
            int tLens = _travelers.Count;
            for (int i = 0; i < tLens; i++)
                _travelers[i].ProceedToNextState();

            //Village
            int vLens = _villages.Count;
            for (int i = 0; i < vLens; i++)
                _villages[i].ProceedToNextState();
        }

        public void CreateTravler(Village originate, Village desitination, int health_population, int infect_population, DiseaseSO carryDisease) {
            if (spritePackerSo == null) return;

            var travelObject = UtilityMethod.CreateObjectToParent(travelerHolders.transform, travelerPrefab.gameObject);
            Traveler traveler = travelObject.GetComponent<Traveler>();
            Sprite RandomSprite  = spritePackerSo.FindSpriteByRandom();

            traveler.SetTraveler(health_population, infect_population, RandomSprite, desitination, originate, carryDisease, 2, OnTravelersReachDestination);

            _travelers.Add(traveler);
        }

        public List<Village> FindAllConnectVillage(string fromVillageID) {
            var rawPairs = connectVillageSO.FindConnectionPairID(fromVillageID);
            return rawPairs.Select(x => FindVillageByID(x)).ToList();
        }

        private void OnTravelersReachDestination(Traveler traveler, Village desitination) {
            desitination.OnTravelerArrive(traveler);

            _travelers.Remove(traveler);
        }

        private Village FindVillageByID(string village_id) {
            return _villages.Find(x => x.ID == village_id);
        }

    }
}