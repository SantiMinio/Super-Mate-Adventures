using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{
    public GameObject[] names = new GameObject[4];
    [SerializeField] GameObject mainScene = null;
    [SerializeField] GameObject levelSelector = null;
    [SerializeField] GameObject settingsScreen = null;

    [SerializeField] AudioClip exitSound = null;

    private void Awake()
    {
        for (int i = 0; i < names.Length; i++)
        {
            names[i].SetActive(false);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        settingsScreen.SetActive(true);
        mainScene.SetActive(false);
    }
    public void Credits()
    {
        if (names[0].activeSelf)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i].SetActive(true);
            }
        }
    }
    public void ExitGame()
    {
        StartCoroutine(ExitDelay());
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

}
