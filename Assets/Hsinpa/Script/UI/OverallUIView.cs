using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallUIView : MonoBehaviour
{
    [SerializeField]
    private BottomButtonView _baseButtonView;
    public BottomButtonView baseButtonView => _baseButtonView;

    [SerializeField]
    private HeaderUIView _headerUIView;
    public HeaderUIView headerUIView => _headerUIView;

    [SerializeField]
    private VillageInfoView _villageInfoView;
    public VillageInfoView villageInfoView => _villageInfoView;
}
