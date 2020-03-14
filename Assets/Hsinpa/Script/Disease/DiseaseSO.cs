using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DiseaseSO : ScriptableObject
{
    public string _id;
    public string _name;
    public string _description;

    public float infectRate;
    public float deathRate;
    public float panicRate;

    public float errorRange;

    public float GetRndInfectRate() {
        float rdnRate = Random.Range(infectRate - errorRange, infectRate + errorRange);
        return Mathf.Clamp(rdnRate, 0, rdnRate);
    }

    public float GetRndDeathRate()
    {
        float rdnRate = Random.Range(deathRate - errorRange, deathRate + errorRange);
        return Mathf.Clamp(rdnRate, 0, rdnRate);
    }

    public float GetRndPanicRate()
    {
        float rdnRate = Random.Range(panicRate - errorRange, panicRate + errorRange);
        return Mathf.Clamp(rdnRate, 0, rdnRate);
    }
}
