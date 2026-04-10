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

    void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            LoadFirstLevel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
