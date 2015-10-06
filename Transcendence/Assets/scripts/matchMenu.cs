using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class matchMenu : MonoBehaviour {


    public Dropdown player1DeckSelector;
    public Dropdown player2DeckSelecor;
    public Button loadPlayer1Deck;
    public Button loadPlayer2Deck;
    public Dropdown.OptionData data = new Dropdown.OptionData();

	// Use this for initialization
	void Start () {
	
        
	}
	
	// Update is called once per frame
	void Update () {
        data.text = "nerd";
        player1DeckSelector.GetComponent<Dropdown>().options.Add(data);
	
	}
}
