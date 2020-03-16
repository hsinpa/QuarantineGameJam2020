using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {
    public class InfectionMethod
    {

        public static int CalculateInfectPeople(float infectPercentage, int infectPopulation, float infectRate) {
            int possibleInfectPerPerson = Random.Range(StatFlag.BaseModifier.baseInfectNumPerPerson - 2, StatFlag.BaseModifier.baseInfectNumPerPerson + 2);

            possibleInfectPerPerson = Mathf.Clamp((int)ActionHandler.spreadRate + possibleInfectPerPerson, 0, possibleInfectPerPerson);

            return Mathf.RoundToInt((1 - infectPercentage) * possibleInfectPerPerson * infectPopulation * infectRate);
        }

    }
}
