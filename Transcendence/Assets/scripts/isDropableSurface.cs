using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class isDropableSurface : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    // Use this for initialization

    public void OnDrop(PointerEventData data)
    {
        isDraggable d = data.pointerDrag.GetComponent<isDraggable>();

        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
	public void OnPointerEnter(PointerEventData data)
    {

    }
    public void OnPointerExit(PointerEventData data)
    {

    }
	
}
