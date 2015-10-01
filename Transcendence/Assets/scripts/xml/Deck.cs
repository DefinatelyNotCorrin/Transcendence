using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
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

    public int getArchiveSize() //returns size of deck before match started
    {
        return archiveDeck.Count;
    }

    public int getDeckSize() //returns size of deck as it is, active, in the game
    {
        return activeDeck.Count;
    }

    public int getDiscardSize() //returns size of discard list
    {
        return discard.Count;
    }

    public Card poll() //returns card of index 0 in deck, removes card from list
    {
        Card card = activeDeck[0];
        activeDeck.RemoveAt(0);
        return card;
    }

    public Card poll(int i) //returns card of index i in deck, removes card from list
    {
        Card card = activeDeck[i];
        activeDeck.RemoveAt(i);
        return card;
    }

    public Card peek() //returns card of index 0 in deck, does not remove card
    {
        return activeDeck[0];
    }

    public Card peek(int i) //returns card of index i in deck, does not remove card
    {
        return activeDeck[i];
    }

    public void insertCardToArchive(Card card, int index) //adds card to archive deck at specified index
    {
        archiveDeck.Insert(index, card);
    }

    public void insertCardToActive(Card card, int index) //adds card to active deck at specified index, deck top is 0
    {
        activeDeck.Insert(index, card);
    }

    public void insertCardToDiscard(Card card, int index) //adds card to discard at specified index, discard top is 0
    {
        discard.Insert(index, card);
    }

    public void resetActive() //clears active Deck and sets it equal to archive
    {
        activeDeck.Clear();
        foreach (Card c in archiveDeck)
        {
            activeDeck.Add(c);
        }
    }
    
    public void shuffle() { /*
        //shuffling - Fisher-Yates method
        int count = getDeckSize();
        //float r = Random.Range(0,count);
        while (count > 1) //go through each element
        {
            //random number from 0 to range of unshuffled deck
            int randomDraw = (int)(Random.Range(0, count));

    //take whatever is drawn and swap it with the end of the list
    Card temp = activeDeck[randomDraw];
    activeDeck[randomDraw] = activeDeck[count - 1];
            activeDeck[count - 1] = temp;

            count--;

        } */
    }

    //SORTING,SHUFFLING,SERIALIZING

}
