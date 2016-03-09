using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilderControl : MonoBehaviour {

    //CardBook
    public Canvas cardBookCanvas;
    public List<Card> displayedCards;
    //Database
        private string Path { get; set; } //path of full database xml
        public Deck Database { get; set; } //all cards of database in selected alliance and not in current build
        public Deck CurrentBuild { get; set; }//all cards in the deck the player is creating
    //Prefabs
        public GameObject creatureCardPrefab;
        public GameObject spellCardPrefab;
        //Top Row
        public Button slot1;
        public Button slot2;
        public Button slot3;
        public Button slot4;
        public Button slot5;
        //Bottom Row
        public Button slot6;
        public Button slot7;
        public Button slot8;
        public Button slot9;
        public Button slot10;
        //Page Controls
        public Button upPage;
        public Button downPage;
        //Taskbar
        public Button newDeck;
        public Button loadDeck;
        public Button exit;
        public Button save;
        public Toggle creaturesToggle;
        public Toggle spellsToggle;
        public Toggle boonsToggle;
    //DeckPanel
    public Canvas deckPanelCanvas;
        private bool isSaved;
        public Deck currentDeck;
        public InputField deckName;
        public Button cardInDeckPrefab;
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
        helperPrompt = (GameObject)Instantiate(helperPrompt, this.transform.position, this.transform.rotation);
        helperPrompt.GetComponent<PromptMenuScript>().isVisible(false);
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
        for (int i = 0; i < 10; i++) {
            GameObject card;
            Card current = Database.poll(0);
            //helperPrompt = (GameObject)Instantiate(helperPrompt, this.transform.position, this.transform.rotation);
            displayedCards.Add(current);
            // Create the card with creature prefab
            if (current.type.Equals("Creature"))
            {
                card = (GameObject)Instantiate(creatureCardPrefab, hand.position, hand.rotation);
                card.transform.FindChild("Splash").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)]; //TODO: nonrandom sprites
            }
            // Or create it using the spell prefab
            else
            {
                card = (GameObject)Instantiate(spellCardPrefab, hand.position, hand.rotation);
                card.transform.FindChild("Splash").gameObject.GetComponent<Image>().sprite = spriteSheet[UnityEngine.Random.Range(0, 8)];
            }
            //quick reference card's script component
            Card cardsScript = card.GetComponent<Card>();
            // Set the card's transform to that of its player's hand
            switch (i)
            {
                case 1:
                    card.transform.SetParent(slot1.transform.parent);
                    break;
                case 2:
                    card.transform.SetParent(slot2.transform.parent);
                    break;
                case 3:
                    card.transform.SetParent(slot3.transform.parent);
                    break;
                case 4:
                    card.transform.SetParent(slot4.transform.parent);
                    break;
                case 5:
                    card.transform.SetParent(slot5.transform.parent);
                    break;
                case 6:
                    card.transform.SetParent(slot6.transform.parent);
                    break;
                case 7:
                    card.transform.SetParent(slot7.transform.parent);
                    break;
                case 8:
                    card.transform.SetParent(slot8.transform.parent);
                    break;
                case 9:
                    card.transform.SetParent(slot9.transform.parent);
                    break;
                case 10:
                    card.transform.SetParent(slot10.transform.parent);
                    break;
            }

            // Initialize the card's current values
            current.setCurrents();

            // Set the card's name
            card.GetComponentInChildren<Text>().text = current.cardName;

            //TODO: set all values of script to those of current, or just set equal to that of current
            cardsScript.cardName = currentCard.cardName;
            card.name = currentCard.cardName;

            // Set all of the remaining information for the card
            cardsScript.ID = currentCard.ID;
            cardsScript.Image = currentCard.Image;
            cardsScript.Description = currentCard.Description;
            cardsScript.Alliance = currentCard.Alliance;
            cardsScript.Type = currentCard.Type;
            cardsScript.Cost = currentCard.Cost;
            cardsScript.Attack = currentCard.Attack;
            cardsScript.Health = currentCard.Health;
            cardsScript.Defense = currentCard.Defense;
            cardsScript.Range = currentCard.Range;
            cardsScript.Target = currentCard.Target;
            cardsScript.CurrentID = currentCard.CurrentID;
            cardsScript.CurrentCost = currentCard.CurrentCost;
            cardsScript.CurrentAttack = currentCard.CurrentAttack;
            cardsScript.CurrentHealth = currentCard.CurrentHealth;
            cardsScript.CurrentDefense = currentCard.CurrentDefense;
            cardsScript.CurrentRange = currentCard.CurrentRange;
            cardsScript.OwnerTag = "Player 1";
            cardsScript.EffectName = currentCard.EffectName;

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
        bookAudio.PlayOneShot(clickSound, 0.7F);
        SceneManager.LoadScene(0);
    }

    public void OnClickCancel()
        //hide the helper prompt without taking action
    {
        bookAudio.PlayOneShot(clickSound, 0.7F);

        helperPrompt.GetComponent<PromptMenuScript>().isVisible(false);

    }

}