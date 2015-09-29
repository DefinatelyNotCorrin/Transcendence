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
        }
        else
        {
            print(("Error: File not Found"));
        }
	}

}
