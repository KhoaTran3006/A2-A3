using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetInteraction : MonoBehaviour
{
    public ParticleSystem waterParticle;     // Assign your water Particle System in inspector
    public float interactDistance = 3f;      // How close the player must be
    private Camera cam;
    private bool isOn = false;

    void Start()
    {
        cam = Camera.main;
        if (waterParticle != null)
            waterParticle.Stop();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right-click
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Check if we're hitting THIS faucet
                if (hit.collider.gameObject == this.gameObject)
                {
                    ToggleFaucet();
                }
            }
        }
    }

    void ToggleFaucet()
    {
        if (waterParticle == null) return;

        isOn = !isOn;

        if (isOn)
            waterParticle.Play();
        else
            waterParticle.Stop();

        // Optional: Debug
        Debug.Log("Faucet turned " + (isOn ? "on" : "off"));
    }
}
