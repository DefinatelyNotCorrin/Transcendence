using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class dropManager : MonoBehaviour {

    // Variables needed for game logic
    public GM gm;
    isDraggable d;
    private dropManager dm;

    // Sorts the objects 
    public void drop(PointerEventData data, GameObject draggingCard, string pointerObjectName)
    {
        Debug.Log("drop was called");
        GameObject pointerObject = GameObject.Find(pointerObjectName);

        d = data.pointerDrag.GetComponent<isDraggable>();

        Field targetField;

        Card actionCard = draggingCard.GetComponent<Card>();

        // Checks to see if the card is a card and the thing below it is a field
        if(pointerObject.CompareTag("Field") && !actionCard.hasBeenPlaced && actionCard.type.Equals("Creature"))
        {
            // Run the logic for a card over a field
            Debug.Log("creatureAndCard called");
            creatureAndField(draggingCard, actionCard, pointerObject);
        }

        // Checks to see if the card is a card and the thing below it is a card
        else if (pointerObject.CompareTag("Card")
            && !actionCard.isExhausted
            && actionCard.hasBeenPlaced
            && !actionCard.ownerTag.Equals(pointerObject.GetComponent<Card>().ownerTag)
            && actionCard.type.Equals("Creature"))
        {
            creatureAndCard(draggingCard, actionCard, pointerObject);
        }

        // 
        else if (pointerObject.CompareTag("Card") 
            && actionCard.type.Equals("Spell"))
        {
            Debug.Log("spellAndCard called");
            spellAndCard(draggingCard, actionCard, pointerObject);
        }
        // If combat is not valid, make the card a card again
        else
        {
            draggingCard.tag = "Card";
        }
    }

    // Logic for a creature card with a field below it
    public void creatureAndField(GameObject draggingCard, Card actionCard, GameObject pointerObject)
    {
        // Runs card placement for player 1
        if (pointerObject.GetComponent<Field>().side.Equals("Player 1")
            && gm.player1.GetComponent<player>().isTurn
            && actionCard.ownerTag.Equals("Player 1")
            && (gm.player1.GetComponent<player>().currentMana - actionCard.currentCost >= 0)
            )
        {
            // Send card to its new field

            d.parentToReturnTo = pointerObject.GetComponent<Field>().transform;

            gm.player1.GetComponent<player>().currentMana = gm.player1.GetComponent<player>().currentMana - actionCard.currentCost;

            GameObject.FindGameObjectWithTag("Player 1 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player1.GetComponent<player>().currentMana.ToString();

            // Set the card tag back to "card" 
            actionCard.hasBeenPlaced = true;
            draggingCard.tag = "Card";
        }
        // Runs card placement for player 2
        else if (pointerObject.GetComponent<Field>().side.Equals("Player 2")
            && gm.player2.GetComponent<player>().isTurn
            && actionCard.ownerTag.Equals("Player 2")
            && (gm.player2.GetComponent<player>().currentMana - actionCard.currentCost >= 0)
            )
        {
            // Send card to its new field

            d.parentToReturnTo = pointerObject.GetComponent<Field>().transform;

            gm.player2.GetComponent<player>().currentMana = gm.player2.GetComponent<player>().currentMana - actionCard.currentCost;

            GameObject.FindGameObjectWithTag("Player 2 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = gm.player2.GetComponent<player>().currentMana.ToString();

            // Set the card tag back to "card" 
            actionCard.hasBeenPlaced = true;
            draggingCard.tag = "Card";
        }
        // If neither is valid, card is set back to a card
        else
        {
            Debug.Log("Invalid move!");
            draggingCard.tag = "Card";
        }
    }

    // Logic for a creature with a card below it
    public void creatureAndCard(GameObject draggingCard, Card actionCard, GameObject pointerObject)
    {
        // Checks to see if it is the player's turn and they own the card
        if (gm.player1.GetComponent<player>().isTurn && actionCard.ownerTag.Equals("Player 1"))
        {
            Debug.Log("Combat conditions met");
            // Runs combat for the cards
            GM.combat(draggingCard, pointerObject);
        }
        // Checks to see if it is the player's turn and they own the card
        else if (gm.player2.GetComponent<player>().isTurn && actionCard.ownerTag.Equals("Player 2"))
        {
            Debug.Log("Combat conditions met");
            // Runs combat for the cards
            GM.combat(draggingCard, pointerObject);
        }
        // If combat is not valid, make the card a card again
        else
        {
            draggingCard.tag = "Card";
        }
    }

    // Logic for a spell with a card below it
    public void spellAndCard(GameObject draggingCard, Card actionCard, GameObject pointerObject)
    {
        // if the spell is owned by the current player, and it does not exceed that player's mana, then
        if (actionCard.ownerTag.Equals(gm.currentPlayer.tag)
            && (gm.currentPlayer.GetComponent<player>().currentMana - actionCard.currentCost >= 0))
        {
            // Subtract mana cost of the spell from the current player's mana
            gm.currentPlayer.GetComponent<player>().currentMana -= actionCard.currentCost;

            if (actionCard.effect.Equals("fireball"))
            {
                Debug.Log("Fireball conditions met");
                fireball(pointerObject, draggingCard);
            }
            else if (actionCard.effect.Equals("heal"))
            {
                Debug.Log("Heal conditions met");
                heal(pointerObject, draggingCard);
            }
            else
            {
                draggingCard.tag = "Card";
            }
        }
        else
        {
            draggingCard.tag = "Card";
        }    
    }

    // Logic for a card with something random below it
    public void spellAndWhatever(GameObject draggingCard, Card actionCard, GameObject pointerObject)
    {

    }

    /*
    * Spell methods to be used for droping interactions
    */

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
