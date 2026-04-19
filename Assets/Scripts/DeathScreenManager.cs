using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Level 1");
        });
        menuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title Screen");
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
