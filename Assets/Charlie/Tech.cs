using System.Collections.Generic;

public class Tech
{
    public int id;
    public bool isComplete;
    public string name;
    public int progress;
    public int baseCost;
    public int baseTurnToResearch;
    public List<int> dependancy;//tech ids
    //effects modifiers
    public float infectRate;
    public float deathRate;
    public float spreadRate;
    public float researchPowerMod;
    public float infectedDetectionRate;
    public int APMOD;
    public bool isMobileHospital;
    public bool isCure;

    public Tech(int id,string nm,int cost, List<int> dep)
    {
        this.id = id;
        name = nm;
        baseCost = cost;
        dependancy = dep;

        isComplete = false;
        progress = 0;
    }

    public void SetEffect(float ir,float dr, float sr, float rpm, float ifr, bool isHospital, bool isCure, int AP)
    {
        infectRate = ir;
        deathRate = dr;
        spreadRate = sr;
        researchPowerMod = rpm;
        infectedDetectionRate = ifr;
        isMobileHospital = isHospital;
        this.isCure = isCure;
        APMOD = AP;
    }
}
