using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GazeMainMenu : MonoBehaviour, IVRPanel {
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
