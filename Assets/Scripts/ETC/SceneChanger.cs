using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingSlider;
    public void LoadGameScene(string sceneToLoad)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGameSceneAsync(sceneToLoad));
    }
    private IEnumerator LoadGameSceneAsync(string sceneToLoad)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Start loading the main game scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Prevent the scene from activating immediately
        asyncLoad.allowSceneActivation = false;

        // Optional: Update a loading bar or text based on loading progress
        // while the scene is loading in the background
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;
            if (asyncLoad.progress >= 0.9f)
            {
                // When the scene is almost loaded, allow it to activate
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Once the scene is loaded, deactivate the loading screen
        loadingScreen.SetActive(false);
    }
}
