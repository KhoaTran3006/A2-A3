using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [Header("=====Battery UI Setup=====")]
    public Image batteryImage; // The UI Image showing battery status
    public Sprite[] batteryImages; // Array of battery colors for each level
    public GameObject noMoreCam; // Out ò Battery Sprite

    [Header("=====Battery UI Container=====")]
    public GameObject batteryUIContainer;

    [Header("=====Camera Reference=====")]
    public SwitchCamera switchCameraScript;

    private float batteryDrainTimer = 0f;
    private int currentBatteryLevel = 4;
    public float drainTime;

    public bool batteryEmpty = false;
    private bool isCoroutineRunning = false;
    private bool is2ndCoroutineRunning = false;

    void Start()
    {
        UpdateBatteryUI();
        //noMoreCam.SetActive(false);

        if (batteryUIContainer != null)
        {
            batteryUIContainer.SetActive(false); // Start hidden
        }
    }

    void Update()
    {
        /*
        if (switchCameraScript == null || batteryEmpty)
        {
            //noMoreCam.SetActive(true);
            return;
        }
        */

        if (currentBatteryLevel <= 0)
        {
            batteryEmpty = true;
        }

        // Check if camOnHand is true and the coroutine is not running
        if (switchCameraScript.camOnHand == true && !isCoroutineRunning)
        {
            StartCoroutine(ShowBatteryUIWithDelay());
        }

        // Drain battery only when camera is active
        if (switchCameraScript.camOnHand == true)
        {
            batteryDrainTimer += Time.deltaTime;

            if (batteryDrainTimer >= drainTime)
            {
                batteryDrainTimer = 0f;
                DecreaseBattery();
            }
        }


        if (switchCameraScript.camOnHand == false)
        {
            StartCoroutine(TurnOffUI());
            /*
            if (batteryEmpty)
            {
                batteryEmpty = false;
                is2ndCoroutineRunning = false;
            }
            */
        }


        if (switchCameraScript.camOnHand == false && batteryEmpty == false && !is2ndCoroutineRunning)
        {
            //Debug.Log("Why");
            is2ndCoroutineRunning = true;
            CheckCamBattery();
        }
    }

    // Coroutine to wait before enabling the battery UI
    private IEnumerator ShowBatteryUIWithDelay()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1.2f); // Wait for the specified delay
        if (batteryUIContainer != null)
        {
            batteryUIContainer.SetActive(true); // Show battery UI after delay
        }
        isCoroutineRunning = false;
    }

    private IEnumerator TurnOffUI()
    {
        yield return new WaitForSeconds(1.2f);
        batteryUIContainer.SetActive(false);
    }


    public void CheckCamBattery()
    {
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1.2f);
        //Debug.Log("Once?");
        noMoreCam.SetActive(false);
    }



    void DecreaseBattery()
    {
        currentBatteryLevel--;

        UpdateBatteryUI();

        if (currentBatteryLevel <= 0)
        {
            currentBatteryLevel = 0;
            batteryEmpty = true;
            Debug.Log("Out of Battery");

            // If battery empty force turn camera off 
            if (switchCameraScript != null)
            {
                Debug.Log("Calling ToggleCamera Coroutine");
                switchCameraScript.camOnHand = false;
                StartCoroutine(switchCameraScript.ToggleCamera());
                StartCoroutine(switchCameraScript.Fading());
            }
            else
            {
                Debug.LogError("switchCameraScript is null!");
            }
        }

    }
    void UpdateBatteryUI()
    {
        if (currentBatteryLevel >= 0 && currentBatteryLevel < batteryImages.Length)
        {
            batteryImage.sprite = batteryImages[currentBatteryLevel];
        }
    }

    // Reset function
    /*
    public void ResetBattery()
    {
        currentBatteryLevel = 4;
        batteryEmpty = false;
        batteryDrainTimer = 0f;
        UpdateBatteryUI();
    }
    */
}
