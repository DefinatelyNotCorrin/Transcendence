using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections.Generic;

public class GM : MonoBehaviour
{

    //the prefab and the spawn locations
    bool isFirstTurn = false;
    public GameObject PrefabCard;
    public RectTransform cardSpawnTemple;
    public RectTransform cardSpawnCitadel;
    public GameObject player1;
    public GameObject player2;
    public GameObject canvas;

    string path;

    int card = 0;

    //Temple is 0 and Citadel is 1

    public void Start()
    {
        firstTurn(player1);
        firstTurn(player2);
    }

    public void Update()
    {
        //starts player 1 turn
        if (player1.GetComponent<player>().isTurn == true && player1.GetComponent<player>().hasStartedGoing == false)
        {
            turn(player1);
            player1.GetComponent<player>().hasStartedGoing = true;
            player2.GetComponent<player>().hasStartedGoing = true;
        }

        //start player 2 turn
        if (player2.GetComponent<player>().isTurn == true && player2.GetComponent<player>().hasStartedGoing == false)
        {
            turn(player1);
            player2.GetComponent<player>().hasStartedGoing = true;
        }

        //the combat loop
        if (player1.GetComponent<player>().hasStartedGoing = true && player1.GetComponent<player>().isTurn == true)
        {
            
        }

        if (player1.GetComponent<player>().hasStartedGoing = true && player1.GetComponent<player>().isTurn == true)
        {

        }

    }
    

    public void firstTurn(GameObject player)
    {

    }

    public void turn(GameObject player)
    {

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
        Debug.Log ("Method Called");

    }

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

    public void instantiateCard(GameObject player, Card currentCard)
    {
        //instantiates for cards on citadel side
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

        //instantiates for players on temple side
        if (player.GetComponent<player>().playerSide == 1)
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
        }
    }
}



