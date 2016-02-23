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

    // Player interaction 
    public Button player1EndTurnButton;
    public Button player2EndTurnButton;

    // Player stats
    public Text player1Mana;
    public Text player2Mana;
    public Text player1VP;
    public Text player2VP;

    // Important sprites, objects, and prefabs
    public Sprite cardBackTemple;
    public Sprite cardBackCitadel;
    public Sprite clear;
    public Canvas switchPlayerMenu;
    public GameObject PrefabCreatureCard;
    public GameObject PrefabSpellCard;
    public GameObject player1;
    public GameObject player2;
    public GameObject currentPlayer;
    public GameObject canvas;

    public RectTransform Player2Hand;

    // All of the sprites for cards
    public Sprite[] spriteSheet;

    // Locations
    private LocationManager locations;

    public void Start()
    {

        switchPlayerMenu.enabled = false;

        player1.GetComponent<player>().deckPath = GameObject.Find("Player1StartData").GetComponent<player>().deckPath;
        player2.GetComponent<player>().deckPath = GameObject.Find("Player2StartData").GetComponent<player>().deckPath;

        spriteSheet = (Sprite[])Resources.LoadAll<Sprite>("");

        this.locations = GameObject.Find("Location Manager").GetComponent<LocationManager>();

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

        // Set player starting mana cap
        player1.GetComponent<player>().manaMax = 2;
        player2.GetComponent<player>().manaMax = 2;

        // Determine who goes first, and start that player's turn
        if (chance == 0)
        {
            Transform hand = locations.getLocationTransform(Location.Player1Hand);

            startTurn(player1);
            currentPlayer = player1;
            player1.GetComponent<player>().isTurn = true;
            player2.GetComponent<player>().isTurn = false;

            foreach (Transform child in hand)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }
            foreach (Transform child in hand)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "Get 20 VP to win.";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Player 1 goes first!";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        }
        else if (chance == 1)
        {
            Transform hand = locations.getLocationTransform(Location.Player2Hand);

            currentPlayer = player2;
            startTurn(player2);
            player1.GetComponent<player>().isTurn = false;
            player2.GetComponent<player>().isTurn = true;

            foreach (Transform child in hand)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }
            foreach (Transform child in hand)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "Get 20 VP to win.";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Player 2 goes first!";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        }
    }

    public void Update()
    {

        // Update mana every frame **** CAUSE WHY NOT? ****
        player1Mana.text = player1.GetComponent<player>().currentMana.ToString();
        player2Mana.text = player2.GetComponent<player>().currentMana.ToString();

        player1VP.text = player1.GetComponent<player>().victoryPoints.ToString();
        player2VP.text = player2.GetComponent<player>().victoryPoints.ToString();

        // Set interactability of "End Turn" buttons based on player turn
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

        if (player1.GetComponent<player>().victoryPoints == 20)
        {
            endGame(player1);
        }

        if (player2.GetComponent<player>().victoryPoints == 20)
        {
            endGame(player2);
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
            // Set Player 1's card back to enabled, and Player 2's card back to clear
            Transform hand1 = locations.getLocationTransform(Location.Player1Hand);

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }

            Transform hand2 = locations.getLocationTransform(Location.Player2Hand);

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            // Give Player 1 all earned VP
            VPTally(player1);

            // Start Player 2's turn
            startTurn(player2);
            player1.GetComponent<player>().isTurn = false;
            player2.GetComponent<player>().isTurn = true;

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now Player 2's turn";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Current Scores:";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "Player 1 has " + player1.GetComponent<player>().victoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "Player 2 has " + player2.GetComponent<player>().victoryPoints + " VP!";

            currentPlayer = player2;
        }

        // Toggles to player 1 turn if Player 2 pressed the button
        else if (player2.GetComponent<player>().isTurn)
        {
            Transform hand1 = locations.getLocationTransform(Location.Player1Hand);

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }

            Transform hand2 = locations.getLocationTransform(Location.Player2Hand);

            foreach (Transform child in hand2)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackCitadel;
            }

            VPTally(player2);

            startTurn(player1);
            player1.GetComponent<player>().isTurn = true;
            player2.GetComponent<player>().isTurn = false;

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now Player 1's turn";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Current Scores:";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "Player 1 has " + player1.GetComponent<player>().victoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "Player 2 has " + player2.GetComponent<player>().victoryPoints + " VP!";

            currentPlayer = player1;
        }

        Debug.Log("The current player is " + currentPlayer.name);
    }

    // The method called when the game is ended
    public void endGame(GameObject winner)
    {
        switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = winner.GetComponent<player>().name + " Wins!";
    }

    // The method that enables/disables the switching player menu
    public void togglePlayerChangeMenu()
    {
        if (switchPlayerMenu.enabled == false)
        {
            switchPlayerMenu.enabled = true;
        }
        else if (switchPlayerMenu.enabled == true)
        {
            switchPlayerMenu.enabled = false;
        }
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

    // Tally a player's VP at the end of their turn
    public void VPTally(GameObject player)
    {
        // Logic for Player 1
        if (player.GetComponent<player>().playerSide == 0)
        {
            if (locations.getLocationTransform(Location.BottomLeft1).childCount == 1 
                || locations.getLocationTransform(Location.BottomLeft2).childCount == 1 
                || locations.getLocationTransform(Location.BottomLeft3).childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }

            if (locations.getLocationTransform(Location.BottomCenter1).childCount == 1 
                || locations.getLocationTransform(Location.BottomCenter2).childCount == 1 
                || locations.getLocationTransform(Location.BottomCenter3).childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }

            if (locations.getLocationTransform(Location.BottomRight1).childCount == 1
                || locations.getLocationTransform(Location.BottomRight2).childCount == 1
                || locations.getLocationTransform(Location.BottomRight3).childCount == 1)
            {
                player1.GetComponent<player>().victoryPoints++;
            }
        }

        // Logic for Player 2
        if (player.GetComponent<player>().playerSide == 1)
        {
            if (locations.getLocationTransform(Location.TopLeft1).childCount == 1
                || locations.getLocationTransform(Location.TopLeft2).childCount == 1
                || locations.getLocationTransform(Location.TopLeft3).childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }

            if (locations.getLocationTransform(Location.TopCenter1).childCount == 1
                || locations.getLocationTransform(Location.TopCenter2).childCount == 1
                || locations.getLocationTransform(Location.TopCenter3).childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }

            if (locations.getLocationTransform(Location.TopRight1).childCount == 1
                || locations.getLocationTransform(Location.TopRight2).childCount == 1
                || locations.getLocationTransform(Location.TopRight3).childCount == 1)
            {
                player2.GetComponent<player>().victoryPoints++;
            }
        }
    }

    // Creates a card a position based on player side and card value
    public void instantiateCard(GameObject player, Card currentCard)
    {
        // Instantiates for cards on temple side
        if (player.GetComponent<player>().playerSide == 0)
        {
            Transform hand = locations.getLocationTransform(Location.Player1Hand);

            if (hand.childCount < 6) //if at hand limit, throw out card
            {
                GameObject card;

                if (currentCard.type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(PrefabCreatureCard, hand.position, hand.rotation);

                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }
                else
                {
                    card = (GameObject)Instantiate(PrefabSpellCard, hand.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0,8)];
                }

                //card.transform.SetParent(hand.parent);

                card.GetComponentInChildren<Text>().text = currentCard.cardName;

                currentCard.setCurrents();

                //passes the Card card values into the GameObject card
                card.GetComponent<Card>().cardName = currentCard.cardName;
                if (currentCard.cardName == null)
                {
                    Debug.Log("NULL NAME");
                }
                card.name = currentCard.cardName;
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
                card.GetComponent<Card>().effect = currentCard.effect;


                //moves the card into the canvas
                card.transform.SetParent(hand);
                //moves the card to the spawn
                card.GetComponent<isDraggable>().parentToReturnTo = hand;

                //sets the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cost;
            }
        }

        //instantiates for players on citadel side
        if (player.GetComponent<player>().playerSide == 1)
        {
            if (Player2Hand.transform.childCount < 6) { //if at hand limit, throw out card

                GameObject card;

                if (currentCard.type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(PrefabCreatureCard, Player2Hand.transform.position, Player2Hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }
                else
                {
                    card = (GameObject)Instantiate(PrefabSpellCard, Player2Hand.transform.position, Player2Hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }

                card.transform.SetParent(Player2Hand.transform.parent);

                card.GetComponentInChildren<Text>().text = currentCard.cardName;

                currentCard.setCurrents();

                //passes the Card card values into the Gameobject card
                card.GetComponent<Card>().cardName = currentCard.cardName;
                card.name = currentCard.cardName;
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
                card.GetComponent<Card>().effect = currentCard.effect;

                //moves the card into the canvas
                card.transform.SetParent(Player2Hand);
                //moves the card to the spawn
                card.GetComponent<isDraggable>().parentToReturnTo = Player2Hand;

                //sets the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = card.GetComponent<Card>().cost;
            }
         }
    }

    public void exitMenuPress()
    {
        if (GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled)
        {
            GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameObject.Find("ExitMeby").GetComponent<Canvas>().enabled = true;
        }
    }

    public void exitMenu()
    {
        Application.LoadLevel(1);
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
                Debug.Log(deck[10].cardName);
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



