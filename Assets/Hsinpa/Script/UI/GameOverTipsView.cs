using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverTipsView : BaseUIView
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Button restartBt;

    private void Start()
    {
        Show(false);

        restartBt.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
    }

    public void SetTitle(string p_title) {
        Show(true);

        this.title.text = p_title;
    }
}
