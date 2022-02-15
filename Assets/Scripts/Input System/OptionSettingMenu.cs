using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettingMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI musicLabel;
    [SerializeField] internal  Slider MusicSlider;
    [SerializeField] private TextMeshProUGUI backgroundSoundsLabel;
    [SerializeField] internal  Slider backgroundSoundsSlider;
    [SerializeField] private TextMeshProUGUI LevelsLabel;
    [SerializeField] private  Slider LevelSlider;
    [SerializeField] private Graphic colorTeamWhite;
    [SerializeField] private Graphic colorTeamBlack;
    [SerializeField] private Material materialWhiteTeam;
    [SerializeField] private Material materialBlackTeam;
    internal int levelsAI;
    private void Start()
    {
        colorTeamWhite.color = materialWhiteTeam.color;
        colorTeamBlack.color = materialBlackTeam.color;
    }

    void Update()
    {
        levelsAI = (int) (LevelSlider.value*5 + 1) ;
        musicLabel.SetText($"{(int) (MusicSlider.value*100)}");
        backgroundSoundsLabel.SetText($"{(int) (backgroundSoundsSlider.value*100)}");
        LevelsLabel.SetText($"{levelsAI}");
        if(gameObject.activeSelf) UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        materialBlackTeam.color = colorTeamBlack.color;
        materialWhiteTeam.color = colorTeamWhite.color;
    }
}
