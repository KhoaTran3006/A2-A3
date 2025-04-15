using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    public float interactRange = 3f;

    private Interactable currentLookTarget;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                // Only update if new target
                if (currentLookTarget != interactable)
                {
                    if (currentLookTarget != null)
                        currentLookTarget.HideUI();

                    interactable.ShowUI();
                    currentLookTarget = interactable;
                }
                return; //STOP here if we're looking at something valid
            }
        }

        //If raycast missed, or hit something without Interactable
        if (currentLookTarget != null)
        {
            currentLookTarget.HideUI();
            currentLookTarget = null;
        }
    }
}
