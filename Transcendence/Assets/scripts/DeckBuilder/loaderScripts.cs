using UnityEngine;
using System.Collections;
using System;

public class loaderScripts : MonoBehaviour {

    // Use this for initialization

    public GameObject prefabCard;

    void Start() {

    }

    public void loadDeck()
    {
    
        string path = "path";
        ArrayList cards = new ArrayList();

        DeckReader reader = new DeckReader();
        cards = reader.load(path);

        GameObject IGcard;
        object DATAcard;

        int forNumber = cards.Count;
        int currentNumber = 0;

        while (forNumber <= currentNumber)
        {
            IGcard = (GameObject)Instantiate(prefabCard, transform.position, transform.rotation);
            DATAcard = cards[currentNumber];
            Convert.ChangeType(DATAcard, typeof(Card));

            //IGcard.transform. = false;

            currentNumber++;
         
        }
    }
}
