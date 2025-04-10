using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGhostEvent : MonoBehaviour
{
    public MovingGhost ghostScript;
    public Animator doorAnim;
    public Light light1, light2;
    public CameraShaker shake;
    public AudioSource ghostSound;
    public AudioSource doorSlam;
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(ItsComing());
        }
    }

    IEnumerator ItsComing()
    {
        ghostSound.Play();
        doorSlam.Play();

        yield return new WaitForSeconds(0.5f);
        doorAnim.SetBool("Oops", true);
        shake.Shake();

        light1.enabled = false;
        light2.enabled = false;

        yield return new WaitForSeconds(0.1f);
        ghostScript.isMoving = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
