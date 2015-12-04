using UnityEngine;
using System.Collections.Generic;

public class player : MonoBehaviour {

    public int playerSide = 0;
    public string deckPath = "/scripts/xml/cards.xml";
    public int baseHP = 30;
    public int currentHP;
    public bool isTurn = false;
    public bool hasStartedGoing = false;
    public int manaMax;
    public int currentMana;
    public List<Card> deck;
    public List<Card> shuffledDeck;
    public int currentDrawCard;

    // Use this for initialization
    void Start ()
    {
        currentMana = 0;
        currentHP = baseHP;
        currentDrawCard = 0;

        List<Card> deck = new List<Card>();

        deckPath = Application.dataPath + "/scripts/xml/cards.xml";

        DeckReader reader = new DeckReader();

        List<Card> archiveDeck = reader.load(deckPath);

        Debug.Log(archiveDeck.Count);

        for (int i = 0; i < archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);

            if (i == 26)
            {
                Debug.Log("End of the line");
                Debug.Log(deck[10].name);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

        /*
        deckPath = Application.dataPath + "/scripts/xml/cards.xml";

        DeckReader reader = new DeckReader();

        List<Card> archiveDeck = reader.load(deckPath);

        Debug.Log(archiveDeck.Count);

        for (int i = 0; i < archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);

            if (i == 26)
            {
                Debug.Log("End of the line");
            }
        }
        */

    }
}
