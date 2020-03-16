using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tech
{
    //UI
    public string imagePath;
    //stats
    public int id;
    public bool isComplete;
    public string techName;
    public string descriptionString;
    public int progress;
    public int baseCost;
    public int baseTurnToResearch;
    public List<int> dependancy;//tech ids
    //effects modifiers
    public  float infectRate;
    public  float deathRate;
    public float spreadRate;
    public  float researchPowerMod;
    public  float infectedDetectionRate;
    public  int APMOD;
    public  bool isMobileHospital;
    public  bool isCure;

    public Tech(int id,string nm,int cost, List<int> dep, string imgPath, string des)
    {
        this.id = id;
        techName = nm;
        baseCost = cost;
        dependancy = dep;
        imagePath = imgPath;
        descriptionString = des;

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
