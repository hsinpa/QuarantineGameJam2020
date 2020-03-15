using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechViewPresenter : MonoBehaviour
{
    public int GLOBAL_RESEARCHPOWER=5;
    //for current tech
    public GameObject CurrentTechRoot;
    public Image CurrentTechIcon;
    public Text CurrentTechName;
    public Image CurrentTechProgress;
    public GameObject NoResearchObject;
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
        Debug.LogError(_techModel.techTree.allTechs.Count);
        foreach (var t in _techModel.techTree.allTechs)
        {
            var go = Instantiate(TechIconPrefab, GridObject.transform, false);

            var ic = go.GetComponent<Image>();
            //ic.sprite = Resources.Load<Sprite>(t.imagePath);

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
            this._techModel.SetCurrentTech(tech);
            this.CurrentTechProgress.fillAmount = (float)tech.progress / (float)tech.baseCost;
            this.CurrentTechName.text = string.Format("{0} {1:P0}", tech.techName, ((float)tech.progress / (float)tech.baseCost));
        }
        //this.CurrentTechIcon.sprite = Resources.Load<Sprite>(tech.imagePath);
    }

    public void NextTurn()
    {
        _techModel.AdvanceCurrentTech(GLOBAL_RESEARCHPOWER);
        _techModel.CheckProgress();

        UpdateUI();
    }

    public void UpdateUI()
    {
        var tech = _techModel.currentTech;
        if (tech != null)
        {
            this.CurrentTechProgress.fillAmount = (float)tech.progress / (float)tech.baseCost;
            this.CurrentTechName.text = string.Format("{0} {1:P}", tech.techName, ((float)tech.progress / (float)tech.baseCost));
            //this.CurrentTechIcon.sprite = Resources.Load<Sprite>(tech.imagePath);
        }
        else
        {
            CurrentTechRoot.SetActive(false);
            NoResearchObject.SetActive(true);
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
        this._techModel = _gm.techModel;
        CurrentTechRoot.SetActive(false);
        NoResearchObject.SetActive(true);

        SetUpEvents();
        PopulateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
