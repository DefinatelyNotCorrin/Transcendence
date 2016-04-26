using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Timers;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public bool isDragging = false;
    private dropManager dm;
    private double CARD_HOVER_TIME = 2.0; // seconds?
    private double startTime;
    private double currentTime;
    private double triggerTime;
    private bool checkTime = false;
    public bool displayZoomedView = false;
    private GM gm;

    //original location of a card before it is dragged
    public Transform parentToReturnTo = null;
    //God cares not for As, he cares only for your faith

    public void Start()
    {
        gm = GameObject.Find("GM").GetComponent<GM>();
    }

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged()
    {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		//Debug.Log ("on begin drag");
        isDragging = true;
        this.checkTime = false;

        if (GameObject.Find("Card Hover") != null)
        {
            Destroy(GameObject.Find("Card Hover"));
        }

        if (this.displayZoomedView)
        {
            this.displayZoomedView = false;
        }

        //saves the current parent of the object
        parentToReturnTo = this.transform.parent;
        //sets the objects parent up one level (resetting the grid)
        if (this.transform.parent.CompareTag("Hand"))
        {
            this.transform.SetParent(this.transform.parent.parent);
        }

        if (this.transform.parent.CompareTag("Field"))
        {
            this.transform.SetParent(this.transform.parent.parent.parent);
        }

        this.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().raycastTarget = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.GetComponent<Image>().raycastTarget = false;        
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Debug.Log("The timer even occured!");
    }

    public void OnDrag(PointerEventData data) {
		//Debug.Log ("dragging");

        //moves the card to the cursor position
        this.transform.position = data.position;

        this.gameObject.tag = "Dragging";
    }

	public void OnEndDrag(PointerEventData data) {
		//Debug.Log ("on end drag");
        isDragging = false;

        dm = GameObject.Find("GM").GetComponent<dropManager>();

        GameObject draggingCard = GameObject.FindGameObjectWithTag("Dragging");
        GameObject pointerObject = data.pointerCurrentRaycast.gameObject;

        //dm.Drop(data, draggingCard, pointerObject.transform);
        
        try
        {
            dm.Drop(data, draggingCard, pointerObject.transform);
        }
        catch
        {
            Debug.Log("Nice try, JOHNATHAN!");
        } 

        this.transform.SetParent(parentToReturnTo);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //this.checkTime = true;
        //this.startTime = Time.time;
        //this.currentTime = Time.time;
        //this.triggerTime = startTime + CARD_HOVER_TIME;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //this.checkTime = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.displayZoomedView)
        {
            this.displayZoomedView = false;
            gm.DisplayDiscription(this.transform.gameObject, false);
            Debug.Log("Display closed for " + this.transform.gameObject.name);
        }
        else
        {
            this.displayZoomedView = true;
            gm.DisplayDiscription(this.transform.gameObject, true);
            Debug.Log("Display open for " + this.transform.gameObject.name);
        }
    }
}
