using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGhostEvent : MonoBehaviour
{
    public MovingGhost ghostScript;
    public Animator doorAnim;
    public Light light1, light2;
    public CameraShaker shake;
    public AudioSource sound;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ItsComing());
        }
    }

    IEnumerator ItsComing()
    {
        sound.Play();
        doorAnim.SetBool("Oops", true);
        shake.Shake();
        yield return new WaitForSeconds(0.5f);

        light1.enabled = false;
        light2.enabled = false;
        yield return new WaitForSeconds(0.1f);

        ghostScript.isMoving = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
