using System.Buffers.Text;
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
    //public AudioSource audioSource;
    //public AudioClip openClip;
    //public AudioClip closeClip;
    //public AudioClip lockedClip; 
    [Header("Door Shake")]
    public float shakeDuration = 0.3f;
    public float shakeAmount = 5f;
    public float shakeSpeed = 50f;
    public float elapsed = 0f;

    private bool isOpen = false;
    private bool hasOpened = false;
    private Vector3 initialForward;
    private bool isShaking;
    private Quaternion baseRotation;


    void Start()
    {
        initialForward = transform.forward;
        baseRotation = transform.rotation;
    }

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

        //Player is trying to open a locked door without correct key
        bool requiresKey = requiredKey != KeyType.None;
        bool wrongKey = heldKey != requiredKey && heldKey != KeyType.None;
        bool noKey = heldKey == KeyType.None;

        if (!hasOpened && requiresKey && (wrongKey || noKey))
        {
            Debug.Log("[DoorSystem] Door locked - shake it!");
            //if (lockedClip != null) audioSource.PlayOneShot(lockedClip);
            StartCoroutine(ShakeDoor()); // always allowed before unlock
            return;
        }

        //If door not yet unlocked and player has correct key
        if (!hasOpened)
        {
            hasOpened = true;
            Debug.Log("[DoorSystem] Door unlocked.");

            if (heldItem != null && requiredKey != KeyType.None)
            {
                Debug.Log($"[DoorSystem] Used key: {heldKey}. Destroying key.");
                Destroy(heldItem);
            }
        }

        //toggle open/close
        isOpen = !isOpen;

        Vector3 toPlayer = (player.transform.position - transform.position).normalized;
        float side = Vector3.Dot(transform.right, toPlayer);
        float front = Vector3.Dot(initialForward, toPlayer);

        float angle = 0f;

        if (front > 0)
            angle = (side > 0) ? openAngle : -openAngle;
        else
            angle = (side > 0) ? -openAngle : openAngle;

        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(0, angle, 0) * Quaternion.identity
            : Quaternion.identity;

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

    IEnumerator ShakeDoor()
    {
        Debug.Log("[DoorSystem] Already shaking, skipping new shake.");

        if (isShaking) yield break;

        Debug.Log("[DoorSystem] Starting shake!");

        isShaking = true;
        float baseY = transform.localEulerAngles.y;

        Quaternion originalRotation = baseRotation;

        while (elapsed < shakeDuration)
        {
            float shakeOffset = Mathf.Sin(elapsed * shakeSpeed) * shakeAmount;
            float currentY = baseY + shakeOffset;
            transform.localRotation = Quaternion.Euler(0, currentY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        isShaking = false;
        Debug.Log("[DoorSystem] Shake finished!");
    }
}
