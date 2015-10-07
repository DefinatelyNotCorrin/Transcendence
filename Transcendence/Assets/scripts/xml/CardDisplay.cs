using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CardDisplay : MonoBehaviour {

    string path;

	// Use this for initialization
	void Start () {
        path = Application.dataPath + "/scripts/xml/cards.xml";
        DeckReader reader = new DeckReader();


        if (File.Exists(path))
        {
            List<Card> database = reader.load(path);

            foreach (Card card in database)
            {
                print((card.name));
            }

            Deck deck = new Deck();
            deck.activeDeck = new List<Card>();
            deck.archiveDeck = new List<Card>();
            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }
            Debug.Log(deck.peek().name);
            //deck.shuffle();
            deck.resetActive();
            List<Card> filtered = deck.filterCost("3");
            Debug.Log("Post Filter");
            foreach (Card c in filtered)
            {
                Debug.Log(c.name);
            }
        }
        else
        {
            Debug.Log("Error: File not Found");
        }
	}

}
