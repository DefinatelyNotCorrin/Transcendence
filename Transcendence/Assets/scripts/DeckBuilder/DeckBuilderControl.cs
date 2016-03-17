﻿using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilderControl : MonoBehaviour {    
    //CardBook
    private Canvas cardBookCanvas;
    //Database
    private string Path { get; set; } //path of full database xml
        private Deck Database { get; set; } //all cards of database in selected alliance and not in current build
        private Deck CurrentBuild { get; set; }//all cards in the deck the player is creating
    public List<Card> DisplayedCards;
    private Sprite[] spriteSheet;
    //Prefabs
        private GameObject creatureCardPrefab;
        private GameObject spellCardPrefab;
    //Top Row
    private Button slot1;
    private Button slot2;
    private Button slot3;
    private Button slot4;
    private Button slot5;
    //Bottom Row
    private Button slot6;
    private Button slot7;
    private Button slot8;
    private Button slot9;
    private Button slot10;
    //Page Controls
    private Button upPage;
    private Button downPage;
    //Taskbar
    private Button newDeck;
    private Button loadDeck;
    private Button exit;
    private Button save;
    private Toggle creaturesToggle;
    private Toggle spellsToggle;
    private Toggle boonsToggle;
    //DeckPanel
    private Canvas deckPanelCanvas;
        private bool isSaved;
        private Deck currentDeck;
        private InputField deckName;
        private Button cardInDeckPrefab;
    //Audio
    public AudioSource bookAudio;
        public AudioClip pageTurn;
        public AudioClip cardAddSound;
        public AudioClip cardAddCancelSound;
        public AudioClip clickSound;
        public AudioClip heavyClickSound;
    //Prompt Menu Prefab    
        public GameObject helperPrompt; 

    // Use this for initialization
    void Start () {
        //prefabs
        creatureCardPrefab = Resources.Load("prefabs/displayedCard", typeof(GameObject)) as GameObject;
        spellCardPrefab = Resources.Load("prefabs/displayedCard", typeof(GameObject)) as GameObject;
        //load the database in from XML
        slot1 = GameObject.Find("Slot1").transform.FindChild("Template").GetComponent<Button>();
        slot2 = GameObject.Find("Slot2").transform.FindChild("Template").GetComponent<Button>();
        slot3 = GameObject.Find("Slot3").transform.FindChild("Template").GetComponent<Button>();
        slot4 = GameObject.Find("Slot4").transform.FindChild("Template").GetComponent<Button>();
        slot5 = GameObject.Find("Slot5").transform.FindChild("Template").GetComponent<Button>();
        slot6 = GameObject.Find("Slot6").transform.FindChild("Template").GetComponent<Button>();
        slot7 = GameObject.Find("Slot7").transform.FindChild("Template").GetComponent<Button>();
        slot8 = GameObject.Find("Slot8").transform.FindChild("Template").GetComponent<Button>();
        slot9 = GameObject.Find("Slot9").transform.FindChild("Template").GetComponent<Button>();
        slot10 = GameObject.Find("Slot10").transform.FindChild("Template").GetComponent<Button>();
        upPage = GameObject.Find("RightPage").GetComponent<Button>();
        downPage = GameObject.Find("LeftPage").GetComponent<Button>();
        newDeck = GameObject.Find("New Deck Button").GetComponent<Button>();
        loadDeck = GameObject.Find("Load Deck Button").GetComponent<Button>();

        spriteSheet = Resources.LoadAll<Sprite>(""); 

        exit = GameObject.Find("Exit Button").GetComponent<Button>();
        save = GameObject.Find("Save Button").GetComponent<Button>();
        creaturesToggle = GameObject.Find("Creatures Toggle").GetComponent<Toggle>();
        spellsToggle = GameObject.Find("Spells Toggle").GetComponent<Toggle>();
        boonsToggle = GameObject.Find("Boons Toggle").GetComponent<Toggle>();
       //helperPrompt = (Resources.Load("prefabs/PromptMenu")) as GameObject;
        helperPrompt = (GameObject)Instantiate(Resources.Load("prefabs/PromptMenu"), transform.position, transform.rotation);
        helperPrompt.GetComponent<PromptMenuScript>().isVisible(false);
        populate();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void IsComponentsInteractable(bool state)
        //disables all interaction
        {
        
        slot1.interactable = state;
        slot2.interactable = state;
        slot3.interactable = state;
        slot4.interactable = state;
        slot5.interactable = state;
        slot6.interactable = state;
        slot7.interactable = state;
        slot8.interactable = state;
        slot9.interactable = state;
        slot10.interactable = state;
        upPage.interactable = state;
        downPage.interactable = state;
        newDeck.interactable = state;
        exit.interactable = state;
        save.interactable = state;
        creaturesToggle.interactable = state;
        spellsToggle.interactable = state;
        boonsToggle.interactable = state;
        deckName.interactable = state;
        cardInDeckPrefab.interactable = state;
        
    }
	private void populate(){
      //populate page with 10 cards from those viable for current player's build
        //load database array from path using Reader
        Path = Application.dataPath + "/scripts/xml/spellsWithModifiers.xml";
        DeckReader reader = new DeckReader();
        if (File.Exists(Path))
        {
            List<Card> cardList = reader.load(Path);
            Debug.Log("Card List: " + cardList[0].CardName);
            Database = new Deck();
            Database.activeDeck = new List<Card>();
            Database.archiveDeck = new List<Card>();
            foreach (Card c in cardList)
            {
                Database.archiveDeck.Add(c);
            }
            Debug.Log("archiveDeck: " + Database.archiveDeck[0].CardName);
            foreach (Card c in Database.archiveDeck)
            {
                Database.activeDeck.Add(c);
            }
            Database.resetActive();
            Debug.Log("activeDeck: " + Database.activeDeck[0].CardName);
            //TODO: Sort Database Deck by cost and lettering 
        }
        else
        {
            Debug.Log("Error: File not Found");
        }
        //instantiate the objects/visual representation
        for (int i = 1; i <= 10; i++) {
            GameObject card;
            Card current = Database.poll(0);
            Debug.Log("Current:" + current.CardName);
            //helperPrompt = (GameObject)Instantiate(helperPrompt, this.transform.position, this.transform.rotation);
            DisplayedCards.Add(current);
            // Create the card with creature prefab
            if (current.Type.Equals("Creature"))
            {
                card = (GameObject)Instantiate(creatureCardPrefab, slot1.transform.position, slot1.transform.rotation);
                card.transform.FindChild("Splash").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)]; //TODO: nonrandom sprites
            }
            // Or create it using the spell prefab
            else
            {
                card = (GameObject)Instantiate(spellCardPrefab, slot1.transform.position, slot1.transform.rotation);
                card.transform.FindChild("Splash").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
            }
            //quick reference card's script component
            Card cardsScript = card.GetComponent<Card>();
            // Set the card's transform to that of its player's hand
            switch (i)
            {
                case 1:
                    card.transform.SetParent(slot1.transform.parent);
                    card.transform.position = slot1.transform.position;
                    break;
                case 2:
                    card.transform.SetParent(slot2.transform.parent);
                    card.transform.position = slot2.transform.position;
                    break;
                case 3:
                    card.transform.SetParent(slot3.transform.parent);
                    card.transform.position = slot3.transform.position;
                    break;
                case 4:
                    card.transform.SetParent(slot4.transform.parent);
                    card.transform.position = slot4.transform.position;
                    break;
                case 5:
                    card.transform.SetParent(slot5.transform.parent);
                    card.transform.position = slot5.transform.position;
                    break;
                case 6:
                    card.transform.SetParent(slot6.transform.parent);
                    card.transform.position = slot6.transform.position;
                    break;
                case 7:
                    card.transform.SetParent(slot7.transform.parent);
                    card.transform.position = slot7.transform.position;
                    break;
                case 8:
                    card.transform.SetParent(slot8.transform.parent);
                    card.transform.position = slot8.transform.position;
                    break;
                case 9:
                    card.transform.SetParent(slot9.transform.parent);
                    card.transform.position = slot9.transform.position;
                    break;
                case 10:
                    card.transform.SetParent(slot10.transform.parent);
                    card.transform.position = slot10.transform.position;
                    break;
            }

            // Initialize the card's current values
            current.SetCurrents();

            // Set the card's name
            card.GetComponentInChildren<Text>().text = current.CardName;

            //TODO: set all values of script to those of current, or just set equal to that of current
            cardsScript.CardName = current.CardName;
            card.name = current.CardName;

            // Set all of the remaining information for the card
            Debug.Log("ID" + current.ID);
            cardsScript.ID = current.ID;
            cardsScript.Image = current.Image;
            cardsScript.Description = current.Description;
            cardsScript.Alliance = current.Alliance;
            cardsScript.Cost = current.Cost;
            cardsScript.Attack = current.Attack;
            cardsScript.Health = current.Health;
            cardsScript.Defense = current.Defense;
            cardsScript.Range = current.Range;
            cardsScript.OwnerTag = "Player 1";
            cardsScript.EffectName = current.EffectName;
            Debug.Log("ScriptID" + current.ID);

            card.transform.Find("Title").gameObject.GetComponent<Text>().text = cardsScript.CardName;
            card.transform.Find("Description").gameObject.GetComponent<Text>().text = cardsScript.Description;
            card.transform.Find("Attack").gameObject.GetComponent<Text>().text = cardsScript.Attack;
            card.transform.Find("Defense").gameObject.GetComponent<Text>().text = cardsScript.Defense;
            card.transform.Find("Health").gameObject.GetComponent<Text>().text = cardsScript.Health;
            card.transform.Find("Range").gameObject.GetComponent<Text>().text = cardsScript.Range;
            card.transform.Find("Cost").gameObject.GetComponent<Text>().text = cardsScript.Cost;

        }
    }

    public void downPagePress()
    {
        bookAudio.PlayOneShot(pageTurn, 0.7F);
        IsComponentsInteractable(false);
        //Set all card slots to new card prefab/obj, if there is page of that id
    }

    public void upPagePress()
    {
        bookAudio.PlayOneShot(pageTurn, 0.7F);
        IsComponentsInteractable(false);
        //Set all card slots to new card prefab/obj, if there is page of that id
    }

    public void NewDeck()
    {
        if (isSaved)
        {
            currentDeck.vacate();
        }
        else
        {
            //this canvas.enabled = false
            OnClickSave();
        }
    }

    public void OnClickLoad()
    { 
        //load previously saved deck, prompt for save first (call save press)
    }
    public void OnClickSave()
    //save current
    {
        if (!helperPrompt.GetComponent<PromptMenuScript>().mutation.Equals("save"))
        //if the current mutation is not already exit, set all values to those needed for an exit prompt
        {
            helperPrompt.GetComponent<PromptMenuScript>().moveToCenter();
            helperPrompt.GetComponent<PromptMenuScript>().setMutation("save");
            helperPrompt.GetComponent<PromptMenuScript>().setDescription("Save current deck?");
            helperPrompt.GetComponent<PromptMenuScript>().setLeftButtonText("Save");
            helperPrompt.GetComponent<PromptMenuScript>().setRightButtonText("Cancel");
            //helperPrompt.transform.FindChild("Canvas").transform.FindChild("Button1").GetComponent<Button>().onClick.AddListener(delegate () { OnClickExit(); });
            //helperPrompt.transform.FindChild("Canvas").transform.FindChild("Button2").GetComponent<Button>().onClick.AddListener(delegate () { OnClickCancel(); });
        }
        helperPrompt.GetComponent<PromptMenuScript>().isVisible(true);
    }
    public void ExitPress()
    //return to title menu
    {
        if (!helperPrompt.GetComponent<PromptMenuScript>().mutation.Equals("exit"))
            //if the current mutation is not already exit, set all values to those needed for an exit prompt
        {
                helperPrompt.GetComponent<PromptMenuScript>().moveToCenter();
                helperPrompt.GetComponent<PromptMenuScript>().setMutation("exit");
                helperPrompt.GetComponent<PromptMenuScript>().setDescription("Exit to menu?");
                helperPrompt.GetComponent<PromptMenuScript>().setLeftButtonText("Exit");
                helperPrompt.GetComponent<PromptMenuScript>().setRightButtonText("Cancel");
                helperPrompt.transform.FindChild("Canvas").transform.FindChild("Button1").GetComponent<Button>().onClick.AddListener(delegate () { OnClickExit(); });
                helperPrompt.transform.FindChild("Canvas").transform.FindChild("Button2").GetComponent<Button>().onClick.AddListener(delegate () { OnClickCancel(); });
         }
        helperPrompt.GetComponent<PromptMenuScript>().isVisible(true);
    }

    public void OnClickExit()
        //exit the scene
    {
        Debug.Log("EXIT_PRESS");
        bookAudio.PlayOneShot(clickSound, 0.7F);
        SceneManager.LoadScene(0);
    }

    public void OnClickCancel()
        //hide the helper prompt without taking action
    {
        Debug.Log("CANCEL_PRESS");
        bookAudio.PlayOneShot(clickSound, 0.7F);
        helperPrompt.GetComponent<PromptMenuScript>().isVisible(false);

    }

    public void NewPress()
    {
        //TODO:Prompt for save, shift populate to start
    }

}