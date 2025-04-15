using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            FruitItem fruit = other.GetComponent<FruitItem>();
            if (fruit != null && !fruit.isClean)
            {
                fruit.Clean();
                Debug.Log("Fruit cleaned in water: " + fruit.name);
            }
        }
    }
}
