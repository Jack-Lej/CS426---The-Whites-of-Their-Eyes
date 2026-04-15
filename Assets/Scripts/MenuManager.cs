using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Other Managers")]
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] BasicFPCC playerController;

    [Header("Menu Items")]
    [SerializeField] CanvasGroup menuPanel;
    [SerializeField] Button resumeButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button weaponsButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button quitButton;


    bool menuOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.alpha = 0;
        resumeButton.onClick.AddListener(() =>
        {
            weaponManager.DisableMouse();
            ToggleMenu();
        });
    }

    public void ToggleMenu()
    {
        Debug.Log("toggle menu");
        if(!menuOpen)
        {
            menuPanel.alpha = 1;
            menuPanel.interactable = true;
            Time.timeScale = 0;
            menuOpen = true;
        }
        else
        {
            menuPanel.alpha = 0;
            Time.timeScale = 1;
            menuPanel.interactable = false;
            menuOpen = false;
        }
        playerController.ToggleLockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
