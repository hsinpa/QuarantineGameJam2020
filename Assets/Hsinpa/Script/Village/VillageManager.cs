using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM.Village {
    public class VillageManager : MonoBehaviour
    {
        [SerializeField]
        private Village[] villages = new Village[0];
        public void SetUp() {
            villages = transform.GetComponentsInChildren<Village>();

            int vLens = villages.Length;
            for (int i = 0; i < vLens; i++)
                villages[i].SetUp(this);
        }

        public void ProceedToNextState() {
            int vLens = villages.Length;
            for (int i = 0; i < vLens; i++)
                villages[i].ProceedToNextState();
        }

        
    }
}
