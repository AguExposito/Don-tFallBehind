using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex==1) 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //loads scene 1
    public void Play()
    {
        loadingScreen.SetActive(true);
        //SceneManager.LoadSceneAsync(1);
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    //self explanatory
    public void Quit()
    {
        Application.Quit();
    }
}
