using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MatchStartScript : MonoBehaviour {
	public Canvas exitMenu;
	public Canvas helperPrompt;
	public Dropdown p1Selector;
	public Dropdown p2Selector;
	public Button p1Lock;
	public Button p2Lock;
	public bool p1IsLocked;
	public bool p2IsLocked;
	public Button startMatch;
	public Button menuButton;
	public AudioClip clickSound;
	public AudioClip heavyClickSound;
	public AudioSource menuAudio;
	public List<Deck> deckPool;
	public List<string> deckNames;

	// Use this for initialization
	void Start () {
		exitMenu = exitMenu.GetComponent<Canvas> ();
		helperPrompt = helperPrompt.GetComponent<Canvas> ();
		menuAudio = GetComponent<AudioSource>();
	//	player1 = deckButton.GetComponent<Button> ();
	//	playButton = playButton.GetComponent<Button> ();
	//	exitButton = exitButton.GetComponent<Button> ();
		exitMenu.enabled = false;
		helperPrompt.enabled = false;

		//TEMP: method to create deck pool from all usable decks, for now, creates same deck off archive
		string path = Application.dataPath + "/scripts/xml/cards.xml";
		DeckReader reader = new DeckReader();
		
		if (File.Exists (path)) {
			List<Card> database = reader.load (path);
			Deck deck = new Deck ();
			deck.name = "testDeckofDatabase";
			deck.activeDeck = new List<Card> (); //MOD ME WHEN CORBIN SYNC
			deck.archiveDeck = new List<Card> ();
			foreach (Card c in database) {
				deck.archiveDeck.Add (c);
			}
			deckPool = new List<Deck> ();
			deckPool.Add(deck);
		} else {
			Debug.Log ("ERROR:Could not find file at path");
		}
		////////////////////TEMP ENDS


		//set the selector options to the string name of each usable deck in deckPool
		foreach (Deck d in deckPool){
			deckNames.Add(d.name);
		}

		foreach (string s in deckNames) {
			Dropdown.OptionData data = new Dropdown.OptionData(s);	
			p1Selector.options.Add(data);
			p2Selector.options.Add(data);
		}

	}

	
	// Update is called once per frame
	void Update () {
		
	}

	//exit match menu popup, disables buttons
	public void MenuPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = true;
		IsComponentsInteractable (false);
	}

	//return to title menu and exit match menu
	public void ExitPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		Application.LoadLevel(1);
	}

	//disables the exit match menu prompt, reenables buttons
	public void CancelPress(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		exitMenu.enabled = false;
		IsComponentsInteractable (true);
	}

	//launches game with selected decks
	public void StartMatch() {
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p1IsLocked && p2IsLocked) { //are both players locked?
		Application.LoadLevel (1); //Level 1 should be the match scene
		}
		else {
			helperPrompt.enabled = true;
			IsComponentsInteractable (false);
		}
	}

	//dissmiss helper prompt with OK
	public void DismissPrompt() {
		helperPrompt.enabled = false;
		IsComponentsInteractable (true);
	}

	//player1 selector
	public void DeckSelectP1(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		//p1Selector.options = deckNames;
	}

	//player2 selector
	public void DeckSelectP2(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
		//p1Selector.options = deckNames;
	}
	

	//when player1 lock is pressed
	public void LockPressP1(){
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p1Selector.interactable) {
			p1Selector.interactable = false;
			p1IsLocked = true;
		} else {
			p1Selector.interactable = true;
			p1IsLocked = false;
		}
	}

	//when player2 lock is pressed
	public void LockPressP2(){
		menuAudio.PlayOneShot(heavyClickSound, 0.7F);
		if (p2Selector.interactable) {
			p2Selector.interactable = false;
			p2IsLocked = true;
		} else {
			p2Selector.interactable = true;
			p2IsLocked = false;
		}
	}

	//sets components of menu to all disabled or all interactable
	public void IsComponentsInteractable(bool state){
		//if statements prevent locking, opening exit menu, canceling, and overriding lock state
		if (!p1IsLocked) {
			p1Selector.interactable = state;
		}
		if (!p2IsLocked) {
			p2Selector.interactable = state;
		}
		p1Lock.interactable = state;
		p2Lock.interactable = state;
		startMatch.interactable = state;
		menuButton.interactable = state;
	}
}
