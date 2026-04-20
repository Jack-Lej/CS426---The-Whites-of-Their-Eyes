using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    [Header("Controls Pop-Up Items")] 
    [SerializeField] CanvasGroup controlsGroup;
    [SerializeField] Button controlsCloseButton;
    RectTransform controlsGroupPosition;

    [Header("Weapons Pop-Up Items")] 
    [SerializeField] CanvasGroup weaponsGroup;
    [SerializeField] Button weaponsCloseButton;
    RectTransform weaponsGroupPosition;

    
    


    private RectTransform rectTransform;

    bool menuOpen = true;
    bool controlsOpen = true;
    bool weaponsOpen = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeButton.onClick.AddListener(() =>
        {
            weaponManager.DisableMouse();
            ToggleMenu();
        });
        controlsButton.onClick.AddListener(() =>
        {
            Debug.Log("controls clicked");
            ToggleControls();
        });
        controlsCloseButton.onClick.AddListener(() =>
        {
            Debug.Log("controls close clicked");
            ToggleControls();
        });
        weaponsButton.onClick.AddListener(() =>
        {
            Debug.Log("Weapons clicked");
            ToggleWeapons();
        });
        weaponsCloseButton.onClick.AddListener(() =>
        {
            Debug.Log("Weapons clicked");
            ToggleWeapons();
        });
        menuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Title Screen");
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        menuPanel.alpha = 0;
        menuPanel.interactable = false;
        menuOpen = false;

        controlsOpen = false;
        controlsGroup.alpha = 0;    
        controlsGroup.interactable = false;
        controlsGroup.blocksRaycasts = false;

        weaponsOpen = false;
        weaponsGroup.alpha = 0;
        weaponsGroup.interactable = false;
        weaponsGroup.blocksRaycasts = false;

    }

    public void ToggleMenu()
    {
        Debug.Log("in menu toggle");
        if(!menuOpen)
        {
            Time.timeScale = 0;
            weaponManager.GetActiveWeapon().SleepWeapon();
            menuPanel.alpha = 1;
            menuPanel.interactable = true;
            menuPanel.blocksRaycasts = true;
            menuOpen = true;
        }
        else
        {
            weaponManager.GetActiveWeapon().WakeWeapon();
            menuPanel.alpha = 0;
            menuPanel.interactable = false;
            menuPanel.blocksRaycasts = true;
            menuOpen = false;
            Time.timeScale = 1;
        }
        playerController.ToggleLockCursor();
    }

    public void ToggleControls()
    {
        Debug.Log("controlsOpen:" + controlsOpen);

        if(!controlsOpen)
        {
            controlsOpen = true;
            controlsGroup.alpha = 1;
            controlsGroup.interactable = true;
            controlsGroup.blocksRaycasts = true;
        }
        else
        {
            controlsOpen = false;
            controlsGroup.alpha = 0;
            controlsGroup.interactable = false;
            controlsGroup.blocksRaycasts = false;
        }
    }

    public void ToggleWeapons()
    {
        Debug.Log("weaponsOpen:" + weaponsOpen);
        if(!weaponsOpen)
        {
            weaponsOpen = true;
            weaponsGroup.alpha = 1;
            weaponsGroup.interactable = true;
            weaponsGroup.blocksRaycasts = true;
        }
        else
        {
            
            weaponsOpen = false;
            weaponsGroup.alpha = 0;
            weaponsGroup.interactable = false;
            weaponsGroup.blocksRaycasts = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
