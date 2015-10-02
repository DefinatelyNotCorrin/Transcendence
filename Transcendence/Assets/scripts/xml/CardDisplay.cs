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
            deck.archiveDeck = new List<Card>();
            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }
            deck.resetActive();
            Debug.Log(deck.peek().name);
            //deck.shuffle();
            deck.filterName("World Guardian");
            Debug.Log("2" + deck.peek().name);
        }
        else
        {
            Debug.Log("Error: File not Found");
        }
	}

}
