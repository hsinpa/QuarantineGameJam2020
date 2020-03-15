using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HoverButtonTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Transform floatTipRect;

    [SerializeField]
    Text floatTipText;

    [SerializeField]
    string skill_id;


    public void Start() {
        var skillStat = GameManager.skillJsonBase.FindSkillByName(skill_id);

        if (skillStat.name != null) {
            string skillDescription = "{0}\nCost :{1}\n\n{2}";

            floatTipText.text = string.Format(skillDescription, skillStat.name, skillStat.cost, skillStat.effect);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (floatTipRect != null)
            floatTipRect.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (floatTipRect != null)
            floatTipRect.gameObject.SetActive(false);
    }
}
