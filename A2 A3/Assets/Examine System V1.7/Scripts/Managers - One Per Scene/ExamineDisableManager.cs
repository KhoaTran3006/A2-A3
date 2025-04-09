using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

namespace ExamineSystem
{
    public class ExamineDisableManager : MonoBehaviour
    {
        [SerializeField] private ExamineInteractor interactorScript;
        [SerializeField] private MonoBehaviour player;
        [SerializeField] private BlurOptimized blur;

        [SerializeField] private Camera fpsCamera;
        [SerializeField] private Camera spiritualCamera;
        [SerializeField] private BlurOptimized fpsBlur;
        [SerializeField] private BlurOptimized spiritualBlur;

        [Header("Should persist?")]
        [SerializeField] private bool persistAcrossScenes = true;

        public static ExamineDisableManager instance;

        void Awake()
        {
            
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                if (persistAcrossScenes)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        public void DisablePlayer(bool disable)
        {
            if (disable)
            {
                if (player != null)
                    player.enabled = false;
                else
                    Debug.LogWarning("DisableManager: Player not assigned.");

                interactorScript.enabled = false;

                // Determine which camera is active and blur that one
                if (fpsCamera.enabled && fpsBlur != null)
                    fpsBlur.enabled = true;
                else if (spiritualCamera.enabled && spiritualBlur != null)
                    spiritualBlur.enabled = true;

                ExamineUIManager.instance.EnableCrosshair(false);
            }
            else
            {
                if (player != null)
                    player.enabled = true;
                else
                    Debug.LogWarning("DisableManager: Player not assigned.");

                interactorScript.enabled = true;

                // Disable blur on both cameras
                if (fpsBlur != null)
                    fpsBlur.enabled = false;
                if (spiritualBlur != null)
                    spiritualBlur.enabled = false;

                ExamineUIManager.instance.EnableCrosshair(true);
            }
        }
    }
}
