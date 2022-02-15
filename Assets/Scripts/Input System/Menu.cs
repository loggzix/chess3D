using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource backgroundSounds;
    [SerializeField] private GameObject menuOutGame;
    [SerializeField] private GameObject optionSetting;
    [SerializeField] public OptionSettingMenu setting;
    public static int LEVELS = 3;
    public void ExitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        // SceneManager.LoadScene("Chess");
        menuOutGame.SetActive(false);
    }

    public void SettingButton()
    {
        menuOutGame.SetActive(true);
    }
    public void OptionButton()
    {
        optionSetting.SetActive(true);
        if(colorPickerWhite.activeSelf || colorPickerBlack.activeSelf)
        {
            colorPickerBlack.SetActive(false);
            colorPickerWhite.SetActive(false);
        }
    }
    public void SaveButton()
    {
        optionSetting.SetActive(false);
        music.volume = setting.MusicSlider.value;
        backgroundSounds.volume = setting.backgroundSoundsSlider.value;
        LEVELS = setting.levelsAI;
    }
    
    [SerializeField] GameObject colorPickerWhite;
    [SerializeField] GameObject colorPickerBlack;
    public void SelectMetarialTeamWhite()
    {
        colorPickerWhite.SetActive(true);
    }
    
    public void SelectMetarialTeamBlack()
    {
        colorPickerBlack.SetActive(true);
    }
}