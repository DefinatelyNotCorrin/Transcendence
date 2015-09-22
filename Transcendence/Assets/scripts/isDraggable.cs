using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool isDragging = false;

    public Transform parentToReturnTo = null;

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged() {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		Debug.Log ("on begin drag");
        isDragging = true;

        parentToReturnTo = this.transform.parent;
	}

	public void OnDrag(PointerEventData data) {
		Debug.Log ("dragging");
	}

	public void OnEndDrag(PointerEventData data) {
		Debug.Log ("on end drag");
        isDragging = false;
	}
}
