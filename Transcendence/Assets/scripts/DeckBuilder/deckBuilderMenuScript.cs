using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class deckBuilderMenuScript : MonoBehaviour
{

    public Button newDeckButton;
    public Button editDeckButton;
    public Button backButton;
    public AudioClip clickSound;
    public AudioClip heavyClickSound;
    public AudioSource menuAudio;

    // Use this for initialization
    void Start()
    {
        //menuAudio = GetComponent<AudioSource>();
        //newDeckButton = newDeckButton.GetComponent<Button>();
        //editDeckButton = editDeckButton.GetComponent<Button>();
        //backButton = backButton.GetComponent<Button>();

    }

    //for when the Create Deck button is pressed
    public void CreatePress()
    {
        menuAudio.PlayOneShot(heavyClickSound, 0.7F);
        Application.LoadLevel(-1); //Level number should be the create menu for the deck builder

    }

    //for when the Edit Deck button is pressed
    public void EditPress()
    {
        menuAudio.PlayOneShot(heavyClickSound, 0.7F);
        Application.LoadLevel(-1); //Level number should be the edit menu for the deck builder

    }

    //for when the back button is pressed
    public void BackPress()
    {
        menuAudio.PlayOneShot(heavyClickSound, 0.7F);
       //Level number should be the main menu
        Application.LoadLevel(0);

    }
}
