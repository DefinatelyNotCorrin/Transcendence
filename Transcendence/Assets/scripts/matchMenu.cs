using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class matchMenu : MonoBehaviour {


    public Dropdown player1DeckSelector;
    public Dropdown player2DeckSelecor;
    public Button loadPlayer1Deck;
    public Button loadPlayer2Deck;
    public Dropdown.OptionData data = new Dropdown.OptionData();
    public List<string> deckList;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        data.text = "nerd";
        int x = 0;
        while (x < 1) ;
        {
            data.text = "nerdDeck";
            loadPlayer1Deck.GetComponent<Dropdown>().options.Add(data);
            loadPlayer2Deck.GetComponent<Dropdown>().options.Add(data);
            x++;

        }
	
	}

    
}
