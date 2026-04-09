using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DiscardWeaponBehavior : MonoBehaviour
{

    bool confirmActive = false;

    bool uiActive = true;
    [SerializeField] WeaponManager manager;

    [SerializeField] CanvasGroup cGroup;

    //Each weapon has three elemnts in its "drop x" interface; a field to enter how much ammo to drop,
    //a button to drop that much ammo, and a button to drop the weapon and all its ammo at once. The text object is used to change the text when confirming the drop
    //The dateTime and bool are used to keep track of the drop weapon button, and when to reset its state
    [SerializeField] Button w1AmmoButton;
    [SerializeField] Button w1WeaponButton;
    [SerializeField] TextMeshProUGUI w1WeaponButtonText;
    [SerializeField] TMP_InputField w1Field;
    DateTime w1Confirm;
    bool drop1Pressed = false;

    [SerializeField] Button w2AmmoButton;
    [SerializeField] Button w2WeaponButton;
    [SerializeField] TextMeshProUGUI w2WeaponButtonText;
    [SerializeField] TMP_InputField w2Field;
    DateTime w2Confirm;
    bool drop2Pressed = false;

    [SerializeField] Button w3AmmoButton;
    [SerializeField] Button w3WeaponButton;
    [SerializeField] TextMeshProUGUI w3WeaponButtonText;
    [SerializeField] TMP_InputField w3Field;
    DateTime w3Confirm;
    bool drop3Pressed = false;

    [SerializeField] Button healthKitButton;
    [SerializeField] TMP_InputField healthkitField;

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
                        w3Confirm = DateTime.Now.AddMilliseconds(3500);
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        healthKitButton.onClick.AddListener(() =>
        {
            manager.DropHealthkit(healthkitField.text);
        });
        ToggleUI();
    }

    void Awake()
    {
       
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
        }

        if(Input.GetButtonDown("Show/Hide Weapon Drop UI"))
            ToggleUI();
    }
}
