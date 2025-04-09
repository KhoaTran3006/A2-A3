using System.Collections;
using System.Collections.Generic;
using Hertzole.GoldPlayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class MovingGhost : MonoBehaviour
{
    public float stepDistance = 5f;         // Distance per step
    public float stepDuration = 0.5f;         // Time between steps
    public Vector3 moveDirection = Vector3.right; // Direction to move in

    public bool isMoving = false;
    private bool hasStartedMoving = false;

    public PostProcessVolume postProcessVolume;
    private Vignette fpsCamVignette;

    public GoldPlayerController movementScript;

    void Start()
    {
        //postProcessVolume.profile = Instantiate(postProcessVolume.profile);
        postProcessVolume.profile.TryGetSettings(out fpsCamVignette);
        fpsCamVignette.intensity.value = 0f;

    }
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
            StartCoroutine(DrainSpeed());
            //isMoving = false;

            StartCoroutine(IncreaseVignette());
        }
    }

    // Decrease mouse sentivity when touched by ghost
    IEnumerator CatchYa()
    {
        float duration = 5f;
        float elapsed = 0f;

        Vector2 originalSensitivity = movementScript.Camera.lookSensitivity;
        Vector2 targetSensitivity = Vector2.zero; // or a very low value like new Vector2(0.1f, 0.1f)

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            movementScript.Camera.lookSensitivity = Vector2.Lerp(originalSensitivity, targetSensitivity, elapsed / duration);
            yield return null;
        }
        isMoving = false;
        //SceneManager.LoadScene("Scene 2"); WAIT FOR ANY CHANGES
    }

    // Decrease player walk/run speed when touched by ghost
    IEnumerator DrainSpeed()
    {
        float duration = 3f;
        float elapsed = 0f;

        // Save the original movement speeds
        MovementSpeeds originalWalk = movementScript.Movement.walkingSpeeds;
        MovementSpeeds originalRun = movementScript.Movement.runSpeeds;

        // Define how slow you want to go (0 = full stop)
        MovementSpeeds target = new MovementSpeeds(0f, 0f, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            movementScript.Movement.walkingSpeeds = new MovementSpeeds(
                Mathf.Lerp(originalWalk.forwardSpeed, target.forwardSpeed, t),
                Mathf.Lerp(originalWalk.sidewaysSpeed, target.sidewaysSpeed, t),
                Mathf.Lerp(originalWalk.backwardsSpeed, target.backwardsSpeed, t)
            );

            movementScript.Movement.runSpeeds = new MovementSpeeds(
                Mathf.Lerp(originalRun.forwardSpeed, target.forwardSpeed, t),
                Mathf.Lerp(originalRun.sidewaysSpeed, target.sidewaysSpeed, t),
                Mathf.Lerp(originalRun.backwardsSpeed, target.backwardsSpeed, t)
            );

            yield return null;
        }

        movementScript.Movement.walkingSpeeds = target;
        movementScript.Movement.runSpeeds = target;
        movementScript.enabled = false;
    }

    //Blind Effect but IT DOES NOT WORK for some unknown reasons
    IEnumerator IncreaseVignette()
    {
        float duration = 3f; // time in seconds to reach target
        float elapsed = 0f;
        float startIntensity = fpsCamVignette.intensity.value;
        float targetIntensity = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            fpsCamVignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        fpsCamVignette.intensity.value = targetIntensity;
    }
}

