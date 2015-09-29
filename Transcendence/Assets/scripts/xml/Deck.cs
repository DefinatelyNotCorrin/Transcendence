using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

    public List<Card> archiveDeck;
    public List<Card> activeDeck;
    public List<Card> discard;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int getArchiveSize() //returns size of deck before match started
    {
        return archiveDeck.Count;
    }

    int getDeckSize() //returns size of deck as it is, active, in the game
    {
        return activeDeck.Count;
    }

    int getDiscardSize() //returns size of discard list
    {
        return discard.Count;
    }

    Card poll() //returns card of index 0 in deck, removes card from list
    {
        Card card = activeDeck[0];
        activeDeck.RemoveAt(0);
        return card;
    }

    Card poll(int i) //returns card of index i in deck, removes card from list
    {
        Card card = activeDeck[i];
        activeDeck.RemoveAt(i);
        return card;
    }

    Card peek() //returns card of index 0 in deck, does not remove card
    {
        return activeDeck[0];
    }

    Card peek(int i) //returns card of index i in deck, does not remove card
    {
        return activeDeck[i];
    }

    void insertCardToArchive(Card card, int index) //adds card to archive deck at specified index
    {
        archiveDeck.Insert(index, card);
    }

    void insertCardToActive(Card card, int index) //adds card to active deck at specified index, deck top is 0
    {
        activeDeck.Insert(index, card);
    }

    void insertCardToDiscard(Card card, int index) //adds card to discard at specified index, discard top is 0
    {
        discard.Insert(index, card);
    }

    void resetActive() //clears active Deck and sets it equal to archive
    {
        activeDeck.Clear();
        foreach (Card c in archiveDeck)
        {
            activeDeck.Add(c);
        }
    }


    //SORTING,SHUFFLING,SERIALIZING

}
