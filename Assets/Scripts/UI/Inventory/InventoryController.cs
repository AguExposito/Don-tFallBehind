using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gridSlotPrefab;
    [Space]
    [Header("Inventory Variables")]
    [SerializeField] private int inventorySpace;
    [SerializeField] private int inventorySpaceOccupied;
    [SerializeField] private int equippedSpace;
    [SerializeField] private int equippedSpaceOccupied;
    [Space]
    [Header("Read Only Variables"), ReadOnly]
    [SerializeField] public GameObject gridInventory;
    [SerializeField] private GameObject gridEquipped;
    [SerializeField] private GameObject gridArmor;
    [SerializeField] private GameObject gridMons;
    [SerializeField] private TextMeshProUGUI inventoryTxt;
    [SerializeField] private TextMeshProUGUI equippedTxt;
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.SetActive(false);
        //Get grid references
        gridInventory = transform.Find("GridInventory").gameObject;
        gridEquipped = transform.Find("GridEquipped").gameObject;
        gridArmor = transform.Find("GridArmor").gameObject;
        gridMons = transform.Find("GridMons").gameObject;

        //Get text references
        inventoryTxt = transform.Find("InventoryTxt").GetComponent<TextMeshProUGUI>();
        equippedTxt = transform.Find("EquippedTxt").GetComponent<TextMeshProUGUI>();

        //Gets inventory variables
        inventorySpaceOccupied = gridInventory.transform.childCount;
        equippedSpaceOccupied = gridEquipped.transform.childCount;

        //Sets inventory variables
        SetUpInventoryVariables(4,3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpInventoryVariables(int newInventorySpace, int newEquippedSpace) {
        for (int i = inventorySpace; i < newInventorySpace; i++)
        {
            Instantiate(gridSlotPrefab,gridInventory.transform);
        }
        for (int i = equippedSpace; i < newEquippedSpace; i++)
        {
            Instantiate(gridSlotPrefab,gridEquipped.transform);
        }

        inventorySpace = newInventorySpace;
        equippedSpace = newEquippedSpace;

        equippedTxt.text = "Equipped " + equippedSpaceOccupied + "/" + equippedSpace;
        inventoryTxt.text = "Inventory " + inventorySpaceOccupied + "/" + inventorySpace;

        
    }
}
