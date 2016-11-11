using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ScoreSavedEvent: UnityEvent<string, int>
{
}

public class GazeKeyboard : MonoBehaviour, IVRPanel {

    public ScoreSavedEvent OnNameSaved;

    public string EnteredText {
        get
        {
            return nameBox.text;
        }
    }

    public int Score
    {
        get
        {
            return Int32.Parse(scoreBox.text);
        }
        set
        {
            scoreBox.text = value + "";
        }
    }
    Text nameBox;
    Text scoreBox;
    Button saveBtn;
    Button backspaceBtn;

    public int maxNameChars;
    public int minNameChars;
    int currNameChars;



	// Use this for initialization
	void Start () {
        nameBox = transform.Find("NamePanel").GetComponentInChildren<Text>();
        scoreBox = transform.Find("ScorePanel").GetComponentInChildren<Text>();
        saveBtn = transform.Find("SaveBtn").GetComponentInChildren<Button>();
        backspaceBtn = transform.Find("BackspaceBtn").GetComponentInChildren<Button>();


        currNameChars = 0;
        saveBtn.interactable = false;

        OnNameSaved = new ScoreSavedEvent();

        Transform _keys = transform.Find("Keys");

        foreach(Transform _key in _keys)
        {
            GazeKey gzKey = _key.gameObject.AddComponent<GazeKey>();
            Button keyBtn = _key.GetComponent<Button>();
            keyBtn.onClick.AddListener(() => {
                if( currNameChars <= maxNameChars)
                {
                    gzKey.AppendKeyCode(nameBox);
                    currNameChars++;
                }

                if(currNameChars >= minNameChars)
                {
                    saveBtn.interactable = true;
                }
                
            });
        }

        backspaceBtn.onClick.AddListener(() => {
            if(currNameChars > 0)
            {
                //Removing last two characters because of added tabs
                nameBox.text = nameBox.text.Remove(EnteredText.Length - 2, 2);
                currNameChars--;
                if (currNameChars < minNameChars)
                {
                    saveBtn.interactable = false;
                }
            }
        });

        saveBtn.onClick.AddListener(() =>{
            OnNameSaved.Invoke(EnteredText, Score);
        });
    }

    public void Show()
    {
        GetComponent<Image>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        GetComponent<Image>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
