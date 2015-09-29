using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour
{

    //should create one single card from a list of cards, if ID and PATH for the XML file are known
    public void generateCard(int cardID, string path)
    {
        //a list that will hold all the cards
        List<Card> cards = new List<Card>();

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();
        cards = reader.load(path);

        //the card that will be put on the playing field
        Card card;

        //sets the in game card's values equal to the values it should have (gotten from the list<>)
        card = cards[cardID];

        //generates the card using the prefabCard prefab
        ///card = (GameObject)Instantiate(CardPrefab, transform.position, transform.rotation);

    }

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(string path)
    {

        //all of the cards in a list<>
        List<Card> cards = new List<Card>(); 

        //loads the cards from the database or deck
        DeckReader reader = new DeckReader();
        cards = reader.load(path);

        //the card that will be put on the playing field
        Card card;

        //information about the size of the array of cards, and what card the while loop is on
        int totalCards = cards.Count;
        int currentCard = 0;

        //goes through the cards one bye one and adds them to the playing field
        while (totalCards <= currentCard)
        {
            //sets the in game card's values equal to the values gotten from the list<>
            card = cards[currentCard];

            //generates the card using the card prefab
            //card = (GameObject)Instantiate(PrefabCard, transform.position, transform.rotation);
           
            currentCard++;
        }
    }
}
