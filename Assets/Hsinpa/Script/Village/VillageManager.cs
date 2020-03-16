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

        public int wholePopulation => villages.Sum(x => x.totalPopulation);
        public int wholeHealthPopulation => villages.Sum(x => x.healthPopulation);
        public int wholeInfectPopulation => villages.Sum(x => x.infectPopulation);
        public int wholedeadPopulation => villages.Sum(x => x.deadPopulation);

        public int villageAliveCount => villages.Sum(x => x.isVillageDestroy ? 0 : 1);


        private int _initialPopulation;
        public int initialPopulation => _initialPopulation;

        private Village _onClickVillage = null;
        public Village onClickVillage => _onClickVillage;

        private ActionHandler actionHandler;

        private VillageInputCtrl villageInputCtrl;
        private OverallUIView overallUIView;

        public void SetUp(VillageInputCtrl villageInputCtrl, OverallUIView overallUIView, ActionHandler actionHandler) {

            this.overallUIView = overallUIView;
            this.villageInputCtrl = villageInputCtrl;
            this.actionHandler = actionHandler;

            _villages = transform.GetComponentsInChildren<Village>().ToList();
            _travelers = new List<Traveler>();
            _onClickVillage = null;
            int vLens = _villages.Count;
            for (int i = 0; i < vLens; i++)
                _villages[i].SetUp(this, actionHandler);

            if (this.villageInputCtrl.OnVillageObjectClick == null) {
                RegisterButtonEvent();
                this.villageInputCtrl.OnVillageObjectClick += OnVillageClick;
            }

            _initialPopulation = villages.Sum(x => x.totalPopulation);

            OnVillageClick(_onClickVillage);
            UpdateBottomUIView();
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

            OnVillageClick(_onClickVillage);
            UpdateBottomUIView();
        }

        public void CreateTravler(Village originate, Village desitination, int health_population, int infect_population, DiseaseSO carryDisease) {
            if (spritePackerSo == null) return;

            var travelObject = UtilityMethod.CreateObjectToParent(travelerHolders.transform, travelerPrefab.gameObject);
            Traveler traveler = travelObject.GetComponent<Traveler>();
            Sprite RandomSprite  = spritePackerSo.FindSpriteByRandom();

            int timeCost = Random.Range(2, 5);

            traveler.SetTraveler(health_population, infect_population, RandomSprite, desitination, originate, carryDisease, timeCost, OnTravelersReachDestination);

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

        private void OnVillageClick(Village village) {
            _onClickVillage = village;
            UpdateBottomUIView();

            overallUIView.villageInfoView.Show(village != null);
            if (village != null)
            {
                string villageName = village.VillageName;

                string info = "Population {0}\nInfect : {1}\nDead : {2}\nFacility : {3}";

                overallUIView.villageInfoView.SetTitle(villageName);
                overallUIView.villageInfoView.SetInfo(string.Format(info, village.totalPopulation, village.infectPopulation, village.deadPopulation, village.facility.ToString("g")));
            }
        }

        public void UpdateBottomUIView() {
            bool hasLabBt = (_onClickVillage != null && _onClickVillage.facility == StatFlag.Facility.University) && actionHandler.CheckActionValid(StatFlag.ActionStat.Lab, _onClickVillage.ID);
            overallUIView.baseButtonView.labBt.interactable = (hasLabBt);

            bool hasHospitalBt = ((_onClickVillage != null && _onClickVillage.facility == StatFlag.Facility.Hospital) 
                                    && actionHandler.CheckActionValid(StatFlag.ActionStat.Cure, _onClickVillage.ID) ) || ActionHandler.isMobileHospital;
            overallUIView.baseButtonView.hospitalBt.interactable = (hasHospitalBt);

            bool hasQuarantineBt = (_onClickVillage != null) && actionHandler.CheckActionValid(StatFlag.ActionStat.Quarantine, _onClickVillage.ID);
            overallUIView.baseButtonView.quarantineBt.interactable = (hasQuarantineBt);

            bool hasTechBt = actionHandler.CheckActionValid(StatFlag.ActionStat.Investigate);
            overallUIView.baseButtonView.techBt.interactable = (hasTechBt);
        }

        private void RegisterButtonEvent() {
            if (overallUIView.baseButtonView.buttonGroup != null) {
                foreach (var buttonTip in overallUIView.baseButtonView.buttonGroup) {

                    UnityEngine.UI.Button button = buttonTip.GetComponent<UnityEngine.UI.Button>();

                    button.onClick.AddListener(() =>
                    {
                        actionHandler.ExecuteAction(buttonTip.skill_id, (buttonTip.skill_id != StatFlag.ActionStat.Investigate) ?  onClickVillage.ID : "");
                        button.interactable = (false);

                        OnVillageClick(onClickVillage);
                    });
                }
            }
        }
    }
}