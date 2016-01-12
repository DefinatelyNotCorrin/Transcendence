using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class effectDatabase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void fireball(GameObject defendingCard, GameObject spellCard)
    {
        defendingCard.GetComponent<Card>().currentHealth--;
        DestroyObject(spellCard);

        defendingCard.transform.FindChild("Health").GetComponent<Text>().text = defendingCard.GetComponent<Card>().currentHealth.ToString();
    }
}
