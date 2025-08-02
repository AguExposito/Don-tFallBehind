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
    void Quit()
    {
        Application.Quit();
    }
}
