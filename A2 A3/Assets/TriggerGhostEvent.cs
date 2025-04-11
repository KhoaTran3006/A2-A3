using System.Collections;
using System.Collections.Generic;
using Hertzole.GoldPlayer;
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
    public GameObject hiddenWall;
    public GoldPlayerController movementScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(ItsComing());
            movementScript.Movement.canRun = true;
        }
    }

    IEnumerator ItsComing()
    {
        hiddenWall.SetActive(true);

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
