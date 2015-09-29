using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour
{

    public GameObject PrefabCard;

    //should create one single card from a list of cards, if ID and PATH for the XML file are known
    public void generateCard(int cardLocation, string path)
    {
        //a list that will hold all the cards
        List<Card> cards = new List<Card>();

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();
        cards = reader.load(path);

        //generates the card using the prefabCard prefab
        GameObject card = (GameObject)Instantiate(PrefabCard, transform.position, transform.rotation);

        card.GetComponent<Card>().name = cards[cardLocation].name;
        card.GetComponent<Card>().health = cards[cardLocation].health;

    }

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(string path)
    {

        //all of the cards in a list<>
        List<Card> cards = new List<Card>(); 

        //loads the cards from the database or deck
        DeckReader reader = new DeckReader();
        cards = reader.load(path);

        //information about the size of the array of cards, and what card the while loop is on
        int totalCards = cards.Count;
        int currentCard = 0;

        //goes through the cards one bye one and adds them to the playing field
        while (currentCard <= totalCards)
        {
            //sets the in game card's values equal to the values gotten from the list<>
            //Card = cards[currentCard];

            //generates the card using the card prefab
            GameObject card = (GameObject)Instantiate(PrefabCard, transform.position, transform.rotation);

            card.GetComponent<Card>().name = cards[currentCard].name;

            currentCard++;
        }
    }
}
