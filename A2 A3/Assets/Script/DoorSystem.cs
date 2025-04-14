using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public KeyType requiredKey = KeyType.None; 
    public Transform hinge; // The hinge or pivot point of the door
    public float openAngle = 90f;
    public float openSpeed = 3f;
    public float interactDistance = 3f;

    private bool isOpen = false;
    private bool hasOpened = false;

    void OnMouseOver()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (Vector3.Distance(player.transform.position, transform.position) > interactDistance)
            return; // Too far away, don't do anything

        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            HandleDoorToggle();
        }
    }

    void TryOpen()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ItemPickup pickup = player.GetComponent<ItemPickup>();

        GameObject heldItem = GetHeldItem(pickup);
        KeyType heldKey = GetHeldKeyType(heldItem);

        if (requiredKey == KeyType.None || heldKey == requiredKey)
        {
            if (!hasOpened)
            {
                OpenDoor(player.transform.position);

                if (heldItem != null && requiredKey != KeyType.None)
                {
                    Debug.Log($"[DoorSystem] Used key: {heldKey}. Destroying key.");
                    Destroy(heldItem);
                }

                hasOpened = true;
            }
        }
        else
        {
            Debug.Log($"[DoorSystem] This door requires: {requiredKey}, but player has: {heldKey}");
        }
    }
    void ToggleDoor()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;

        // Flip open state
        isOpen = !isOpen;

        Vector3 toPlayer = playerPos - transform.position;
        float dot = Vector3.Dot(transform.right, toPlayer);
        float angle = (dot > 0) ? -openAngle : openAngle;

        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(0, angle, 0) * transform.rotation
            : Quaternion.identity;

        Debug.Log("[DoorSystem] Door " + (isOpen ? "opened" : "closed"));
        StopAllCoroutines();
        StartCoroutine(RotateDoor(targetRotation));
    }

    void HandleDoorToggle()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ItemPickup pickup = player.GetComponent<ItemPickup>();
        GameObject heldItem = GetHeldItem(pickup);
        KeyType heldKey = GetHeldKeyType(heldItem);

        // If not yet unlocked, check for key
        if (!hasOpened)
        {
            if (requiredKey == KeyType.None || heldKey == requiredKey)
            {
                hasOpened = true;
                Debug.Log("[DoorSystem] Door unlocked.");

                if (heldItem != null && requiredKey != KeyType.None)
                {
                    Debug.Log($"[DoorSystem] Used key: {heldKey}. Destroying key.");
                    Destroy(heldItem);
                }
            }
            else
            {
                Debug.Log($"[DoorSystem] This door requires: {requiredKey}, but player has: {heldKey}");
                return; // Don't toggle if locked
            }
        }

        // Now toggle door open/close
        isOpen = !isOpen;

        Vector3 toPlayer = player.transform.position - transform.position;
        float dot = Vector3.Dot(transform.right, toPlayer);
        float angle = (dot > 0) ? -openAngle : openAngle;

        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(0, angle, 0) * transform.rotation
            : Quaternion.identity;

        Debug.Log("[DoorSystem] Door " + (isOpen ? "opened" : "closed"));
        StopAllCoroutines();
        StartCoroutine(RotateDoor(targetRotation));
    }

    KeyType GetHeldKeyType(GameObject item)
    {
        if (item == null) return KeyType.None;

        DoorKey key = item.GetComponent<DoorKey>();
        return key != null ? key.keyType : KeyType.None;
    }

    void OpenDoor(Vector3 playerPos)
    {
        // Determine which side the player is on
        Vector3 toPlayer = playerPos - transform.position;
        float dot = Vector3.Dot(transform.right, toPlayer);

        float angle = (dot > 0) ? -openAngle : openAngle;

        Quaternion targetRotation = Quaternion.Euler(0, angle, 0) * transform.rotation;
        StartCoroutine(RotateDoor(targetRotation));

        isOpen = true;
        Debug.Log("[DoorSystem] Door opened to angle: " + angle);
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    GameObject GetHeldItem(ItemPickup pickup)
    {
        var held = pickup.GetType()
                         .GetField("heldObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                         ?.GetValue(pickup) as GameObject;

        Debug.Log($"[DoorSystem] Held item: {(held != null ? held.name : "None")}");
        return held;
    }

    bool HasCorrectKey(GameObject item)
    {
        DoorKey key = item.GetComponent<DoorKey>();
        return key != null && key.keyType == requiredKey;
    }
}
