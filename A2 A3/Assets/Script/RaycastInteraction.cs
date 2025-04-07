using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public GameObject onhandPos;
    public GameObject inspectionUI;                 // UI Panel with black semi-transparent background
    public MonoBehaviour playerMovementScript;      // Your movement script to disable during inspection
    //public AudioSource lockedDoorAudio;             // AudioSource for locked doors
    public Transform inspectDisplayPoint;           // Empty object in front of the camera
    public float rotationSpeed = 100f;
    public Camera cam;
    private GameObject currentInspectedObject;
    private bool isInspecting = false;

    void Update()
    {
        if (isInspecting)
        {
            RotateInspectedObject();

            if (Input.GetKeyDown(KeyCode.Escape))
                ExitInspection();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            {
                //Debug.DrawRay(transform.position, Vector3.forward);
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log(hitObject.name);

                switch (hitObject.tag)
                {
                    case "Interactable" or "InvisibleObject":
                        EnterInspectionMode(hitObject);
                        GameObject onhandObj= hitObject;
                        onhandObj.transform.position = onhandPos.transform.position;
                        break;

                    case "Open Door":
                        //Animator anim = hitObject.GetComponent<Animator>();
                        //if (anim != null) anim.SetTrigger("Open");
                        Debug.Log("Open Door");
                        break;

                    case "Locked Doors":
                        //if (lockedDoorAudio != null) lockedDoorAudio.Play();
                        Debug.Log("Locked");
                        break;
                }
            }
        }
    }

    void EnterInspectionMode(GameObject obj)
    {
        if (!obj.TryGetComponent<InteractableItem>(out InteractableItem item) || item.inspectionPrefab == null)
        {
            Debug.LogWarning("InteractableItem missing or prefab not assigned!");
            return;
        }

        isInspecting = true;
        inspectionUI.SetActive(true);
        playerMovementScript.enabled = false;

        currentInspectedObject = Instantiate(item.inspectionPrefab, inspectDisplayPoint.position, Quaternion.identity);
        currentInspectedObject.transform.SetParent(inspectDisplayPoint);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitInspection()
    {
        isInspecting = false;
        inspectionUI.SetActive(false);
        playerMovementScript.enabled = true;

        if (currentInspectedObject != null)
            Destroy(currentInspectedObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void RotateInspectedObject()
    {
        if (currentInspectedObject == null) return;

        float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        currentInspectedObject.transform.Rotate(cam.transform.up, rotX, Space.World);
        currentInspectedObject.transform.Rotate(cam.transform.right, rotY, Space.World);
    }
}
