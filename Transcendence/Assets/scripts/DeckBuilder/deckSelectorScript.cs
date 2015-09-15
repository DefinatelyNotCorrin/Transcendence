using UnityEngine;
using System.Collections;
using Unity.UI;


public class deckSelectorScript : MonoBehaviour {

    public int deckSelectorMenuValue = 0;
    public Dropdown deckSelector;

    //XML nonsense needed to read in the list of decks with names, ect.
    //public int numberOfDecks;

    // should set the deckSekector menu selection value to the deckSelectorMenuValue variable
    public void setMenuValue() { 
        deckSelector.value = deckSelectorMenuValue; 
	}

    public int getMenuValue()
    {   // should return the value of what option is selected
        if (deckSelectorMenuValue != null)
        {
            return deckSelectorMenuValue;
        }
        else // returns zero to stop a null issue from happening, since 0 is the label
        {
            return 0;
        }
    }
}
