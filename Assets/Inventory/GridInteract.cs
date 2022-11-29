using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script marking Grid is being interactable
/// If deactivated can't receive Player input
/// Script responsible for assigning selected Grid
/// - if we hover selected Grid - it's assigng as Selected
/// </summary>
[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    ItemGrid itemGrid;
    RectTransform rectTransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //inventoryController.selectedItemGrid = itemGrid;
        inventoryController.SelectedItemGrid = itemGrid;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.SetAsFirstSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    
}
