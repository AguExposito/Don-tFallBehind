using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler , IDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Image image;
    private void Start()
    {
        image=transform.GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData) 
    {
        Debug.Log("BeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root.GetChild(transform.root.childCount-1).GetChild(0));//reparents outside grid hierarchy, canvasui is always last sibling
        transform.SetAsLastSibling(); //makes it always visible
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        transform.position = Input.mousePosition; //Makes item follow mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("endDrag");
        transform.SetParent(parentAfterDrag); //Sets new parent
        image.raycastTarget = true;
    }

}
