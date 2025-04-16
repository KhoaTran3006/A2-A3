using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject inventoryPanel;
    public Transform[] uiSlots; // 3D preview container

    private void Awake() => Instance = this;

    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
        // Optionally animate sliding
    }

    public void UpdateSlotUI(int index, GameObject prefab)
    {
        Transform slot = uiSlots[index];
        foreach (Transform child in slot) Destroy(child.gameObject);

        GameObject instance = Instantiate(prefab, slot);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }
}
