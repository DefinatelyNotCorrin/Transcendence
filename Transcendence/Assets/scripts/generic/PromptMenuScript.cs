using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromptMenuScript : MonoBehaviour {
    public Canvas canvas;
    public string promptName;
    public Button button1;
    public Button button2;
    public Text description;

	// Use this for initialization
	void Start ()
    {
        moveToCenter();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    public void setName(string name)
    {
        this.name = name;
    }
    public void setDescription(string str)
    {
        description.text = str;
    }
    public void setLeftButtonText(string str)
    {
        button1.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    public void setRightButtonText(string str)
    {
        button2.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    public void isVisible(bool state)
    {
        canvas.enabled = state;
    }
    public void moveToCenter()
    {
        canvas.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }

}
