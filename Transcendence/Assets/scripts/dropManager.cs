using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class dropManager : MonoBehaviour {

    public GM gm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void drop(PointerEventData data, GameObject draggingCard)
    {
        isDraggable d = data.pointerDrag.GetComponent<isDraggable>();

        // Checks to see if the card is a card and the thing below it is a field
        if (data.pointerCurrentRaycast.gameObject.CompareTag("Field")
            && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced == false
            && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Creature"))
        {
            // Runs card placement for player 1
            if ((data.pointerCurrentRaycast.gameObject.GetComponent<Field>().side.Equals("Player 1")) && gm.player1.GetComponent<player>().isTurn == true
                && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 1"))
                && (gm.player1.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost >= 0))
            {
                // Send card to its new field

                d.parentToReturnTo = this.transform;

                gm.player1.GetComponent<player>().currentMana = gm.player1.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost;

                GameObject.FindGameObjectWithTag("Player 1 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player1.GetComponent<player>().currentMana.ToString();

                // Set the card tag back to "card" 
                GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced = true;
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
            // Runs card placement for player 2
            else if ((data.pointerCurrentRaycast.gameObject.GetComponent<Field>().side.Equals("Player 2")) && gm.player2.GetComponent<player>().isTurn == true && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 2")) && (gm.player2.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost >= 0))
            {
                // Send card to its new field

                d.parentToReturnTo = this.transform;

                gm.player2.GetComponent<player>().currentMana = gm.player2.GetComponent<player>().currentMana - GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().currentCost;

                GameObject.FindGameObjectWithTag("Player 2 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player2.GetComponent<player>().currentMana.ToString();

                // Set the card tag back to "card" 
                GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced = true;
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
            // If neither is valid, card is set back to a card
            else
            {
                Debug.Log("Invalid move!");
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }
        }

        // Checks to see if the card is a card and the thing below it is a card
        else if (data.pointerCurrentRaycast.gameObject.CompareTag("Card")
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().isExhausted == false)
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().hasBeenPlaced == true)
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals(data.pointerCurrentRaycast.gameObject.GetComponent<Card>().ownerTag) == false)
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Creature")))
        {

            // Checks to see if it is the player's turn and they own the card
            if (gm.player1.GetComponent<player>().isTurn && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 1"))
            {
                // Runs combat for the cards
                GM.combat(GameObject.FindGameObjectWithTag("Dragging"), data.pointerCurrentRaycast.gameObject);
            }
            // Checks to see if it is the player's turn and they own the card
            else if (gm.player2.GetComponent<player>().isTurn && GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().ownerTag.Equals("Player 2"))
            {
                // Runs combat for the cards
                GM.combat(GameObject.FindGameObjectWithTag("Dragging"), data.pointerCurrentRaycast.gameObject);
            }
            // If combat is not valid, make the card a card again
            else
            {
                GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
            }

        }

        else if (data.pointerCurrentRaycast.gameObject.CompareTag("Card")
            && (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().type.Equals("Spell")))
        {

            if (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().effect.Equals("fireball"))
            {
                if (!data.pointerCurrentRaycast.gameObject.Equals(null))
                {

                }
                else
                {
                    GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
                }
            }

            if (GameObject.FindGameObjectWithTag("Dragging").GetComponent<Card>().effect.Equals("heal"))
            {
                if (!data.pointerCurrentRaycast.gameObject.Equals(null))
                {
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
                }
            }

        }
        // If combat is not valid, make the card a card again
        else
        {
            GameObject.FindGameObjectWithTag("Dragging").tag = "Card";
        }
    }

    
    // Basic fireball method. Does damage based on the attack of the spell card played. 
    public void fireball(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();

        if (targetedCard.GetComponent<Card>().currentHealth >= 0)
        {
            DestroyObject(targetedCard);
            Debug.Log("the card has died to a fireball!");
        }
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
            if (GameObject.Find("topMiddle1").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("to Middle1").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle1").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topMiddle2").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topMiddle2").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle2").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

            }
            if (GameObject.Find("topMiddle3").transform.Find("cardSmall") != null)
            {
                GameObject.Find("topMiddle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;
                GameObject.Find("topMiddle3").transform.Find("cardSmall").transform.FindChild("Health").GetComponent<Text>().text = GameObject.Find("top Middle3").transform.Find("cardSmall").GetComponent<Card>().currentHealth.ToString();

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
