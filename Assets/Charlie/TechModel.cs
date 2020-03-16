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

        Tech superMask = new Tech(1, "Super Mask", 30, null, "", "Decrease Infect Rate by 20%");
        superMask.SetEffect(0.8f, 0, 0, 0, 0, false, false, 0);
        techTree.addTech(superMask);

        Tech superSickroom = new Tech(2, "Upgraded sickroom", 30, null, "", "Decrease Death Rate by 20%");
        superSickroom.SetEffect(0, 0.8f, 0, 0, 0, false, false, 0);
        techTree.addTech(superSickroom);

        Tech positioningChips = new Tech(3, "E-Assistant", 30, null, "", "Reduce basic reproduction number by 1 ");
        positioningChips.SetEffect(0, 0, -1, 0, 0, false, false, 0);
        techTree.addTech(positioningChips);

        Tech electronicAssistant = new Tech(4, "Video Call", 30, null, "", "Reduce travelers by 50%");
        electronicAssistant.SetEffect(0, 0, 0, 0.5f, 0, false, false, 0);
        techTree.addTech(electronicAssistant);

        Tech performanceImprove = new Tech(5, "Performance Improve", 30, null, "", "AP +1 per turn");
        performanceImprove.SetEffect(0, 0, -1, 0, 0, false, false, 1);
        techTree.addTech(performanceImprove);

        Tech autoTestGate = new Tech(6, "Detection Gate", 30, null, "", "80% to detect infected traveler");
        autoTestGate.SetEffect(0, 0, 0, 0, 0.8f, false, false, 0);
        techTree.addTech(autoTestGate);

        Tech mobileHospital = new Tech(7, "Mobile Hospital", 30, null, "", "Install mobile hospital to all village");
        mobileHospital.SetEffect(0, 0, 0, 0, 0, true, false, 0);
        techTree.addTech(mobileHospital);

        Tech theCure = new Tech(8, "The cure", 100, null, "", "Research to win");
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
        currentTech.progress += _GameManager.investigationPower;
        if (currentTech.progress > currentTech.baseCost)
        {
            currentTech.progress = currentTech.baseCost;
            CheckProgress();
        }
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
