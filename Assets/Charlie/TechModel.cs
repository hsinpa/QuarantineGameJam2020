using System.Collections.Generic;
using UnityEngine;

public class TechModel : MonoBehaviour
{
    public Tech currentTech;
    public TechTree techTree;

    public void Init()
    {
        currentTech = null;
        techTree = new TechTree();
        //add techs
        
        Tech superMask = new Tech(1, "Super Mask", 30, null, "");
        superMask.SetEffect(0.8f, 0, 0, 0, 0, false, false, 0);
        techTree.addTech(superMask);

        Tech superSickroom = new Tech(2, "Upgraded sickroom", 30, null, "");
        superSickroom.SetEffect(0, 0.8f, 0, 0, 0, false, false, 0);
        techTree.addTech(superSickroom);

        Tech positioningChips = new Tech(3, "Positioning Chips", 30, null, "");
        positioningChips.SetEffect(0, 0, -1, 0, 0, false, false, 0);
        techTree.addTech(positioningChips);

        Tech electronicAssistant = new Tech(4, "Electronic Assistant", 30, null, "");
        electronicAssistant.SetEffect(0, 0, 0, 2, 0, false, false, 0);
        techTree.addTech(electronicAssistant);

        Tech autoTestGate = new Tech(5, "Automatic Infection Detection Gate", 30, null, "");
        autoTestGate.SetEffect(0, 0, 0, 0, 0.8f, false, false, 0);
        techTree.addTech(autoTestGate);

        Tech mobileHospital = new Tech(6, "Mobile Hospital", 30, null, "");
        mobileHospital.SetEffect(0, 0, 0, 0, 0, true, false, 0);
        techTree.addTech(mobileHospital);

        Tech theCure = new Tech(7, "The cure", 100, null, "");
        theCure.SetEffect(0, 0, -1, 0, 0, false, true, 0);
        techTree.addTech(theCure);

        Debug.LogError(techTree.allTechs.Count);
    }

    public void SetCurrentTech(Tech t)
    {
        currentTech = t;
    }

    public void AdvanceTech(Tech t, int p)
    {
        t.progress += p;
    }

    public void AdvanceCurrentTech(int p)
    {
        currentTech.progress = +p;
    }

    public void CheckProgress()
    {
        if (currentTech.progress > currentTech.baseCost)
            currentTech.isComplete = true;

        currentTech = null;
    }

    public void CalculateResearchPower()
    {

    }

    public void Awake()
    {
        Init();
    }
}
