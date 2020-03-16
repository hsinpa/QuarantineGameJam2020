using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TechModel : MonoBehaviour
{
    public GameManager _GameManager;
    public int DEFAULT_GLOBAL_RESEARCHPOWER = 5;
    public Tech currentTech;
    public TechTree techTree;
    //public UnityAction<Tech> OnTechComplete;
    public TechCompleteEvent OnTechCompleteEvent;

    public void Init()
    {
        currentTech = null;
        techTree = new TechTree();
        OnTechCompleteEvent = new TechCompleteEvent();
        //add techs

        Tech superMask = new Tech(1, "Super Mask", 30, null, "", "SuperMask Description placeholder");
        superMask.SetEffect(0.8f, 0, 0, 0, 0, false, false, 0);
        techTree.addTech(superMask);

        Tech superSickroom = new Tech(2, "Upgraded sickroom", 30, null, "", "superSickroom Description placeholder");
        superSickroom.SetEffect(0, 0.8f, 0, 0, 0, false, false, 0);
        techTree.addTech(superSickroom);

        Tech positioningChips = new Tech(3, "Positioning Chips", 30, null, "", "positioningChips Description placeholder");
        positioningChips.SetEffect(0, 0, -1, 0, 0, false, false, 0);
        techTree.addTech(positioningChips);

        Tech electronicAssistant = new Tech(4, "Electronic Assistant", 30, null, "", "electronicAssistant Description placeholder");
        electronicAssistant.SetEffect(0, 0, 0, 0.5f, 0, false, false, 0);
        techTree.addTech(electronicAssistant);

        Tech autoTestGate = new Tech(5, "Automatic Infection Detection Gate", 30, null, "", "autoTestGate Description placeholder");
        autoTestGate.SetEffect(0, 0, 0, 0, 0.8f, false, false, 0);
        techTree.addTech(autoTestGate);

        Tech mobileHospital = new Tech(6, "Mobile Hospital", 30, null, "", "mobileHospital Description placeholder");
        mobileHospital.SetEffect(0, 0, 0, 0, 0, true, false, 0);
        techTree.addTech(mobileHospital);

        Tech theCure = new Tech(7, "The cure", 100, null, "", "theCure Description placeholder");
        theCure.SetEffect(0, 0, -1, 0, 0, false, true, 0);
        techTree.addTech(theCure);
    }

    public void SetCurrentTech(Tech t)
    {
        currentTech = t;
    }

    public void AdvanceTech(Tech t)
    {
        t.progress += _GameManager.investigationPower;
    }

    public void AdvanceCurrentTech()
    {
        if (currentTech == null) return;//TODO:warning window?

        if (currentTech.progress < currentTech.baseCost)
            currentTech.progress += _GameManager.investigationPower;
    }

    public void CheckProgress()
    {
        if (currentTech == null) return;//TODO:warning window?

        if (currentTech.progress >= currentTech.baseCost)
        {
            currentTech.isComplete = true;

            OnTechCompleteEvent.Invoke(currentTech);

            currentTech = null;
        }
    }

    public void CalculateResearchPower()
    {

    }

    public void Awake()
    {
        Init();
    }
}
