using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DiscardWeaponBehavior : MonoBehaviour
{

    bool confirmActive = false;

    bool uiActive = true;

    [Header("Other Managers")]
    [SerializeField] WeaponManager manager;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup cGroup;

    [Header("Weapon 1 Drop Elements")]
    //Each weapon has three elemnts in its "drop x" interface; a field to enter how much ammo to drop,
    //a button to drop that much ammo, and a button to drop the weapon and all its ammo at once. The text object is used to change the text when confirming the drop
    //The dateTime and bool are used to keep track of the drop weapon button, and when to reset its state
    [SerializeField] Button w1AmmoButton;
    [SerializeField] Button w1WeaponButton;
    [SerializeField] TextMeshProUGUI w1WeaponButtonText;
    [SerializeField] TMP_InputField w1Field;
    DateTime w1Confirm;
    bool drop1Pressed = false;

    [Header("Weapon 2 Drop Elements")]
    [SerializeField] Button w2AmmoButton;
    [SerializeField] Button w2WeaponButton;
    [SerializeField] TextMeshProUGUI w2WeaponButtonText;
    [SerializeField] TMP_InputField w2Field;
    DateTime w2Confirm;
    bool drop2Pressed = false;

    [Header("Weapon 3 Drop Elements")]
    [SerializeField] Button w3AmmoButton;
    [SerializeField] Button w3WeaponButton;
    [SerializeField] TextMeshProUGUI w3WeaponButtonText;
    [SerializeField] TMP_InputField w3Field;
    DateTime w3Confirm;
    bool drop3Pressed = false;

    [Header("Weapon 4 Drop Elements")]
    [SerializeField] Button w4AmmoButton;
    [SerializeField] Button w4WeaponButton;
    [SerializeField] TextMeshProUGUI w4WeaponButtonText;
    [SerializeField] TMP_InputField w4Field;
    DateTime w4Confirm;
    bool drop4Pressed = false;

    [Header("Weapon 5 Drop Elements")]
    [SerializeField] Button w5AmmoButton;
    [SerializeField] Button w5WeaponButton;
    [SerializeField] TextMeshProUGUI w5WeaponButtonText;
    [SerializeField] TMP_InputField w5Field;
    DateTime w5Confirm;
    bool drop5Pressed = false;

    [Header("Weapon 6 Drop Elements")]
    [SerializeField] Button w6AmmoButton;
    [SerializeField] Button w6WeaponButton;
    [SerializeField] TextMeshProUGUI w6WeaponButtonText;
    [SerializeField] TMP_InputField w6Field;
    DateTime w6Confirm;
    bool drop6Pressed = false;

    [Header("Health Kit Drop Elements")]
    [SerializeField] Button healthKitButton;
    [SerializeField] TMP_InputField healthkitField;

    [SerializeField] CanvasGroup levelWeightTextGroup;
    [SerializeField] TMP_Text levelWeightText;

    public static DiscardWeaponBehavior Instance;

    //Function to confirm the dropping of a weapon; player must click twice in order to avoid game-ruining accidents
    void DropWeapon(int w)
    {
        switch(w)
        {
            case 1:
                {
                    if(!drop1Pressed)
                    {
                        drop1Pressed = true;
                        w1Confirm = DateTime.Now.AddMilliseconds(2500);
                        w1WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w1AmmoButton);
                        Destroy(w1Field);
                        Destroy(w1WeaponButton);
                        manager.DropWeapon(1);
                    }
                    break;
                }
            case 2:
                {
                    if(!drop2Pressed)
                    {
                        drop2Pressed = true;
                        w2Confirm = DateTime.Now.AddMilliseconds(2500);
                        w2WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w2AmmoButton);
                        Destroy(w2Field);
                        Destroy(w2WeaponButton);
                        manager.DropWeapon(2);
                    }
                    break;
                }    
            case 3:
                {
                    if(!drop3Pressed)
                    {
                        drop3Pressed = true;
                        w3Confirm = DateTime.Now.AddMilliseconds(2500);
                        w3WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w3AmmoButton);
                        Destroy(w3Field);
                        Destroy(w3WeaponButton);
                        manager.DropWeapon(3);
                    }
                    break;
                } 
            case 4:
                {
                    if(!drop4Pressed)
                    {
                        drop4Pressed = true;
                        w4Confirm = DateTime.Now.AddMilliseconds(2500);
                        w4WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w4AmmoButton);
                        Destroy(w4Field);
                        Destroy(w4WeaponButton);
                        manager.DropWeapon(4);
                    }
                    break;
                } 
            case 5:
                {
                    if(!drop5Pressed)
                    {
                        drop5Pressed = true;
                        w5Confirm = DateTime.Now.AddMilliseconds(2500);
                        w5WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w5AmmoButton);
                        Destroy(w5Field);
                        Destroy(w5WeaponButton);
                        manager.DropWeapon(5);
                    }
                    break;
                } 
            case 6:
                {
                    if(!drop6Pressed)
                    {
                        drop6Pressed = true;
                        w6Confirm = DateTime.Now.AddMilliseconds(2500);
                        w6WeaponButtonText.text = "Confirm?";
                    }
                    else
                    {
                        confirmActive = false;
                        Destroy(w6AmmoButton);
                        Destroy(w6Field);
                        Destroy(w6WeaponButton);
                        manager.DropWeapon(6);
                    }
                    break;
                }                    

        }
    }

    void ToggleUI()
    {
        if(uiActive)
        {
            uiActive = false;
            cGroup.alpha = 0;
            cGroup.interactable = false;
        }
        else
        {
            uiActive = true;
            cGroup.alpha = 1;
            cGroup.interactable = true;
        }
    }

    public void DisplayLevelWeightText(int maxWeight)
    {
        levelWeightTextGroup.alpha = 1;
        levelWeightTextGroup.interactable = true;
        levelWeightText.text = string.Concat("Level Cleared! Max weight for next level is: ", maxWeight, ". Press U and ` to activate weapon drop UI");
    }

    public void HideLevelWeightText()
    {
        levelWeightTextGroup.alpha = 0;
        levelWeightTextGroup.interactable = false;
    }

    void Start()
    {
        healthKitButton.onClick.AddListener(() =>
        {
            manager.DropHealthkit(healthkitField.text);
        });
        w1AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(1, w1Field.text);
        });
        w1WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(1);
        });
        w2AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(2, w2Field.text);
        });
        w2WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(2);
        });
        w3AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(3, w3Field.text);
        });
        w3WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(3);
        });
        w4AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(4, w4Field.text);
        });
        w4WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(4);
        });
        w5AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(5, w5Field.text);
        });
        w5WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(5);
        });
        w6AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(6, w6Field.text);
        });
        w6WeaponButton.onClick.AddListener(() =>
        {
            DropWeapon(6);
        });
        
        levelWeightTextGroup.alpha = 0;
        levelWeightTextGroup.interactable = false;
        levelWeightTextGroup.blocksRaycasts = false;
        ToggleUI();
    }
    



    //ConfirmActive used to ensure Update only has to only perform a bool check each frame
    void Update()
    {
        if(confirmActive)
        {
            if(w1Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop1Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop Shotgun";
            }
            if(w2Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop2Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop Rifle";
            }
            if(w3Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop3Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop Railgun";
            }
            if(w4Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop4Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop Launcher";
            }
            if(w5Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop5Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop Minigun";
            }
            if(w6Confirm.CompareTo(DateTime.Now) <= 0)
            {
                drop6Pressed = false;
                confirmActive = false;
                w1WeaponButtonText.text = "Drop SoDL";
            }
        }

        if(Input.GetButtonDown("Show/Hide Weapon Drop UI"))
            ToggleUI();
    }
}
