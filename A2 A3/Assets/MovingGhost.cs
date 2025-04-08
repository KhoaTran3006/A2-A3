using System.Collections;
using System.Collections.Generic;
using Hertzole.GoldPlayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingGhost : MonoBehaviour
{
    public float stepDistance = 5f;         // Distance per step
    public float stepDuration = 0.5f;         // Time between steps
    public Vector3 moveDirection = Vector3.right; // Direction to move in

    public bool isMoving = false;
    private bool hasStartedMoving = false;

    public GoldPlayerController movementScript;

    void Update()
    {
        if (isMoving && !hasStartedMoving)
        {
            hasStartedMoving = true;
            StartCoroutine(MoveStepByStep());
        }
    }

    IEnumerator MoveStepByStep()
    {
        while (isMoving)
        {
            Vector3 targetPosition = transform.position + moveDirection.normalized * stepDistance;

            // Smooth movement over 0.2 seconds
            float elapsed = 0f;
            float duration = 0.2f;
            Vector3 startPos = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            yield return new WaitForSeconds(stepDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LightZone"))
        {
            Light lightComponent = other.GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = false;
                //Debug.Log("Touched");
            }
        }
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CatchYa());
            //isMoving = false;
        }
    }

    IEnumerator CatchYa()
    {
        movementScript.enabled = false;
        yield return new WaitForSeconds(2);
        isMoving = false;
        SceneManager.LoadScene("Scene 2");
    }
}

