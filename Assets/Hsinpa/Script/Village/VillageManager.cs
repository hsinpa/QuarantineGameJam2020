using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utility;

namespace JAM.Village {
    public class VillageManager : MonoBehaviour
    {
        private List<Village> villages;

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

            int vLens = villages.Count;
            for (int i = 0; i < vLens; i++)
                villages[i].SetUp(this);
        }

        public void ProceedToNextState() {
            int vLens = villages.Count;
            for (int i = 0; i < vLens; i++)
                villages[i].ProceedToNextState();
        }

        public void CreateTravler(Village originate, Village desitination, int population, DiseaseSO carryDisease) {
            var travelObject = UtilityMethod.CreateObjectToParent(travelerHolders.transform, travelerPrefab.gameObject);
            Traveler traveler = travelObject.GetComponent<Traveler>();
            Sprite RandomSprite  = spritePackerSo.FindSpriteByRandom();

            traveler.SetTraveler(population, RandomSprite, desitination, originate, carryDisease, 1);
        }

        public List<Village> FindAllConnectVillage(string fromVillageID) {
            var rawPairs = connectVillageSO.FindConnectionPairID(fromVillageID);
            return rawPairs.Select(x => FindVillageByID(x)).ToList();
        }

        private Village FindVillageByID(string village_id) {
            return villages.Find(x => x.ID == village_id);
        }
    }
}
