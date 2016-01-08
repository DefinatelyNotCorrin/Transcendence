﻿using UnityEngine;
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
    public GameObject Player1Data;
    public GameObject Player2Data;
    public string tempName;
	public bool p1IsLocked;
	public bool p2IsLocked;
	public Button startMatch;
	public Button menuButton;
	public AudioClip clickSound;
	public AudioClip heavyClickSound;
	public AudioSource menuAudio;
	public List<Deck> deckPool = new List<Deck>();
	public List<string> deckNames;
    public List<string> paths;
    int i = 0;

	// Use this for initialization
	void Start () {

		exitMenu = exitMenu.GetComponent<Canvas> ();
		helperPrompt = helperPrompt.GetComponent<Canvas> ();
		menuAudio = GetComponent<AudioSource>();

		exitMenu.enabled = false;
		helperPrompt.enabled = false;

        DontDestroyOnLoad(Player1Data);
        DontDestroyOnLoad(Player2Data);

        paths.Add("/scripts/xml/cards.xml");
        paths.Add("/scripts/xml/2cards.xml");
        paths.Add("/scripts/xml/3cards.xml"); 

        foreach (string p in paths)
        {

            //TEMP: method to create deck pool from all usable decks, for now, creates same deck off archive
            string path = Application.dataPath + p;

            DeckReader reader = new DeckReader();

            List<Card> database = reader.load(path);

            Deck deck = new Deck(path, i.ToString());

            foreach (Card c in database)
            {
                deck.archiveDeck.Add(c);
            }

            deckPool.Add(deck);
            i++;
        }

        foreach (Deck d in deckPool)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(d.name);
            p1Selector.options.Add(data);
            p2Selector.options.Add(data);
            Debug.Log("Option added of name " + d.name);
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
        tempName = p1Selector.GetComponent<Dropdown>().captionText.text;

        foreach (Deck d in deckPool)
        {
            if (d.name.Equals(tempName))
            {
                Player1Data.GetComponent<player>().deckPath = d.path;
            }
        }
	}

	//player2 selector
	public void DeckSelectP2(){
		menuAudio.PlayOneShot(clickSound, 0.7F);
        tempName = p2Selector.GetComponent<Dropdown>().captionText.text;

        foreach (Deck d in deckPool)
        {
            if (d.name.Equals(tempName))
            {
                Player2Data.GetComponent<player>().deckPath = d.path;
            }
        }
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
