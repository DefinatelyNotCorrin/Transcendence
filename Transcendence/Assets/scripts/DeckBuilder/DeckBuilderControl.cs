using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilderControl : MonoBehaviour {

    //CardBook
    public Canvas cardBookCanvas;
        //Database
        
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
            SavePress();
        }
    }

    public void LoadPress()
    {
        //load previously saved deck, prompt for save first (call save press)
    }
    public void SavePress()
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
    {
        bookAudio.PlayOneShot(clickSound, 0.7F);
        SceneManager.LoadScene(0);
    }

    public void OnClickCancel()
    {
        bookAudio.PlayOneShot(clickSound, 0.7F);

        helperPrompt.GetComponent<PromptMenuScript>().isVisible(false);

    }

}
