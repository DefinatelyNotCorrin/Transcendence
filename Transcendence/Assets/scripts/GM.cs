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

    //the prefab and the spawn locations
    bool isFirstTurn = false;
    public GameObject PrefabCard;
    public RectTransform cardSpawnTemple;
    public RectTransform cardSpawnCitadel;
    public GameObject player1;
    public GameObject player2;
    public GameObject canvas;
    int cardnum = 4;
    string path;

    int card = 0;

    //Temple is 0 and Citadel is 1

    public void Start()
    {
        drawCard(player1);
        drawCard(player2);
    }

    public void Update()
    {
        //starts player 1 turn
        if (player1.GetComponent<player>().isTurn == true && player1.GetComponent<player>().hasStartedGoing == false)
        {
            turn(player1);
            player1.GetComponent<player>().hasStartedGoing = true;
            player2.GetComponent<player>().isTurn = false;
            player2.GetComponent<player>().hasStartedGoing = false;
        }

        //start player 2 turn
        if (player2.GetComponent<player>().isTurn == true && player2.GetComponent<player>().hasStartedGoing == false)
        {
            turn(player1);
            player2.GetComponent<player>().hasStartedGoing = true;
            player1.GetComponent<player>().isTurn = false;
            player1.GetComponent<player>().hasStartedGoing = false;
        }

        //the combat loop
        if (player1.GetComponent<player>().hasStartedGoing = true && player1.GetComponent<player>().isTurn == true)
        {
            
        }

        if (player1.GetComponent<player>().hasStartedGoing = true && player1.GetComponent<player>().isTurn == true)
        {

        }

    }
    
    // First turn setup that runs at the start of the game
    public void firstTurn(GameObject player)
    {
        drawCard(player);
    }

    // Turn that runs during a player's turn
    public void turn(GameObject player)
    {
        if (player.GetComponent<player>().manaMax < 15)
        {
            player.GetComponent<player>().manaMax++;
        }
        player.GetComponent<player>().currentMana = player.GetComponent<player>().manaMax;

        drawCard(player);
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

        
        List<Card> deck = new List<Card>();

        String deckPath = Application.dataPath + "/scripts/xml/cards.xml";

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
        

        instantiateCard(player, deck[cardnum]);
        cardnum++;
        player.GetComponent<player>().currentDrawCard++;
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
            card.GetComponent<Card>().battleSide = player.GetComponent<player>().playerSide;

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
            card.GetComponent<Card>().battleSide = player.GetComponent<player>().playerSide;

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

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(GameObject player)
    {

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



