using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections.Generic;
using System;

public class GM : MonoBehaviour
{
    /*
    IMPORTANT NUMBERS:
    Temple is 0, Citadel is 1
    Player 1 is side 0, Player 2 is side 1
    */

    // Prefabs, locations, players, buttons, and readouts 

    public bool hasEndedTurn;
    public Button player1EndTurnButton;
    public Button player2EndTurnButton;
    public Text player1Mana;
    public Text player2Mana;
    public Text player1VP;
    public Text player2VP;
    public Sprite cardBackTemple;
    public Sprite cardBackCitadel;
    public Sprite clear;
    public GameObject PrefabCard;
    public GameObject player1;
    public GameObject player2;
    public GameObject canvas;
    public GameObject playerHandTemple;
    public GameObject playerHandCitadel;
    public GameObject bottomLeft1;
    public GameObject bottomLeft2;
    public GameObject bottomLeft3;
    public GameObject bottomMiddle1;
    public GameObject bottomMiddle2;
    public GameObject bottomMiddle3;
    public GameObject bottomRight1;
    public GameObject bottomRight2;
    public GameObject bottomRight3;
    public GameObject topLeft1;
    public GameObject topLeft2;
    public GameObject topLeft3;
    public GameObject topMiddle1;
    public GameObject topMiddle2;
    public GameObject topMiddle3;
    public GameObject topRight1;
    public GameObject topRight2;
    public GameObject topRight3;
    public RectTransform cardSpawnTemple;
    public RectTransform cardSpawnCitadel;


    public void Start()
    {

        // The initial mulligan
        player1.GetComponent<player>().loadDeck();
        player2.GetComponent<player>().loadDeck();

        drawCard(player1);
        drawCard(player2);

        drawCard(player1);
        drawCard(player2);

        drawCard(player1);
        drawCard(player2);

        drawCard(player1);
        drawCard(player2);

        int chance = UnityEngine.Random.Range(0, 2);

        Debug.Log(chance);

        // Set player starting mana cap
        player1.GetComponent<player>().manaMax = 2;
        player2.GetComponent<player>().manaMax = 2;

        // Determine who goes first, and start that player's turn
        if (chance == 1)
        {
            startTurn(player1);
            player1.GetComponent<player>().isTurn = true;
            player2.GetComponent<player>().isTurn = false;

            foreach (Transform child in playerHandTemple.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }
            foreach (Transform child in playerHandCitadel.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }
        }
        else if (chance == 0)
        {
            startTurn(player2);
            player1.GetComponent<player>().isTurn = false;
            player2.GetComponent<player>().isTurn = true;

            foreach (Transform child in playerHandTemple.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }
            foreach (Transform child in playerHandCitadel.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }
        }
    }

    public void Update()
    {

        // Update mana every frame **** CAUSE WHY NOT? ****
        player1Mana.text = player1.GetComponent<player>().currentMana.ToString();
        player2Mana.text = player2.GetComponent<player>().currentMana.ToString();

        player1VP.text = player1.GetComponent<player>().victoryPoints.ToString();
        player2VP.text = player2.GetComponent<player>().victoryPoints.ToString();

        // Toggle "End Turn" buttons based on player turn
        if (player1.GetComponent<player>().isTurn)
        {
            player1EndTurnButton.interactable = true;
            player2EndTurnButton.interactable = false;
        }

        if (player2.GetComponent<player>().isTurn)
        {
            player1EndTurnButton.interactable = false;
            player2EndTurnButton.interactable = true;

        }
    }

    // Method that runs at the start of a player's turn
    public void startTurn(GameObject player)
    {
        if (player.GetComponent<player>().manaMax < 13)
        {
            player.GetComponent<player>().manaMax++;
        }
        player.GetComponent<player>().currentMana = player.GetComponent<player>().manaMax;

        drawCard(player);
    }

    // What happens when a player ends their turn
    public void endTurn()
    {

        // Toggles to Player 2 turn if Player 1 pressed the button
        if (player1.GetComponent<player>().isTurn)
        {

            // Set the Player 1's card back to enabled, and Player 2's card back to clear
            foreach (Transform child in playerHandTemple.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }
            foreach (Transform child in playerHandCitadel.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            // Give Player 1 all earned VP
            VPTally(player1);

            // Start Player 2's turn
            startTurn(player2);
            player1.GetComponent<player>().isTurn = false;
            player2.GetComponent<player>().isTurn = true;

        }

        // Toggles to player 1 turn if Player 2 pressed the button
        else if (player2.GetComponent<player>().isTurn)
        {
            
            foreach (Transform child in playerHandTemple.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }
            foreach (Transform child in playerHandCitadel.transform)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }

            VPTally(player2);

            startTurn(player1);
            player1.GetComponent<player>().isTurn = true;
            player2.GetComponent<player>().isTurn = false;
        }
    }

    // The method called when the game is ended
    public void endGame(player winner)
    {
        Debug.Log(winner.name + " wins!");
    }

    // Combat method for cards
    public static void combat(GameObject attackingCard, GameObject defendingCard)
    {

        if (attackingCard == null || defendingCard == null)
        {
            Debug.Log("Card is null");
        }

        // Attacking card combat
        attackingCard.GetComponent<Card>().currentHealth = attackingCard.GetComponent<Card>().currentHealth - defendingCard.GetComponent<Card>().currentAttack;

        // Defending card combat
        defendingCard.GetComponent<Card>().currentHealth = defendingCard.GetComponent<Card>().currentHealth - attackingCard.GetComponent<Card>().currentAttack;

        attackingCard.transform.Find("Health").gameObject.GetComponent<Text>().text = attackingCard.GetComponent<Card>().currentHealth.ToString();
        defendingCard.transform.Find("Health").gameObject.GetComponent<Text>().text = defendingCard.GetComponent<Card>().currentHealth.ToString();

        //Destroy dead cards
        if (attackingCard.GetComponent<Card>().currentHealth <= 0)
        {
            Destroy(attackingCard);
            Debug.Log("Attacking Card Died!");
        }
        else {
            attackingCard.gameObject.tag = "Card";
            attackingCard.GetComponent<Card>().isExhausted = true;
        }

        if (defendingCard.GetComponent<Card>().currentHealth <= 0)
        {
            Destroy(defendingCard);
            Debug.Log("Defending Card Died!");
        }

    }

    // Draw a card method for the player
    public void drawCard(GameObject player)
    {        
        instantiateCard(player, player.GetComponent<player>().deck.poll());
    }

    //Tally a player's VP at the end of their turn
    public void VPTally(GameObject player)
    {
        // Logic for Player 1
        if (player.GetComponent<player>().playerSide == 0)
        {
            
            if (bottomLeft1.transform.childCount == 1 || bottomLeft2.transform.childCount == 1 || bottomLeft3.transform.childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }

            if (bottomMiddle1.transform.childCount == 1 || bottomMiddle2.transform.childCount == 1 || bottomMiddle3.transform.childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }

            if (bottomRight1.transform.childCount == 1 || bottomRight2.transform.childCount == 1 || bottomRight3.transform.childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }
        }

        // Logic for Player 2
        if (player.GetComponent<player>().playerSide == 0)
        {

            if (topLeft1.transform.childCount == 1 || topLeft2.transform.childCount == 1 || topLeft3.transform.childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }

            if (topMiddle1.transform.childCount == 1 || topMiddle2.transform.childCount == 1 || topMiddle3.transform.childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }

            if (topRight1.transform.childCount == 1 || topRight2.transform.childCount == 1 || topRight3.transform.childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }
        }

    }

    // Creates a card a position based on player side and card value
    public void instantiateCard(GameObject player, Card currentCard)
    {
        //instantiates for cards on temple side
        if (player.GetComponent<player>().playerSide == 0)
        {

            GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnTemple.transform.position, cardSpawnTemple.rotation);

            card.transform.SetParent(cardSpawnTemple.transform.parent);

            card.GetComponentInChildren<Text>().text = currentCard.name;

            currentCard.setCurrents();

            //passes the Card card values into the Gameobject card
            card.GetComponent<Card>().name = currentCard.name;
            card.GetComponent<Card>().ID = currentCard.ID;
            card.GetComponent<Card>().image = currentCard.image;
            card.GetComponent<Card>().description = currentCard.description;
            card.GetComponent<Card>().alliance = currentCard.alliance;
            card.GetComponent<Card>().type = currentCard.type;
            card.GetComponent<Card>().cost = currentCard.cost;
            card.GetComponent<Card>().attack = currentCard.attack;
            card.GetComponent<Card>().health = currentCard.health;
            card.GetComponent<Card>().defense = currentCard.defense;
            card.GetComponent<Card>().range = currentCard.range;
            card.GetComponent<Card>().target = currentCard.target;
            card.GetComponent<Card>().currentID = currentCard.currentID;
            card.GetComponent<Card>().currentCost = currentCard.currentCost;
            card.GetComponent<Card>().currentAttack = currentCard.currentAttack;
            card.GetComponent<Card>().currentHealth = currentCard.currentHealth;
            card.GetComponent<Card>().currentDefense = currentCard.currentDefense;
            card.GetComponent<Card>().currentRange = currentCard.currentRange;
            card.GetComponent<Card>().ownerTag = "Player 1";
            

            //moves the card into the canvas
            card.transform.SetParent(cardSpawnTemple);
            //moves the card to the spawn
            card.GetComponent<isDraggable>().parentToReturnTo = cardSpawnTemple;

            //sets the visible attributes of the card game object to those stored in it's card script parameters
            card.transform.Find("Title").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().name;
            card.transform.Find("Description").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().description;
            card.transform.Find("Attack").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().attack;
            card.transform.Find("Defense").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().defense;
            card.transform.Find("Health").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().health;
            card.transform.Find("Range").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().range;
            card.transform.Find("Cost").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cost;
        }

        //instantiates for players on citadel side
        if (player.GetComponent<player>().playerSide == 1)
        {
            GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnCitadel.transform.position, cardSpawnCitadel.rotation);

            card.transform.SetParent(cardSpawnCitadel.transform.parent);

            card.GetComponentInChildren<Text>().text = currentCard.name;

            currentCard.setCurrents();

            //passes the Card card values into the Gameobject card
            card.GetComponent<Card>().name = currentCard.name;
            card.GetComponent<Card>().ID = currentCard.ID;
            card.GetComponent<Card>().image = currentCard.image;
            card.GetComponent<Card>().description = currentCard.description;
            card.GetComponent<Card>().alliance = currentCard.alliance;
            card.GetComponent<Card>().type = currentCard.type;
            card.GetComponent<Card>().cost = currentCard.cost;
            card.GetComponent<Card>().attack = currentCard.attack;
            card.GetComponent<Card>().health = currentCard.health;
            card.GetComponent<Card>().defense = currentCard.defense;
            card.GetComponent<Card>().range = currentCard.range;
            card.GetComponent<Card>().target = currentCard.target;
            card.GetComponent<Card>().currentID = currentCard.currentID;
            card.GetComponent<Card>().currentCost = currentCard.currentCost;
            card.GetComponent<Card>().currentAttack = currentCard.currentAttack;
            card.GetComponent<Card>().currentHealth = currentCard.currentHealth;
            card.GetComponent<Card>().currentDefense = currentCard.currentDefense;
            card.GetComponent<Card>().currentRange = currentCard.currentRange;
            card.GetComponent<Card>().ownerTag = "Player 2";

            //moves the card into the canvas
            card.transform.SetParent(cardSpawnCitadel);
            //moves the card to the spawn
            card.GetComponent<isDraggable>().parentToReturnTo = cardSpawnCitadel;

            //sets the visible attributes of the card game object to those stored in it's card script parameters
            card.transform.Find("Title").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().name;
            card.transform.Find("Description").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().description;
            card.transform.Find("Attack").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().attack;
            card.transform.Find("Defense").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().defense;
            card.transform.Find("Health").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().health;
            card.transform.Find("Range").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().range;
            card.transform.Find("Cost").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cost;
        }
    }

    /*
    The following methods are for testing purposes only, and are cut down versions of what will be in game logic
    */

    /*
    Here is old logic I may want to see later:
        // Runs once at the start of Player 1's turn
        //if (player1.GetComponent<player>().isTurn == true && player1.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player1Mana.text = player1.GetComponent<player>().currentMana.ToString();
        //    player1.GetComponent<player>().hasStartedGoing = true;

        //}

        // Runs once at the start of Player 2's turn
        //if (player2.GetComponent<player>().isTurn == true && player2.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player2Mana.text = player2.GetComponent<player>().currentMana.ToString();
        //    player2.GetComponent<player>().hasStartedGoing = true;

        //}

        // The check for the end turn buttons loop
        //if (player1.GetComponent<player>().isTurn == true)
        //{
        //    player1EndTurnButton.interactable = true;
        //    player2EndTurnButton.interactable = true;
        //}

        //if (player2.GetComponent<player>().isTurn == true)
        //{
        //    player2EndTurnButton.interactable = true;
        //    player1EndTurnButton.interactable = true;
        //}

                /*List<Card> deck = new List<Card>();

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

        ///// UPDATE USING THE DECK THAT IS IN THE PLAYER OBJECT - MAKE SURE THE PLAYER DECK IS LOADED IN START!

        
    String deckPath = Application.dataPath + "/scripts/xml/cards.xml";

    DeckReader reader = new DeckReader();

    List<Card> deck = new List<Card>();
    List<Card> archiveDeck = reader.load(deckPath);

        for (int i = 0; i<archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);
        }
*/

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(GameObject player)
    {
        string path;
        path = Application.dataPath + player.GetComponent<player>().deckPath;

        List<Card> cards = new List<Card>();
        DeckReader reader = new DeckReader();

        if (path.Equals(Application.dataPath + ""))
        {
            //EditorUtility.DisplayDialog("No Deck Selected", "You need to load a deck!", "Alright. God.");
            Application.Quit();
        }
        else
        {
            cards = reader.load(path);

            //information about the size of the array of cards, and what card the while loop is on
            int totalCards = cards.Count;
            int currentCard = 0;

            //goes through the cards one by one and adds them to the playing field
            while (currentCard < totalCards)
            {
                instantiateCard(player, cards[currentCard]);
                currentCard++;
            }
        }
    }

    //should create one single card from a list of cards, given player object
    public void generateCard(GameObject player)
    {
        string path;
        int card = 0;
        //string path, int cardLocation
        //a list that will hold all the cards
        List<Card> cards = new List<Card>();

        //player.GetComponent<player>().deckPath = Application.dataPath + player.GetComponent<player>().deckPath;

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();

        path = Application.dataPath + player.GetComponent<player>().deckPath;

        if (path.Equals(Application.dataPath + ""))
        {
            //EditorUtility.DisplayDialog("No Deck Selected", "You need to load a deck!", "Alright. God.");
        }
        else
        {
            cards = reader.load(path);
            instantiateCard(player, cards[card]);
            card++;

            if (card == cards.Count)
            {
                card = 0;
            }
        }

        //generates the card using the prefabCard prefab
        Debug.Log("Method Called");

    }
}



