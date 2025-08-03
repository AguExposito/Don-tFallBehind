using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    //button
    [SerializeField] Button continueButton;
    public bool continued = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //by by button for now
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
        StartCoroutine(LoadSceneAsync());
    }

    //loads scene but doesn display it
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //hello button once operation done
            if (operation.progress >= 0.9f)
            {
                if(continueButton!=null)
                    continueButton.gameObject.SetActive(true);
            }
            if (continued == true)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

    }

    //buton utility - activates the loaded scene once continue is pressed.
    public void ActivateLoadedScene()
    {
        continued = true;
    }
}
