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

        menuPanel.alpha = 0;
        menuPanel.interactable = false;
        menuOpen = false;

        controlsOpen = false;
        controlsGroup.alpha = 0;    
        controlsGroup.interactable = false;
        controlsGroupPosition = controlsGroup.GetComponent<RectTransform>();
        rectTransform = controlsGroup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(10000f, 10000f);
        
        weaponsOpen = false;
        weaponsGroup.alpha = 0;
        weaponsGroup.interactable = false;
        weaponsGroupPosition = weaponsGroup.GetComponent<RectTransform>();
        rectTransform = weaponsGroup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(10000f, 10000f);
        
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
            resumeButton.interactable = true;
            controlsButton.interactable = true;
            weaponsButton.interactable = true;
            menuOpen = true;
        }
        else
        {
            weaponManager.GetActiveWeapon().WakeWeapon();
            menuPanel.alpha = 0;
            menuPanel.interactable = false;
            resumeButton.interactable = false;
            controlsButton.interactable = false;
            weaponsButton.interactable = false;
            menuOpen = false;
            Time.timeScale = 1;
        }
        playerController.ToggleLockCursor();
    }

    public void ToggleControls()
    {
        Debug.Log("in controls toggle");
        if(!controlsOpen)
        {
            controlsOpen = true;
            controlsGroup.alpha = 1;
            controlsGroup.interactable = true;
            rectTransform = controlsGroup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = controlsGroupPosition.anchoredPosition;
        }
        else
        {
            controlsOpen = false;
            controlsGroup.alpha = 0;
            controlsGroup.interactable = false;
            rectTransform = controlsGroup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(10000f, 10000);
        }
    }

    public void ToggleWeapons()
    {
        Debug.Log("in weapons toggle");
        if(!weaponsOpen)
        {
            weaponsOpen = true;
            weaponsGroup.alpha = 1;
            weaponsGroup.interactable = true;
            rectTransform = weaponsGroup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = weaponsGroupPosition.anchoredPosition;
        }
        else
        {
            weaponsOpen = false;
            weaponsGroup.alpha = 0;
            weaponsGroup.interactable = false;
            rectTransform = weaponsGroup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(10000, 10000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
