using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class CardBook : MonoBehaviour
{
	
	//Prefabs
	public GameObject creatureCardPrefab;
	public GameObject spellCardPrefab;
	//Reader
	private string path;
	public Deck Database { get; set; } //all cards of database in selected alliance and not in current build
	public Deck CurrentBuild { get; set; }//all cards in the deck the player is creating


    void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}		

	public int GetArchiveSize() //returns size of database of all obtainable cards
	{
		return Database.archiveDeck.Count;
	}	

	public int GetCurrentBuildSize()
	{
        return Database.activeDeck.Count;
	}
		
}

