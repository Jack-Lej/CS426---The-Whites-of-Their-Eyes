using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] Button startButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Level 1");
        });
        tutorialButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Tutorial");
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
