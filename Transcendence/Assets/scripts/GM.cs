using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour
{

    //the prefab and the spawn locations
    public GameObject PrefabCard;
    public RectTransform cardSpawnTemple;
    public RectTransform cardSpawnCitadel;

    //should create one single card from a list of cards, if ID and PATH for the XML file are known
    public void generateCard(GameObject player)
    {

		//string path, int cardLocation
        //a list that will hold all the cards
        List<Card> cards = new List<Card>();

        player.GetComponent<player>().deckPath = Application.dataPath + player.GetComponent<player>().deckPath;

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();
        cards = reader.load(player.GetComponent<player>().deckPath);

        //generates the card using the prefabCard prefab
		Debug.Log ("Method Called");

        if (player.GetComponent<player>().playerSide == 0)
        {
            GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnTemple.transform.position, cardSpawnTemple.rotation);

            card.transform.SetParent(cardSpawnTemple.transform.parent);

            card.GetComponentInChildren<Text>().text = cards[0].name;

            cards[0].setCurrents();

            //passes the Card card values into the Gameobject card
            card.GetComponent<Card>().name = cards[0].name;
            card.GetComponent<Card>().ID = cards[0].ID;
            card.GetComponent<Card>().image = cards[0].image;
            card.GetComponent<Card>().description = cards[0].description;
            card.GetComponent<Card>().alliance = cards[0].alliance;
            card.GetComponent<Card>().type = cards[0].type;
            card.GetComponent<Card>().cost = cards[0].cost;
            card.GetComponent<Card>().attack = cards[0].attack;
            card.GetComponent<Card>().health = cards[0].health;
            card.GetComponent<Card>().defense = cards[0].defense;
            card.GetComponent<Card>().range = cards[0].range;
            card.GetComponent<Card>().target = cards[0].target;
            card.GetComponent<Card>().currentID = cards[0].currentID;
            card.GetComponent<Card>().currentCost = cards[0].currentCost;
            card.GetComponent<Card>().currentAttack = cards[0].currentAttack;
            card.GetComponent<Card>().currentHealth = cards[0].currentHealth;
            card.GetComponent<Card>().currentDefense = cards[0].currentDefense;
            card.GetComponent<Card>().currentRange = cards[0].currentRange;
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

        if (player.GetComponent<player>().playerSide == 1)
        {
            GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnTemple.transform.position, cardSpawnTemple.rotation);

            card.transform.SetParent(cardSpawnTemple.transform.parent);

            card.GetComponentInChildren<Text>().text = cards[0].name;

            cards[0].setCurrents();

            //passes the Card card values into the Gameobject card
            card.GetComponent<Card>().name = cards[0].name;
            card.GetComponent<Card>().ID = cards[0].ID;
            card.GetComponent<Card>().image = cards[0].image;
            card.GetComponent<Card>().description = cards[0].description;
            card.GetComponent<Card>().alliance = cards[0].alliance;
            card.GetComponent<Card>().type = cards[0].type;
            card.GetComponent<Card>().cost = cards[0].cost;
            card.GetComponent<Card>().attack = cards[0].attack;
            card.GetComponent<Card>().health = cards[0].health;
            card.GetComponent<Card>().defense = cards[0].defense;
            card.GetComponent<Card>().range = cards[0].range;
            card.GetComponent<Card>().target = cards[0].target;
            card.GetComponent<Card>().currentID = cards[0].currentID;
            card.GetComponent<Card>().currentCost = cards[0].currentCost;
            card.GetComponent<Card>().currentAttack = cards[0].currentAttack;
            card.GetComponent<Card>().currentHealth = cards[0].currentHealth;
            card.GetComponent<Card>().currentDefense = cards[0].currentDefense;
            card.GetComponent<Card>().currentRange = cards[0].currentRange;
            card.GetComponent<Card>().battleSide = cardSpawnTemple.GetComponent<cardSpawn1>().battleSide;

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

    //should create cards for each one in a deck and then instanstiate them in the game
    public void generateDeck(GameObject player)
    {

        //all of the cards in a list<>
        List<Card> cards = new List<Card>();

        player.GetComponent<player>().deckPath = Application.dataPath + player.GetComponent<player>().deckPath;

        //loads the cards into the list from an XML file
        DeckReader reader = new DeckReader();
        cards = reader.load(player.GetComponent<player>().deckPath);

        //information about the size of the array of cards, and what card the while loop is on
        int totalCards = cards.Count;
        int currentCard = 0;

        //goes through the cards one bye one and adds them to the playing field
        while (currentCard <= totalCards)
        {
            if (player.GetComponent<player>().playerSide == currentCard)
            {
                GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnTemple.transform.position, cardSpawnTemple.rotation);

                card.transform.SetParent(cardSpawnTemple.transform.parent);

                card.GetComponentInChildren<Text>().text = cards[currentCard].name;

                cards[currentCard].setCurrents();

                //passes the Card card values into the Gameobject card
                card.GetComponent<Card>().name = cards[currentCard].name;
                card.GetComponent<Card>().ID = cards[currentCard].ID;
                card.GetComponent<Card>().image = cards[currentCard].image;
                card.GetComponent<Card>().description = cards[currentCard].description;
                card.GetComponent<Card>().alliance = cards[currentCard].alliance;
                card.GetComponent<Card>().type = cards[currentCard].type;
                card.GetComponent<Card>().cost = cards[currentCard].cost;
                card.GetComponent<Card>().attack = cards[currentCard].attack;
                card.GetComponent<Card>().health = cards[currentCard].health;
                card.GetComponent<Card>().defense = cards[currentCard].defense;
                card.GetComponent<Card>().range = cards[currentCard].range;
                card.GetComponent<Card>().target = cards[currentCard].target;
                card.GetComponent<Card>().currentID = cards[currentCard].currentID;
                card.GetComponent<Card>().currentCost = cards[currentCard].currentCost;
                card.GetComponent<Card>().currentAttack = cards[currentCard].currentAttack;
                card.GetComponent<Card>().currentHealth = cards[currentCard].currentHealth;
                card.GetComponent<Card>().currentDefense = cards[currentCard].currentDefense;
                card.GetComponent<Card>().currentRange = cards[currentCard].currentRange;
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
                currentCard++;
            }

            if (player.GetComponent<player>().playerSide == 1)
            {
                GameObject card = (GameObject)Instantiate(PrefabCard, cardSpawnTemple.transform.position, cardSpawnTemple.rotation);

                card.transform.SetParent(cardSpawnTemple.transform.parent);

                card.GetComponentInChildren<Text>().text = cards[currentCard].name;

                cards[currentCard].setCurrents();

                //passes the Card card values into the Gameobject card
                card.GetComponent<Card>().name = cards[currentCard].name;
                card.GetComponent<Card>().ID = cards[currentCard].ID;
                card.GetComponent<Card>().image = cards[currentCard].image;
                card.GetComponent<Card>().description = cards[currentCard].description;
                card.GetComponent<Card>().alliance = cards[currentCard].alliance;
                card.GetComponent<Card>().type = cards[currentCard].type;
                card.GetComponent<Card>().cost = cards[currentCard].cost;
                card.GetComponent<Card>().attack = cards[currentCard].attack;
                card.GetComponent<Card>().health = cards[currentCard].health;
                card.GetComponent<Card>().defense = cards[currentCard].defense;
                card.GetComponent<Card>().range = cards[currentCard].range;
                card.GetComponent<Card>().target = cards[currentCard].target;
                card.GetComponent<Card>().currentID = cards[currentCard].currentID;
                card.GetComponent<Card>().currentCost = cards[currentCard].currentCost;
                card.GetComponent<Card>().currentAttack = cards[currentCard].currentAttack;
                card.GetComponent<Card>().currentHealth = cards[currentCard].currentHealth;
                card.GetComponent<Card>().currentDefense = cards[currentCard].currentDefense;
                card.GetComponent<Card>().currentRange = cards[currentCard].currentRange;
                card.GetComponent<Card>().battleSide = cardSpawnTemple.GetComponent<cardSpawn1>().battleSide;

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
                currentCard++;
            }
            
        }
    }
}
