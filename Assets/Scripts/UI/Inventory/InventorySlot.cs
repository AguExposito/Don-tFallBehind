using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{    
    [SerializeField] public enum GridType { Equippable, Inventory, Mon, Armor };
    [SerializeField] public GridType gridType;
    private void Start()
    {
        switch (transform.parent.name) {
            case "GridEquipped": { gridType = GridType.Equippable; }break;
            case "GridInventory": { gridType = GridType.Inventory; } break;
            case "GridArmor": { gridType = GridType.Armor; } break;
            case "GridMons": { gridType = GridType.Mon; } break;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {        
        if (transform.childCount == 0)
        {            
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            InventoryItemController inventoryItem = dropped.GetComponent<InventoryItemController>();
            
            //Checks if item can be placed on selected slot
            if (CanBeReparented(inventoryItem)) {
                draggableItem.parentAfterDrag = transform;
            }

            //if on equippable slot instantiate gameobject
            if (gridType == GridType.Equippable)
            {

            }
        }
    }
    private bool CanBeReparented(InventoryItemController dgi) {
        switch (dgi.itemType) {
            case InventoryItemController.ItemType.Weapon: {
                    if (gridType == GridType.Equippable || gridType == GridType.Inventory) 
                    { return true; }
                    else { return false; }
                }
            case InventoryItemController.ItemType.Material: {
                    if (gridType == GridType.Equippable || gridType == GridType.Inventory) 
                    { return true; }
                    else { return false; }
                }
            case InventoryItemController.ItemType.Armor: {
                    if (gridType == GridType.Armor || gridType == GridType.Inventory) 
                    { return true; }
                    else { return false; }
                } 
            case InventoryItemController.ItemType.Mon: {
                    if (gridType == GridType.Mon || gridType == GridType.Inventory) 
                    { return true; }
                    else { return false; }
                } 
            default: return false;
        }
    }
}
