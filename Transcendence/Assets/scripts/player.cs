using UnityEngine;
using System.Collections.Generic;

public class player : MonoBehaviour {

    public int playerSide = 0;
    public int baseHP = 30;
    public int currentHP;
    public int manaMax;
    public int currentMana;
    public int victoryPoints;
    public Deck deck;
    public bool isTurn = false;
    public string deckPath;

    // Use this for initialization
    void Start ()
    {
        victoryPoints = 0;
        currentHP = baseHP;
        deckPath = Application.dataPath + "/scripts/xml/cards.xml";
    }

    // 
    public void loadDeck()
    {
        deck = new Deck();

        DeckReader reader = new DeckReader();

        List<Card> stock = reader.load(deckPath);

        foreach (Card c in stock)
        {
            deck.archiveDeck.Add(c);
        }

        deck.resetActive();
        deck.shuffle();
    }
}
