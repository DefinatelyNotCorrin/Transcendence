using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class effectDatabase : MonoBehaviour {

    public GM gm = new GM();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    // Basic fireball method. Does damage based on the attack of the spell card played. 
    public void fireball(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();
    }

    public void heal(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth + spellCard.GetComponent<Card>().currentHealth;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();
    }

    public void zoneDamage(GameObject targtedCard, GameObject spellCard)
    {
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleTemplePlayingSpaces"))
        {
            if (GameObject.Find("bottomCenter1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("bottomCenter3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("bottomCenter3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("bottomCenter3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightTemplePlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("leftCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topLeft1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topLeft2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topLeft3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topLeft3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topLeft3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("middleCitadelPlayingSpaces"))
        {
            if (GameObject.Find("top Middle1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("top Middle1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("top Middle2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("top Middle2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("top Middle3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("top Middle3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
        }
        if (targtedCard.transform.parent.transform.parent.name.Equals("rightCitadelPlayingSpaces"))
        {
            if (GameObject.Find("topRight1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topRight2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();
            }
            if (GameObject.Find("topRight3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topRight3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("topRight3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();
            }
        }

        Destroy(spellCard);

    }

    public void draw(GameObject spellCard)
    {
       
        if (spellCard.GetComponent<Card>().battleSide == 0)
        {
            for (int i = 0; i < spellCard.GetComponent<Card>().currentCost; i++)
            {
                gm.drawCard(GameObject.Find("Player1"));
            }
        }

        if (spellCard.GetComponent<Card>().battleSide == 1)
        {
            for (int i = 0; i < spellCard.GetComponent<Card>().currentCost; i++)
            {
                gm.drawCard(GameObject.Find("Player2"));
            }
        }
    }
}
