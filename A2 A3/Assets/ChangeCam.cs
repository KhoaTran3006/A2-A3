using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{

    public Camera fpsCam;
    public Camera camView;

    public Animator camAnimator;
    public GameObject cameraObj;

    public bool camOnHand = false;
    public Collider[] invisibleColli;

    public GameObject nightVision;

    void Start()
    {
        // Ensure the correct camera is enabled at start
        fpsCam.enabled = true;
        camView.enabled = false;

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

            camAnimator.SetBool("UseCam", camOnHand);
            //camAnimator.SetBool("CamOff", !camOnHand);
            //camAnimator.SetBool("1stTimeTrigger", true);

            StartCoroutine(ToggleCamera()); // Toggle between FPS camera and Cam Obj

            //ToggleCamera(); 
            ToggleCollider(camOnHand); // Enable or Disable invisible object colliders based on camOnHand
        }
    }

    IEnumerator ToggleCamera()
    {
        //camAnimator.SetBool("UseCam", camOnHand);
        yield return new WaitForSeconds(1.2f);
        fpsCam.enabled = !camOnHand;
        camView.enabled = camOnHand;

        cameraObj.SetActive(!camOnHand);
        nightVision.SetActive(camOnHand);

        camAnimator.SetBool("1stTimeTrigger", true);
        camAnimator.SetBool("CamOff", !camOnHand);
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