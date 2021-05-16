using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public const string FileName = "SaveData";
    [SerializeField] AudioMixer mixerMaster = null;


    private void Start()
    {
        SettingsData settings = new SettingsData();

        if (BinarySerialization.IsFileExist(Settings.SettingsDataName)) settings = BinarySerialization.Deserialize<SettingsData>(Settings.SettingsDataName);
        else
        {
            settings.resolutionWidht = Screen.currentResolution.width;
            settings.resolutionHeight = Screen.currentResolution.height;
            BinarySerialization.Serialize(Settings.SettingsDataName, settings);
        }

        mixerMaster.SetFloat("Volume", settings.volume);
        Screen.SetResolution(settings.resolutionWidht, settings.resolutionHeight, settings.fullScreen);
        QualitySettings.SetQualityLevel(settings.qualityIndex);
    }
}
