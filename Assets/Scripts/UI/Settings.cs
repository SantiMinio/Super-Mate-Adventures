using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer masterAudio = null;
    public const string SettingsDataName = "Settings";
    SettingsData data;

    [SerializeField] GameObject principalHud = null;
    [SerializeField] Toggle fullScreenUI = null;
    [SerializeField] Slider volumeSlider = null;
    [SerializeField] Dropdown resolutionDrop = null;
    [SerializeField] Dropdown qualityDrop = null;

    Resolution[] resolutions;

    private void Start()
    {
        data = BinarySerialization.Deserialize<SettingsData>(SettingsDataName);

        resolutions = Screen.resolutions;

        List<string> resolutionsString = new List<string>();
        int current = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionsString.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == data.resolutionWidht && resolutions[i].height == data.resolutionHeight) current = i;
        }

        fullScreenUI.isOn = data.fullScreen;
        volumeSlider.value = data.volumeSlider;
        resolutionDrop.ClearOptions();
        resolutionDrop.AddOptions(resolutionsString);
        resolutionDrop.value = current;
        resolutionDrop.RefreshShownValue();
        qualityDrop.value = data.qualityIndex;
        qualityDrop.RefreshShownValue();
    }

    public void SetVolume(float value)
    {
        var logValue = Mathf.Log10(value) * 20;
        data.volumeSlider = value;
        masterAudio.SetFloat("Volume", logValue);
        Debug.Log(logValue);
        data.volume = logValue;
        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);

        data.qualityIndex = value;

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetResolution(int value)
    {
        Resolution change = resolutions[value];
        Screen.SetResolution(change.width, change.height, data.fullScreen);

        data.resolutionWidht = change.width;
        data.resolutionHeight = change.height;

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        data.fullScreen = value;
        BinarySerialization.Serialize(SettingsDataName, data);
    }
    public void ResetValues()
    {
        data = new SettingsData();

        masterAudio.SetFloat("Volume", data.volume);
        QualitySettings.SetQualityLevel(data.qualityIndex);
        Screen.SetResolution(data.resolutionWidht, data.resolutionHeight, data.fullScreen);
        Screen.fullScreen = data.fullScreen;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) resolutionDrop.value = i;
        }

        fullScreenUI.isOn = data.fullScreen;
        volumeSlider.value = data.volume ;

        resolutionDrop.RefreshShownValue();
 
        qualityDrop.value = data.qualityIndex;
        qualityDrop.RefreshShownValue();

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void Back()
    {
        principalHud.SetActive(true);
        gameObject.SetActive(false);
    }
}
