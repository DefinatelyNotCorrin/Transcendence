using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DisplayedCard : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (this.GetComponent<Card>().Count < instead, let's 
        //currentDeck.Add(DisplayedCard);
        GameObject.Find("DeckBuilder").GetComponent<DeckBuilderControl>().CurrentDeck.activeDeck.Add(this.GetComponent<Card>());

    }

    // Use this for initialization
    void Start () {
	
	}
	
	void Update () {
	
	}

    
}
