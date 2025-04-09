using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;         // Max distance to pick up an object
    public Transform onHandPos;            // Empty GameObject under the camera where items are held
    public float throwForce;

    private GameObject heldObject;         // Reference to the currently held object
    private Camera cam;                    // Reference to the main camera

    void Start()
    {
        cam = Camera.main;                 // Get the main camera at the start
    }

    void Update()
    {
        if (heldObject == null)
        {
            // Try to pick up when left mouse is clicked
            if (Input.GetMouseButtonDown(0))
            {
                TryPickup();
            }
        }
        else
        {
            // Drop held item with right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                DropItem();
            }
        }
    }

    void TryPickup()
    {
        // Cast a ray from the center of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        // Raycast to detect objects within pickup range
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            // Check if the object has a Pickable component
            Pickable pickable = hit.collider.GetComponent<Pickable>();
            if (pickable != null)
            {
                PickupItem(hit.collider.gameObject);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        heldObject = item;

        // Disable physics
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Attach object to onHandPos
        heldObject.transform.SetParent(onHandPos);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    void DropItem()
    {
        // Re-enable physics
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            
            rb.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
        }

        // Detach from the camera's hand position
        heldObject.transform.SetParent(null);
        heldObject = null;
    }
}
