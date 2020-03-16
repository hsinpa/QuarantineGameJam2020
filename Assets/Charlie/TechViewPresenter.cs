using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechViewPresenter : MonoBehaviour
{
    //for current tech
    public GameObject CurrentTechRoot;
    public Image CurrentTechIcon;
    public Text CurrentTechName;
    public Text CurrentTechDescription;
    public Image CurrentTechProgress;
    public GameObject NoResearchObject;

    public SpritePackerSO spritePacker;

    //Grid
    public GameObject GridObject;
    
    public GameObject TechIconPrefab;
    //ButtonBars
    public Button CloseButton;

    private TechModel _techModel;
    private GameManager _gm;
    private List<Button> techButtons;

    private GameObject popupObject;

    void SetUpEvents()
    {
        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(CloseView);
    }

    void PopulateGrid()
    {
        foreach (var t in _techModel.techTree.allTechs)
        {
            string spriteID = "Icon_Tech_0" + t.id;
            var go = Instantiate(TechIconPrefab, GridObject.transform, false);

            var ic = go.GetComponent<Image>();
            ic.sprite = spritePacker.FindSpriteByName(spriteID);
            

            var button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>OnTechClick(t.id));

            if (!_techModel.techTree.isTechAvailable(t))
                go.transform.Find("UnavailableMark").gameObject.SetActive(true);
            else
                go.transform.Find("UnavailableMark").gameObject.SetActive(false);

            techButtons.Add(button);
        }
    }
    
    void CloseView()
    {
        this.gameObject.SetActive(false);
    }

    void OnTechClick(int id)
    {
        var tech = _techModel.techTree.getById(id);

        if (_techModel.techTree.isTechAvailable(tech))
        {
            CurrentTechRoot.SetActive(true);
            NoResearchObject.SetActive(false);
            _techModel.SetCurrentTech(tech);
            CurrentTechProgress.fillAmount = (float)tech.progress / (float)tech.baseCost;
            CurrentTechName.text = string.Format("{0} {1:P0}", tech.techName, ((float)tech.progress / (float)tech.baseCost));
            CurrentTechDescription.text = tech.descriptionString;
        }

        string spriteID = "Banner_Tech_0" + id;
        this.CurrentTechIcon.sprite = spritePacker.FindSpriteByName(spriteID);
    }

    public void OnNextTurn()
    {
        _techModel.AdvanceCurrentTech();
        _techModel.CheckProgress();
        UpdateUI();
    }

    public void UpdateUI()
    {
        var tech = _techModel.currentTech;
        if (tech != null)
        {
            CurrentTechProgress.fillAmount = (float)tech.progress / (float)tech.baseCost;
            CurrentTechName.text = string.Format("{0} {1:P}", tech.techName, ((float)tech.progress / (float)tech.baseCost));
            //this.CurrentTechIcon.sprite = Resources.Load<Sprite>(tech.imagePath);
            CurrentTechDescription.text = tech.descriptionString;
        }
        else
        {
            CurrentTechRoot.SetActive(false);
            NoResearchObject.SetActive(true);
            CurrentTechDescription.text = "No active research underway";
        }

        for (int i = 0; i < _techModel.techTree.allTechs.Count; i++)
        {
            var t = _techModel.techTree.allTechs[i];
            if (t.isComplete)
                techButtons[i].transform.Find("CompleteMark").gameObject.SetActive(true);
            else
                techButtons[i].transform.Find("CompleteMark").gameObject.SetActive(false);

            if (!_techModel.techTree.isTechAvailable(t))
                techButtons[i].transform.Find("UnavailableMark").gameObject.SetActive(true);
            else
                techButtons[i].transform.Find("UnavailableMark").gameObject.SetActive(false);
        }
    }

    void OnTechHover()
    {
        //??
    }

    // Start is called before the first frame update
    void Start()
    {
        techButtons = new List<Button>();
        _gm = FindObjectOfType<GameManager>();
        _gm.techViewPresenter = this;
        _techModel = _gm.techModel;
        CurrentTechRoot.SetActive(false);
        NoResearchObject.SetActive(true);

        SetUpEvents();
        PopulateGrid();

        _techModel.OnTechCompleteEvent.AddListener (x => Debug.LogError("Research Complete="+x.techName));
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
