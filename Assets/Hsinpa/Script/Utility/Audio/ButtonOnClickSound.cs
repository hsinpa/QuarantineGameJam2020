using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClickSound : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SimpleAudioScript.instance.PlayAudio(StatFlag.Audio.ButtonSound);
        });
    }
}
