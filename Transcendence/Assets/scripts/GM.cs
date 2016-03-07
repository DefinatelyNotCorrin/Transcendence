using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

// TODO refactor several methods with the current player object
public class GM : MonoBehaviour
{
    /*
    IMPORTANT NUMBERS:
    Temple is 0, Citadel is 1
    Player 1 is side 0, Player 2 is side 1.
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

    /// <summary>
    /// Set basic information for each player, load their decks, and give them their mulliagns. Then pick a player to go first, and start his/her turn. 
    /// </summary>
    public void Start()
    {
        this.switchPlayerMenu.enabled = false;

        player1.GetComponent<player>().DeckPath = GameObject.Find("Player1StartData").GetComponent<player>().DeckPath;
        player2.GetComponent<player>().DeckPath = GameObject.Find("Player2StartData").GetComponent<player>().DeckPath;

        spriteSheet = Resources.LoadAll<Sprite>("");

        this.locations = GameObject.Find("Location Manager").GetComponent<LocationManager>();

        this.player1.GetComponent<player>().PlayerSide = 0;
        this.player2.GetComponent<player>().PlayerSide = 1;

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
        player1.GetComponent<player>().ManaMax = 2;
        player2.GetComponent<player>().ManaMax = 2;

        // Determine who goes first, and start that player's turn
        if (chance == 0)
        {
            Transform hand1 = locations.getLocationTransform(Location.Player1Hand);
            Transform hand2 = locations.getLocationTransform(Location.Player2Hand);

            Debug.Log("Player 1 goes first.");

            startTurn(player1);
            currentPlayer = player1;

            player1.GetComponent<player>().IsTurn = true;
            player2.GetComponent<player>().IsTurn = false;

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = clear;
            }
            foreach (Transform child in hand2)
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
            Debug.Log("Player 2 goes first.");

            Transform hand1 = locations.getLocationTransform(Location.Player1Hand);
            Transform hand2 = locations.getLocationTransform(Location.Player2Hand);

            currentPlayer = player2;
            startTurn(player2);
            player1.GetComponent<player>().IsTurn = false;
            player2.GetComponent<player>().IsTurn = true;

            foreach (Transform child in hand1)
            {
                child.Find("Card Back").GetComponent<Image>().sprite = cardBackTemple;
            }
            foreach (Transform child in hand2)
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

    /// <summary>
    /// Updates player mana and VP, checks for win conditions, and makes sure that the correct buttons are enabled. 
    /// </summary>
    public void Update()
    {

        // Update mana every frame **** CAUSE WHY NOT? ****
        player1Mana.text = player1.GetComponent<player>().CurrentMana.ToString();
        player2Mana.text = player2.GetComponent<player>().CurrentMana.ToString();

        player1VP.text = player1.GetComponent<player>().VictoryPoints.ToString();
        player2VP.text = player2.GetComponent<player>().VictoryPoints.ToString();

        // Set interactability of "End Turn" buttons based on player turn
        if (player1.GetComponent<player>().IsTurn)
        {
            player1EndTurnButton.interactable = true;
            player2EndTurnButton.interactable = false;
        }

        if (player2.GetComponent<player>().IsTurn)
        {
            player1EndTurnButton.interactable = false;
            player2EndTurnButton.interactable = true;
        }

        if (player1.GetComponent<player>().VictoryPoints == 20)
        {
            endGame(player1);
        }

        if (player2.GetComponent<player>().VictoryPoints == 20)
        {
            endGame(player2);
        }
    }


    /// <summary>
    /// Starts the turn of a player by giving him/her mana and a card. 
    /// </summary>
    /// <param name="player"></param> The player whose turn is being started. 
    public void startTurn(GameObject player)
    {
        if (player.GetComponent<player>().ManaMax < 13)
        {
            player.GetComponent<player>().ManaMax++;
        }
        player.GetComponent<player>().CurrentMana = player.GetComponent<player>().ManaMax;

        drawCard(player);
    }

    /// <summary>
    /// Ends the turn of the current player, tallies the VP of both players, and then starts the turn of the other player.
    /// </summary>
    public void endTurn()
    {

        // Toggles to Player 2 turn if Player 1 pressed the button
        if (player1.GetComponent<player>().IsTurn)
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
            player1.GetComponent<player>().IsTurn = false;
            player2.GetComponent<player>().IsTurn = true;

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now Player 2's turn";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Current Scores:";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "Player 1 has " + player1.GetComponent<player>().VictoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "Player 2 has " + player2.GetComponent<player>().VictoryPoints + " VP!";

            currentPlayer = player2;
        }

        // Toggles to player 1 turn if Player 2 pressed the button
        else if (player2.GetComponent<player>().IsTurn)
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
            player1.GetComponent<player>().IsTurn = true;
            player2.GetComponent<player>().IsTurn = false;

            togglePlayerChangeMenu();
            switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "It is now Player 1's turn";
            switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = "Current Scores:";
            switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "Player 1 has " + player1.GetComponent<player>().VictoryPoints + " VP!";
            switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "Player 2 has " + player2.GetComponent<player>().VictoryPoints + " VP!";

            currentPlayer = player1;
        }

        Debug.Log("The current player is " + currentPlayer.name);
    }

    /// <summary>
    /// Displays the victory screen. 
    /// </summary>
    /// <param name="winner"></param> The player who has won. 
    public void endGame(GameObject winner)
    {
        switchPlayerMenu.transform.Find("CurrentPlayer").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Player1Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("Player2Score").GetComponent<Text>().text = "";
        switchPlayerMenu.transform.Find("CurrentScore:").GetComponent<Text>().text = winner.GetComponent<player>().name + " Wins!";
    }

    /// <summary>
    /// Toggle the meby that displays between players' turns. 
    /// </summary>
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

    /// <summary>
    /// Runs combat between two cards.
    /// </summary>
    /// <param name="attackingCard"></param> The card attacking.
    /// <param name="defendingCard"></param> The card being attacked. 
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
        instantiateCard(player, player.GetComponent<player>().Deck.poll());
    }


    /// <summary>
    /// Tally a player's VP at the end of their turn
    /// </summary>
    /// <param name="player"></param> The player whose turn it currently is. 
    public void VPTally(GameObject player)
    {
        // Logic for Player 1
        if (player.GetComponent<player>().PlayerSide == 0)
        {
            if (locations.getLocationTransform(Location.BottomLeft1).childCount == 1 
                || locations.getLocationTransform(Location.BottomLeft2).childCount == 1 
                || locations.getLocationTransform(Location.BottomLeft3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }

            if (locations.getLocationTransform(Location.BottomCenter1).childCount == 1 
                || locations.getLocationTransform(Location.BottomCenter2).childCount == 1 
                || locations.getLocationTransform(Location.BottomCenter3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }

            if (locations.getLocationTransform(Location.BottomRight1).childCount == 1
                || locations.getLocationTransform(Location.BottomRight2).childCount == 1
                || locations.getLocationTransform(Location.BottomRight3).childCount == 1)
            {
                player1.GetComponent<player>().VictoryPoints++;
            }
        }

        // Logic for Player 2
        if (player.GetComponent<player>().PlayerSide == 1)
        {
            if (locations.getLocationTransform(Location.TopLeft1).childCount == 1
                || locations.getLocationTransform(Location.TopLeft2).childCount == 1
                || locations.getLocationTransform(Location.TopLeft3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }

            if (locations.getLocationTransform(Location.TopCenter1).childCount == 1
                || locations.getLocationTransform(Location.TopCenter2).childCount == 1
                || locations.getLocationTransform(Location.TopCenter3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }

            if (locations.getLocationTransform(Location.TopRight1).childCount == 1
                || locations.getLocationTransform(Location.TopRight2).childCount == 1
                || locations.getLocationTransform(Location.TopRight3).childCount == 1)
            {
                player2.GetComponent<player>().VictoryPoints++;
            }
        }
    }

    /// <summary>
    /// Create a card gameobject in the hand of a player.
    /// </summary>
    /// <param name="player"></param> The player to whom the card will be given.
    /// <param name="currentCard"></param> The next card that shall be drawn in that player's deck. 
    public void instantiateCard(GameObject player, Card currentCard)
    {
        // Instantiate card for player 1
        if (player.GetComponent<player>().PlayerSide == 0)
        {
            Transform hand = locations.getLocationTransform(Location.Player1Hand);

            if (hand.childCount < 6) //if at hand limit, throw out card
            {
                GameObject card;
                Card cardsScript;

                // Create the card with creature prefab
                if (currentCard.type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(PrefabCreatureCard, hand.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }
                // Or create it using the spell prefab
                else
                {
                    card = (GameObject)Instantiate(PrefabSpellCard, hand.position, hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0,8)];
                }

                // Set the reference to the new card's script
                cardsScript = card.GetComponent<Card>();

                // Set the card's transform to that of its player's hand
                card.transform.SetParent(Player2Hand.transform.parent);

                // Initialize the card's current values
                currentCard.setCurrents();

                // Set the card's name
                card.GetComponentInChildren<Text>().text = currentCard.cardName;
                cardsScript.cardName = currentCard.cardName;
                card.name = currentCard.cardName;

                // Set all of the remaining information for the card
                cardsScript.ID = currentCard.ID;
                cardsScript.image = currentCard.image;
                cardsScript.description = currentCard.description;
                cardsScript.alliance = currentCard.alliance;
                cardsScript.type = currentCard.type;
                cardsScript.cost = currentCard.cost;
                cardsScript.attack = currentCard.attack;
                cardsScript.health = currentCard.health;
                cardsScript.defense = currentCard.defense;
                cardsScript.range = currentCard.range;
                cardsScript.target = currentCard.target;
                cardsScript.currentID = currentCard.currentID;
                cardsScript.currentCost = currentCard.currentCost;
                cardsScript.currentAttack = currentCard.currentAttack;
                cardsScript.currentHealth = currentCard.currentHealth;
                cardsScript.currentDefense = currentCard.currentDefense;
                cardsScript.currentRange = currentCard.currentRange;
                cardsScript.ownerTag = "Player 1";
                cardsScript.effectName = currentCard.effectName;

                // Set the effect for each card
                foreach (Effect effect in Enum.GetValues(typeof(Effect)))
                {
                    // Set the effect if the card is a spell and has an effect
                    if (effect.ToString().Equals(currentCard.effectName))
                    {
                        cardsScript.effect = effect;
                        Debug.Log(effect.ToString());
                    }
                }

                // Move the card onto the canvas
                card.transform.SetParent(hand);

                // Set the parentToReturnTo for the card
                card.GetComponent<isDraggable>().parentToReturnTo = hand;

                // Set the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = cardsScript.cardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = cardsScript.description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = cardsScript.attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = cardsScript.defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = cardsScript.health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = cardsScript.range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = cardsScript.cost;
            }
        }

        // Instanstiate card for player 2
        if (player.GetComponent<player>().PlayerSide == 1)
        {
            if (Player2Hand.transform.childCount < 6) { //if at hand limit, throw out card

                // The new card, and its script
                GameObject card;
                Card cardsScript;

                // Create the card with the creature prefab
                if (currentCard.type.Equals("Creature"))
                {
                    card = (GameObject)Instantiate(PrefabCreatureCard, Player2Hand.transform.position, Player2Hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }
                // Or create it with the spell prefab
                else
                {
                    card = (GameObject)Instantiate(PrefabSpellCard, Player2Hand.transform.position, Player2Hand.rotation);
                    card.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
                }

                // Set the reference to the new card's script
                cardsScript = card.GetComponent<Card>();

                // Set the card's transform to that of its player's hand
                card.transform.SetParent(Player2Hand.transform.parent);

                // Initialize the card's current values
                currentCard.setCurrents();

                // Set the card's name
                card.GetComponentInChildren<Text>().text = currentCard.cardName;
                cardsScript.cardName = currentCard.cardName;
                card.name = currentCard.cardName;

                // Set all of the remaining information for the card
                cardsScript.ID = currentCard.ID;
                cardsScript.image = currentCard.image;
                cardsScript.description = currentCard.description;
                cardsScript.alliance = currentCard.alliance;
                cardsScript.type = currentCard.type;
                cardsScript.cost = currentCard.cost;
                cardsScript.attack = currentCard.attack;
                cardsScript.health = currentCard.health;
                cardsScript.defense = currentCard.defense;
                cardsScript.range = currentCard.range;
                cardsScript.target = currentCard.target;
                cardsScript.currentID = currentCard.currentID;
                cardsScript.currentCost = currentCard.currentCost;
                cardsScript.currentAttack = currentCard.currentAttack;
                cardsScript.currentHealth = currentCard.currentHealth;
                cardsScript.currentDefense = currentCard.currentDefense;
                cardsScript.currentRange = currentCard.currentRange;
                cardsScript.ownerTag = "Player 2";
                cardsScript.effectName = currentCard.effectName;

                // Set the effect for each card
                foreach (Effect effect in Enum.GetValues(typeof(Effect)))
                {
                    // Set the effect if the card is a spell and has an effect
                    if (effect.ToString().Equals(currentCard.effectName))
                    {
                        cardsScript.effect = effect;
                        Debug.Log(effect.ToString());
                    }
                }

                // Move the card onto the canvas
                card.transform.SetParent(Player2Hand);

                // Set the parentToReturnTo for the card
                card.GetComponent<isDraggable>().parentToReturnTo = Player2Hand;

                // Set the visible attributes of the card game object to those stored in it's card script parameters
                card.transform.Find("Title").gameObject.GetComponent<Text>().text = cardsScript.cardName;
                card.transform.Find("Description").gameObject.GetComponent<Text>().text = cardsScript.description;
                card.transform.Find("Attack").gameObject.GetComponent<Text>().text = cardsScript.attack;
                card.transform.Find("Defense").gameObject.GetComponent<Text>().text = cardsScript.defense;
                card.transform.Find("Health").gameObject.GetComponent<Text>().text = cardsScript.health;
                card.transform.Find("Range").gameObject.GetComponent<Text>().text = cardsScript.range;
                card.transform.Find("Cost").gameObject.GetComponent<Text>().text = cardsScript.cost;
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
        SceneManager.LoadScene(1);
    }

    /*
    The following methods are for testing purposes only, and are cut down versions of what will be in game logic
    */

    /*
    Here is old logic I may want to see later:
        // Runs once at the start of Player 1's turn
        //if (player1.GetComponent<player>().IsTurn == true && player1.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player1Mana.text = player1.GetComponent<player>().CurrentMana.ToString();
        //    player1.GetComponent<player>().hasStartedGoing = true;

        //}

        // Runs once at the start of Player 2's turn
        //if (player2.GetComponent<player>().IsTurn == true && player2.GetComponent<player>().hasStartedGoing == false)
        //{
        //    turn(player1);
        //    player2Mana.text = player2.GetComponent<player>().CurrentMana.ToString();
        //    player2.GetComponent<player>().hasStartedGoing = true;

        //}

        // The check for the end turn buttons loop
        //if (player1.GetComponent<player>().IsTurn == true)
        //{
        //    player1EndTurnButton.interactable = true;
        //    player2EndTurnButton.interactable = true;
        //}

        //if (player2.GetComponent<player>().IsTurn == true)
        //{
        //    player2EndTurnButton.interactable = true;
        //    player1EndTurnButton.interactable = true;
        //}

                /*List<Card> deck = new List<Card>();

        DeckPath = Application.dataPath + "/scripts/xml/cards.xml";

        DeckReader reader = new DeckReader();

        List<Card> archiveDeck = reader.load(DeckPath);

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

        
    String DeckPath = Application.dataPath + "/scripts/xml/cards.xml";

    DeckReader reader = new DeckReader();

    List<Card> deck = new List<Card>();
    List<Card> archiveDeck = reader.load(DeckPath);

        for (int i = 0; i<archiveDeck.Count; i++)
        {
            deck.Add(archiveDeck[i]);
        }
*/

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(GameObject player)
    {
        string path;
        path = Application.dataPath + player.GetComponent<player>().DeckPath;

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

        //player.GetComponent<player>().DeckPath = Application.dataPath + player.GetComponent<player>().DeckPath;

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();

        path = Application.dataPath + player.GetComponent<player>().DeckPath;

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



