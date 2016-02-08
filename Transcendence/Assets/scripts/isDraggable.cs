using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class IsDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool isDragging = false;
    private DropManager dm;

    //original location of a card before it is dragged
    public Transform parentToReturnTo = null;

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged() {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		//Debug.Log ("on begin drag");
        isDragging = true;

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

        
        GetComponent<CanvasGroup>().blocksRaycasts = false;       
        
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

        dm = GameObject.Find("GM").GetComponent<DropManager>();

        GameObject draggingCard = GameObject.FindGameObjectWithTag("Dragging");
        GameObject pointerObject = data.pointerCurrentRaycast.gameObject;

        dm.drop(data, draggingCard, pointerObject.name);
        //
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
