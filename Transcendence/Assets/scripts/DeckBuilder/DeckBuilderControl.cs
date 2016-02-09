using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeckBuilderControl : MonoBehaviour {

    //CardBook
    public Canvas cardBookCanvas;
        //Prefabs
        public GameObject PrefabCreatureCard;
        public GameObject PrefabSpellCard;
        //Top Row
        public Button card1;
        public Button card2;
        public Button card3;
        public Button card4;
        public Button card5;
        //Bottom Row
        public Button card6;
        public Button card7;
        public Button card8;
        public Button card9;
        public Button card10;
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
    public InputField deckName;
    public Button cardInDeckPrefab;
     
    
    
    //Audio
    public AudioSource bookAudio;
    public AudioClip cardAddSound;
    public AudioClip cardAddCancelSound;
    public AudioClip clickSound;
    public AudioClip heavyClickSound;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
