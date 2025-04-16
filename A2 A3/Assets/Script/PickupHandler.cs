using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private GameObject heldObject;
    [SerializeField] private Transform holdPos;

    public GameObject HeldObject => heldObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to pick up
        {
            TryPickupItem();
        }
    }
    public void Equip(GameObject item)
    {
        if (heldObject != null)
            Destroy(heldObject);

        heldObject = Instantiate(item, holdPos); // assign new item
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
        heldObject.transform.localScale = Vector3.one * 0.1f;

        if (heldObject.TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;
        if (heldObject.TryGetComponent<Collider>(out var col))
            col.enabled = false;
        Debug.Log($"[PickupHandler] Instantiating held object: {item.name}");
    }

    void TryPickupItem()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            StoredItem storedItem = hit.collider.GetComponent<StoredItem>();
            if (storedItem != null)
            {
                Debug.Log($"[Pickup] Hit: {storedItem.name}");

                //Add this block right here:
                InventoryManager inventory = FindObjectOfType<InventoryManager>();
                if (inventory != null && inventory.AddItem(hit.collider.gameObject))
                {
                    Debug.Log("[Pickup] Added to inventory!");
                    hit.collider.gameObject.SetActive(false); // hide original
                }
                else
                {
                    Debug.Log("[Pickup] Inventory full or manager not found.");
                }
            }
            else
            {
                Debug.Log("[Pickup] No StoredItem on object.");
            }
        }
        else
        {
            Debug.Log("[Pickup] Nothing hit.");
        }
    }
    
}
