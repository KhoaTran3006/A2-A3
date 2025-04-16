using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] slotImages = new Image[4]; // For 2D icons
    public GameObject[] storedItems = new GameObject[4];
    public Transform handPos;
    
    public GameObject[] heldVisualPrefabs = new GameObject[4]; // For visual instantiation

    private int currentSelectedIndex = -1;

    void Update()
    {
        HandleSlotSelection();
        if (Input.GetKeyDown(KeyCode.Tab)) UIManager.Instance.ToggleInventory();
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            {
                StoredItem stored = item.GetComponent<StoredItem>();
                if (stored != null)
                {
                    storedItems[i] = item; // keep logic (DoorKey, etc.)
                    heldVisualPrefabs[i] = stored.heldPrefab; // used to show in hand

                    item.SetActive(false); // optional: hide world version

                    if (stored.itemIcon != null)
                    {
                        slotImages[i].sprite = stored.itemIcon;
                        slotImages[i].color = Color.white;
                        slotImages[i].enabled = true;
                    }

                    return true;
                }
            }
        }

        return false;
    }

    void HandleSlotSelection()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                EquipItem(i);
            }
        }
    }

    void EquipItem(int index)
    {
        if (storedItems[index] == null || heldVisualPrefabs[index] == null) return;

        PickupHandler pickup = FindObjectOfType<PickupHandler>();
        if (pickup != null)
        {
            pickup.Equip(heldVisualPrefabs[index]); // visual only
            currentSelectedIndex = index;
            HighlightSlot(index);
        }
    }

    void HighlightSlot(int index)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == index) ? Color.white : new Color(0, 0, 0, 0.5f);
        }
    }

    public GameObject GetCurrentEquippedItem()
    {
        return currentSelectedIndex != -1 ? storedItems[currentSelectedIndex] : null;
    }
}
