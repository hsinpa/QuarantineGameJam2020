using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JAM.Village;
using System.Linq;

public class DiseaseManager
{
    float explode_rate;
    int currentExplosion;
    int maxExplosion = 3;

    public DiseaseManager() {
        Reset();
    }

    public bool IsExplodeDisease() {

        float randomValue = Random.Range(0f, 1f);
        bool isExplode = (randomValue > explode_rate) && (currentExplosion <= maxExplosion);

        SetExplodeStat(isExplode);

        if (isExplode)
            currentExplosion++;

        return isExplode;
    }

    public Village GetExplodeVillage(List<Village> villages) {
        var filteredVillage = villages.FindAll(x => !x.isDiseaseExist);

        if (filteredVillage.Count <= 0) return null;

        return filteredVillage[Random.Range(0, filteredVillage.Count)];
    }

    private void SetExplodeStat(bool isExplode) {
        if (isExplode)
            explode_rate = 1;
        else
            //Decay
            explode_rate -= 0.05f;
    }

    public void Reset() {
        currentExplosion = 0;
        explode_rate = 0.6f;
    }
}
