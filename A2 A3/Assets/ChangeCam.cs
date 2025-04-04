using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    public Camera fpsCam;
    public Camera camObj;

    public bool camOnHand = false;
    public Collider[] invisibleColli;

    public GameObject nightVision;

    void Start()
    {
        // Ensure the correct camera is enabled at start
        fpsCam.enabled = true;
        camObj.enabled = false;

        nightVision.SetActive(false);

        // Find all objects tagged as "InvisibleObject" and store their colliders
        GameObject[] invisibleObjs = GameObject.FindGameObjectsWithTag("InvisibleObject");

        // If no invisible objects are found in the current scene, skip the collider setup
        if (invisibleObjs == null || invisibleObjs.Length == 0)
        {
            return;
        }
        else
        {
            // If there are invisible objects, start the collider array
            invisibleColli = new Collider[invisibleObjs.Length];
        }

        // For each invisible object found, get its collider and store it in the array
        for (int i = 0; i < invisibleObjs.Length; i++)
        {
            invisibleColli[i] = invisibleObjs[i].GetComponent<Collider>();
        }

        if (camOnHand == false)
        {
            ToggleCollider(camOnHand);  // Disable colliders if camera is not in hand mode
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            camOnHand = !camOnHand;
            ToggleCamera(); // Toggle between FPS camera and Cam Obj
            ToggleCollider(camOnHand); // Enable or Disable invisible object colliders based on camOnHand
        }
    }

    void ToggleCamera()
    {
        fpsCam.enabled = !camOnHand;
        camObj.enabled = camOnHand;
        nightVision.SetActive(camOnHand);
    }

    void ToggleCollider(bool enable)
    {
        foreach (Collider col in invisibleColli)
        {
            if (col != null)
                col.enabled = enable;
        }
    }
}