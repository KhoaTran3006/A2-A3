using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public CanvasGroup worldSpaceUI;

    public void ShowUI()
    {
        if (worldSpaceUI != null)
        {
            worldSpaceUI.alpha = 1;
            worldSpaceUI.interactable = false;
            worldSpaceUI.blocksRaycasts = false;
        }
    }

    public void HideUI()
    {
        if (worldSpaceUI != null)
        {
            worldSpaceUI.alpha = 0;
            worldSpaceUI.interactable = false;
            worldSpaceUI.blocksRaycasts = false;
        }
    }
}

