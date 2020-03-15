using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HoverButtonTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Transform floatTipRect;

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
