using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GazeKey : MonoBehaviour {

	// Use this for initialization
	public void AppendKeyCode(Text nameBox)
    {
        nameBox.text += gameObject.name + "\t";
    }
}
