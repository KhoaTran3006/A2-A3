using System.Collections;
using System.Collections.Generic;
using Hertzole.GoldPlayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChecker : MonoBehaviour
{
    public bool isScene2;
    public bool isScene1;
    public Animator fpsAnima;

    public FadeInFadeOut fadeinScript;
    public bool isTriggered;

    public GoldPlayerController playerScript;
    public SwitchCamera switchCameraScript;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Scene 2") // Replace with your actual scene name
        {
            isScene2 = true;
            //Debug.Log("We are in Scene 2!");
            playerScript.Camera.CanLookAround = false;
            fadeinScript.beginningScreen.SetActive(true);
        }
        else
        {
            isScene2 = false;
            //Debug.Log("This is not Scene 2.");
        }

        if (currentScene == "Scene 1") // Replace with your actual scene name
        {
            isScene1 = true;
            //Debug.Log("We are in Scene 1!");
            switchCameraScript.canEquipCam = false;
            fadeinScript.beginningScreen.SetActive(false);
        }
        else
        {
            isScene1 = false;
            //Debug.Log("This is not Scene 1.");
            switchCameraScript.canEquipCam = true;
        }
    }

    public void Update()
    {
        if (isScene2 && !isTriggered)
        {
            StartCoroutine(StartToWakeUp());
            isTriggered = true;
        }
    }

    IEnumerator StartToWakeUp()
    {
        yield return new WaitForSeconds(3f);
        fadeinScript.WakeUp();
        //Debug.Log("YO");

        yield return new WaitForSeconds(0.5f);
        fadeinScript.FadeOut();

        yield return new WaitForSeconds(1.5f);
        fadeinScript.WakeUp();
        fpsAnima.SetBool("Wakey", true);
        //Debug.Log("Where am I");

        yield return new WaitForSeconds(3f);
        playerScript.Camera.CanLookAround = true;
    }
}
