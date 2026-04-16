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

    [Header("Controls Pop-Up Items")] 
    [SerializeField] CanvasGroup controlsGroup;
    [SerializeField] Button controlsCloseButton;

    [Header("Weapons Pop-Up Items")] 
    [SerializeField] CanvasGroup weaponsGroup;
    [SerializeField] Button weaponsCloseButton;



    bool menuOpen = true;
    bool controlsOpen = true;
    bool weaponsOpen = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.alpha = 0;
        resumeButton.onClick.AddListener(() =>
        {
            weaponManager.DisableMouse();
            ToggleMenu();
        });
        controlsButton.onClick.AddListener(() =>
        {
            //weaponManager.DisableMouse();
            ToggleControls();
        });
        weaponsButton.onClick.AddListener(() =>
        {
            //weaponManager.DisableMouse();
            ToggleWeapons();
        });
        ToggleControls();
        ToggleMenu();
        ToggleWeapons();
    }

    public void ToggleMenu()
    {
        if(!menuOpen)
        {
            weaponManager.GetActiveWeapon().SleepWeapon();
            menuPanel.alpha = 1;
            menuPanel.interactable = true;
            Time.timeScale = 0;
            menuOpen = true;
        }
        else
        {
            weaponManager.GetActiveWeapon().WakeWeapon();
            menuPanel.alpha = 0;
            Time.timeScale = 1;
            menuPanel.interactable = false;
            menuOpen = false;
        }
        playerController.ToggleLockCursor();
    }

    public void ToggleControls()
    {
        if(!controlsOpen)
        {
            controlsOpen = true;
            controlsGroup.interactable = true;
            controlsGroup.alpha = 1;
        }
        else
        {
            controlsOpen = false;
            controlsGroup.interactable = false;
            controlsGroup.alpha = 0;
        }
    }

    public void ToggleWeapons()
    {
        if(!weaponsOpen)
        {
            weaponsOpen = true;
            weaponsGroup.interactable = true;
            weaponsGroup.alpha = 1;
        }
        else
        {
            weaponsOpen = false;
            weaponsGroup.interactable = false;
            weaponsGroup.alpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
