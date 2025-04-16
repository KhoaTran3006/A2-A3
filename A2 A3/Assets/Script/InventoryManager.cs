using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // A single inventory slot stores the actual item and its icon
    [System.Serializable]
    public class InventorySlot
    {
        public GameObject item;   // The item GameObject stored in this slot
        public Image icon;        // The 2D UI icon to represent the item
    }

    public InventorySlot[] slots = new InventorySlot[4]; // 4 inventory slots
    public Transform holdPosition;                       // Position to show held item in player's hand
    public LayerMask pickableLayer;                      // Only pick up objects on this layer

    private int selectedIndex = -1;                      // Currently selected slot index
    private GameObject heldObject;                       // The currently held item (shown in hand)

    void Update()
    {
        HandlePickup();        // Handle right-click to pick up
        HandleSlotSwitch();    // Handle number key 1–4 to switch slots
        HandleDropOrUse();     // Handle left-click to use, Z to drop
    }

    // -----------------------------
    // RIGHT-CLICK TO PICK UP ITEMS
    // -----------------------------
    void HandlePickup()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            // Cast ray from center of screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f, pickableLayer))
            {
                // Only pick up objects with Pickable component
                Pickable pickable = hit.collider.GetComponent<Pickable>();
                if (pickable != null)
                {
                    StoreItem(hit.collider.gameObject);
                }
            }
        }
    }

    // -----------------------------
    // NUMBER KEYS 1–4 TO SWITCH SLOT
    // -----------------------------
    void HandleSlotSwitch()
    {
        for (int i = 0; i < 4; i++)
        {
            // If player presses 1, 2, 3, or 4
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SelectSlot(i);
                break;
            }
        }
    }

    // -----------------------------
    // Z TO DROP | LEFT-CLICK TO USE
    // -----------------------------
    void HandleDropOrUse()
    {
        if (Input.GetKeyDown(KeyCode.Z) && heldObject != null)
        {
            DropItem(); // Drop the currently held item
        }
        else if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Debug.Log("[Inventory] Used item: " + heldObject.name);
            // Optional: trigger item's ability here
        }
    }

    // -----------------------------
    // STORE OBJECT INTO INVENTORY
    // -----------------------------
    void StoreItem(GameObject obj)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // Find first empty slot
            {
                slots[i].item = obj; // Store the item
                slots[i].icon.enabled = true;
                slots[i].icon.sprite = obj.GetComponent<SpriteRenderer>()?.sprite; // Use object's sprite as icon

                obj.SetActive(false); // Hide the object in the world

                Debug.Log($"[Inventory] Stored {obj.name} into slot {i + 1}");
                return;
            }
        }

        Debug.Log("[Inventory] No empty slot available.");
    }

    // -----------------------------
    // SELECT A SLOT BY INDEX
    // -----------------------------
    void SelectSlot(int index)
    {
        Debug.Log($"[Inventory] Switched to slot {index + 1}");

        // Fade out all icons to indicate not selected
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].icon.color = new Color(1, 1, 1, 0.4f); // Semi-transparent
        }

        // If the selected slot has an item
        if (slots[index].item != null)
        {
            // Hide currently held object if there is one
            if (heldObject != null)
                heldObject.SetActive(false);

            heldObject = slots[index].item; // Update new held item
            heldObject.SetActive(true);     // Show it again
            heldObject.transform.SetParent(holdPosition); // Parent to player's hand
            heldObject.transform.localPosition = Vector3.zero; // Snap to position
            heldObject.transform.localRotation = Quaternion.identity;

            slots[index].icon.color = Color.white; // Highlight selected slot
        }
        else
        {
            // If empty slot selected, hide current held item
            if (heldObject != null)
                heldObject.SetActive(false);
            heldObject = null;
        }

        selectedIndex = index;
    }

    // -----------------------------
    // DROP THE CURRENTLY HELD ITEM
    // -----------------------------
    void DropItem()
    {
        heldObject.transform.SetParent(null); // Unparent from hold position
        heldObject.transform.position = holdPosition.position + transform.forward; // Drop in front of player
        heldObject.SetActive(true); // Show in world

        Debug.Log($"[Inventory] Dropped item: {heldObject.name}");

        // Clear the slot
        slots[selectedIndex].item = null;
        slots[selectedIndex].icon.sprite = null;
        slots[selectedIndex].icon.enabled = false;

        heldObject = null;
    }

    // -----------------------------
    // GET CURRENT HELD OBJECT
    // Called by DoorSystem.cs
    // -----------------------------
    public GameObject GetHeldItem()
    {
        return heldObject;
    }
}
