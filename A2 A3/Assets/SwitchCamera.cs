using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    [Header("=========Camera=========")]
    public Camera fpsCam;
    public Camera camView;
    [Header("=====Camera Animation=====")]
    public Animator camAnimator;
    public GameObject cameraObj;
    [Header("======FadeOutEffect======")]

    [Header("==========Bool==========")]
    public bool camOnHand = false;
    public bool equipCam = false;
    [Header("======InvisibleColliders======")]
    public Collider[] invisibleColli;
    [Header("==========Script==========")]
    public FadeInFadeOut fadeOutScript;

    void Start()
    {
        // Ensure the correct camera is enabled at start
        fpsCam.enabled = true;
        camView.enabled = false;

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
            ToggleCollider(camOnHand);  // Disable colliders if camera is not on hand
        }
    }

    void Update()
    {
        // Press E equip Camera first
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (camOnHand)
            {
                return;
            }
            else
            {
                equipCam = !equipCam;
                CameraEquiped();
            }
        }

        // Press C to swtich view
        if (Input.GetKeyDown(KeyCode.C) && equipCam)
        {
            camOnHand = !camOnHand;
            camAnimator.SetBool("UseCam", camOnHand);
            if (!camOnHand) // turning OFF the Spiritual Camera
            {
                if (ExamineSystem.ExamineUIManager.instance.examinableItem != null)
                {
                    ExamineSystem.ExamineUIManager.instance.examinableItem.ForceDropIfExamining();

                }
            }

            StartCoroutine(ToggleCamera());
            // Toggle between FPS camera and Cam Obj
            //ToggleCollider(camOnHand); // Enable or Disable invisible object colliders based on camOnHand
            if (!camOnHand)
            {
                //Trigger fade out effect
                //fadeOutScript.BlackScreenOut();
                StartCoroutine(Fading());
                //Debug.Log("started");
            }
        }
    }
    IEnumerator Fading()
    {
        yield return new WaitForSeconds(0.1f);
        fadeOutScript.BlackScreenOut();
    }

    IEnumerator ToggleCamera()
    {
        //fadeOutScript.BlackScreenOut(); 


        yield return new WaitForSeconds(1.1f); // wait for animation to finish

        //fadeOutScript.BlackScreenOut();
        // Switch view
        fpsCam.enabled = !camOnHand;
        camView.enabled = camOnHand;


        cameraObj.SetActive(!camOnHand);

        camAnimator.SetBool("1stTimeTrigger", true);
        camAnimator.SetBool("CamOff", !camOnHand);


        ToggleCollider(camOnHand);
    }

    void ToggleCollider(bool enable)
    {

        foreach (Collider col in invisibleColli)
        {
            if (col != null)
            {
                col.enabled = enable;

                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = !enable; // freeze physics if collider is off

                Renderer rend = col.GetComponent<Renderer>();
                if (rend != null)
                    rend.enabled = enable;
            }
        }
    }

    public void CameraEquiped()
    {
        camAnimator.SetBool("OnHand", equipCam);
        camAnimator.SetBool("1stTimeTrigger", equipCam);
        camAnimator.SetBool("TakeCamOff", true);
    }
}