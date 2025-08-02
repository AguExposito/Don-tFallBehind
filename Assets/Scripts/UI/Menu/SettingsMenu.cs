using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider graphicSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle lowResFilter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private GameObject lowResScreen;
    [SerializeField] private RenderTexture lowResTexture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshSettings();
    }

    public void RefreshSettings()
    {
        //Ajusta los sliders a los valores guardados en settings
        graphicSlider.value = Settings.graphicQuality;
        volumeSlider.value = Settings.volumeLevel;
        lowResFilter.isOn = Settings.isLowRes;

        //self exp
        Apply();
    }

    public void Apply()
    {
        //aplica los valores de los sliders al script settings (que los contiene)
        Settings.graphicQuality = (int)graphicSlider.value;
        Settings.volumeLevel = volumeSlider.value;
        Settings.isLowRes = lowResFilter.isOn;

        //aplica los valores de settings al juego
        QualitySettings.SetQualityLevel(Settings.graphicQuality);
        audioMixer.SetFloat("Master", Mathf.Log10(Settings.volumeLevel) * 20);
        if (Settings.isLowRes)
        {
            lowResScreen.gameObject.SetActive(true);
            mainCamera.targetTexture = lowResTexture; // Set the camera's render texture to the low resolution texture
        }
        else
        {
            mainCamera.targetTexture = null; // Disable the camera's render texture
            lowResScreen.gameObject.SetActive(false);
        }
    }
}

public class Settings
{
    public static int graphicQuality;
    public static float volumeLevel = 0.7f;
    public static bool isLowRes = true;
}

