using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageInfoView : BaseUIView
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text infoText;


    public void SetTitle(string title) {
        titleText.text = title;
    }

    public void SetInfo(string infoString)
    {
        infoText.text = infoString;
    }

}
