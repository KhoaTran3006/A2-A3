using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffLight : MonoBehaviour
{
    public Light coneLight;

    public void Start()
    {
        Debug.Log("Evrything goood");

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            coneLight.enabled = false;
            Debug.Log("Touched");
        }
    }
}
