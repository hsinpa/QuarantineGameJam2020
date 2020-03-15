using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIView : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup canvasGroup;

    public virtual void Show(bool isShow) {
        canvasGroup.alpha = isShow ? 1 : 0;
        canvasGroup.blocksRaycasts = isShow;
        canvasGroup.interactable = isShow;
    }

}
