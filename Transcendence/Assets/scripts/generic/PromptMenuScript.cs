using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromptMenuScript : MonoBehaviour {
    public string promptName;
    public Button button1;
    public Button button2;
    public Text description;

	// Use this for initialization
	void Start ()
    {
        setLeftButtonText("test1");
        setRightButtonText("test2");
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void setLeftButtonText(string str)
    {
        button1.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    void setRightButtonText(string str)
    {
        button2.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    void isVisible(bool state)
    {
        this.enabled = state;
    }


}
