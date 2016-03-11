using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class dropManager : MonoBehaviour {

    // Variables needed for game logic
    private GM GM;
    private isDraggable D;
    private EffectManager EF;

    public void Start()
    {
        this.GM = GameObject.Find("GM").GetComponent<GM>();
        this.EF = GameObject.Find("Effect Manager").GetComponent<EffectManager>();
    }

    // Sorts the objects 
    public void drop(PointerEventData data, GameObject draggingCardObject, Transform pointerObjectName)
    {
        GameObject pointerObject = pointerObjectName.gameObject;
        Card draggingCardScript = draggingCardObject.GetComponent<Card>();

        D = data.pointerDrag.GetComponent<isDraggable>();

        Debug.Log("Dragging card is a " + draggingCardObject.GetComponent<Card>().Type);

        // Checks to see if the card is a card and the thing below it is a field
        if (pointerObject.CompareTag("Field") && !draggingCardScript.HasBeenPlaced && draggingCardScript.Type.Equals("Creature"))
        {
            // Run the logic for a card over a field
            Debug.Log("Creature and Field conditions met.");
            creatureAndField(draggingCardObject, draggingCardScript, pointerObject);
        }

        // Checks to see if the card is a card and the thing below it is a card
        else if (pointerObject.CompareTag("Card")
            && draggingCardScript.Type.Equals("Creature")
            && !draggingCardScript.IsExhausted
            && draggingCardScript.HasBeenPlaced
            && draggingCardScript.OwnerTag.Equals(GM.currentPlayer.tag)
            && !draggingCardScript.OwnerTag.Equals(pointerObject.GetComponent<Card>().OwnerTag))
        {
            Debug.Log("Placed Card and Enemy Card conditions met.");
            creatureAndCard(draggingCardObject, draggingCardScript, pointerObject);
        }

        // 
        else if (pointerObject.CompareTag("Card")
            && draggingCardScript.Type.Equals("Spell")
            && draggingCardScript.TargetName.Equals("Single"))
        {
            Debug.Log("Spell and Card conditions met.");
            spellAndCard(draggingCardObject, draggingCardScript, pointerObject);
        }
        // If combat is not valid, make the card a card again
        else
        {
            Debug.Log("Invalid move!");
            draggingCardObject.tag = "Card";
        }
    }

    /// <summary>
    /// Creature and field logics. 
    /// </summary>
    /// <param name="draggingCardGameObject"></param> GameObject of the card being dragged.
    /// <param name="draggingCardScript"></param> Card script of the card being dragged. 
    /// <param name="pointerObject"></param>
    public void creatureAndField(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // Runs card placement for player 1
        if (pointerObject.GetComponent<Field>().side.Equals("Player 1")
            && GM.player1.GetComponent<player>().IsTurn
            && draggingCardScript.OwnerTag.Equals("Player 1")
            && (GM.player1.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost >= 0)
            )
        {
            // Send card to its new field
            D.parentToReturnTo = pointerObject.GetComponent<Field>().transform;

            GM.player1.GetComponent<player>().CurrentMana = GM.player1.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost;

            GameObject.FindGameObjectWithTag("Player 1 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = GM.player1.GetComponent<player>().CurrentMana.ToString();

            // Set the card tag back to "card" 
            draggingCardScript.HasBeenPlaced = true;
            draggingCardGameObject.tag = "Card";
            //draggingCardScript.BattleSide = 0;
            draggingCardGameObject.transform.FindChild("Outline").GetComponent<Image>().sprite = GM.clear;
        }
        // Runs card placement for player 2
        else if (pointerObject.GetComponent<Field>().side.Equals("Player 2")
            && GM.player2.GetComponent<player>().IsTurn
            && draggingCardScript.OwnerTag.Equals("Player 2")
            && (GM.player2.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost >= 0)
            )
        {
            // Send card to its new field
            D.parentToReturnTo = pointerObject.GetComponent<Field>().transform;

            GM.player2.GetComponent<player>().CurrentMana = GM.player2.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost;

            GameObject.FindGameObjectWithTag("Player 2 Mana").gameObject.transform.FindChild("Text").GetComponent<Text>().text = GM.player2.GetComponent<player>().CurrentMana.ToString();

            // Set the card tag back to "card" 
            draggingCardScript.HasBeenPlaced = true;
            draggingCardGameObject.tag = "Card";
            //draggingCardScript.BattleSide = 1;
            draggingCardGameObject.transform.FindChild("Outline").GetComponent<Image>().sprite = GM.clear;
        }
        // If neither is valid, card is set back to a card
        else
        {
            Debug.Log("Invalid move!");
            draggingCardGameObject.tag = "Card";
        }
    }

    // Logic for a creature with a card below it
    public void creatureAndCard(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // Checks to see if it is the player's turn and they own the card
        if (GM.player1.GetComponent<player>().IsTurn && draggingCardScript.OwnerTag.Equals("Player 1"))
        {
            Debug.Log("Combat conditions met");
            // Runs combat for the cards
            GM.Combat(draggingCardGameObject, pointerObject);
        }
        // Checks to see if it is the player's turn and they own the card
        else if (GM.player2.GetComponent<player>().IsTurn && draggingCardScript.OwnerTag.Equals("Player 2"))
        {
            Debug.Log("Combat conditions met");
            // Runs combat for the cards
            GM.Combat(draggingCardGameObject, pointerObject);
        }
        // If combat is not valid, make the card a card again
        else
        {
            draggingCardGameObject.tag = "Card";
        }
    }

    // Logic for a spell with a card below it
    public void spellAndCard(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
        // if the spell is owned by the current player, and it does not exceed that player's mana, then
        //actionCard.ownerTag.Equals(gm.currentPlayer.tag)
        if ((GM.currentPlayer.GetComponent<player>().CurrentMana - draggingCardScript.CurrentCost >= 0))
        {
            // Subtract mana cost of the spell from the current player's mana
            GM.currentPlayer.GetComponent<player>().CurrentMana -= draggingCardScript.CurrentCost;

            EF.RunEffect(draggingCardScript.Effect, pointerObject, draggingCardGameObject);

            if (draggingCardScript.Effect == Effect.None)
            {
                draggingCardGameObject.tag = "Card";
            }
        }
        else
        {
            Debug.Log("No spell conditions were met");
            draggingCardGameObject.tag = "Card";
        }    
    }

    // Logic for a card with something random below it
    public void spellAndWhatever(GameObject draggingCardGameObject, Card draggingCardScript, GameObject pointerObject)
    {
    }

    /// <remarks>
    /// The following are spell method that are deprecated. Dont use!
    /// </remarks>

    /*
    // Basic fireball method. Does damage based on the attack of the spell card played. 
    public void fireball(GameObject targetedCard, GameObject spellCard)
    {
        targetedCard.GetComponent<Card>().currentHealth = targetedCard.GetComponent<Card>().currentHealth - spellCard.GetComponent<Card>().currentAttack;

        DestroyObject(spellCard);

        targetedCard.transform.FindChild("Health").GetComponent<Text>().text = targetedCard.GetComponent<Card>().currentHealth.ToString();

        if (targetedCard.GetComponent<Card>().currentHealth <= 0)
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
                GM.drawCard(GameObject.Find("Player1"));
            }
        }

        if (spellCard.GetComponent<Card>().battleSide == 1)
        {
            for (int i = 0; i < spellCard.GetComponent<Card>().currentCost; i++)
            {
                GM.drawCard(GameObject.Find("Player2"));
            }
        }
    }
    */
}
