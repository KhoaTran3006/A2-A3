using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExplosion : MonoBehaviour
{
    public AudioSource lightExplosion;
    public AudioSource shortCircuit;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            Debug.Log("Yo");
            lightExplosion.Play();
            shortCircuit.Play();
        }
    }
}
