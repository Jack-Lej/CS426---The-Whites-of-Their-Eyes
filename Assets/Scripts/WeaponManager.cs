using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;



public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    //List of weapons the player has, assignable in Unity editor
    [SerializeField] Weapon weapon1;
    [SerializeField] Weapon weapon2;
    [SerializeField] Weapon weapon3;
    [SerializeField] Weapon weapon4;
    [SerializeField] Weapon weapon5;
    [SerializeField] Weapon weapon6;
    [SerializeField] Weapon weapon7;
    [SerializeField] Weapon weapon8;
    [SerializeField] Weapon weapon9;

    [Header("Health Kit Info")]

    [SerializeField] int numHealthKits;

    [SerializeField] int healthKitHealing;

    [SerializeField] Character player;

    [Header("Canvas Text")]

    [SerializeField] protected TMP_Text weaponText;

    [SerializeField] protected TMP_Text weightText;

    [SerializeField] protected TMP_Text healthkitText;

    [SerializeField] protected DiscardWeaponBehavior weightUI;

    [SerializeField] protected BasicFPCC playerMovement;

    [SerializeField] protected MenuManager menuManager;

    private Weapon[] weaponArr = new Weapon[10];

    //Weapon currently in use
    private Weapon activeWeapon;

    private int activeWeaponNum;
//helklo
    private float totalWeight;
    private float startWeight;

    DateTime reloadTimer;
    bool switchAlarm;
    bool mouseDisabled;
    DateTime switchTimer;
    DateTime shootTimer;

    public static WeaponManager Instance;


    void Start()
    {
        reloadTimer = DateTime.Now;
        switchTimer = DateTime.Now;
        switchAlarm = false;
        mouseDisabled = false;
        shootTimer = DateTime.Now;

        weaponArr[0] = null;
        weaponArr[1] = weapon1;
        weaponArr[2] = weapon2;
        weaponArr[3] = weapon3;
        weaponArr[4] = weapon4;
        weaponArr[5] = weapon5;
        weaponArr[6] = weapon6;
        weaponArr[7] = weapon7;
        weaponArr[8] = weapon8;
        weaponArr[9] = weapon9;
        activeWeapon = weapon1;
        activeWeapon.WakeWeapon();
        activeWeaponNum = 1;

        healthkitText.text = string.Concat("Healthkits: ", numHealthKits);
        weaponText.text = activeWeapon.GetWeaponText();

        UpdateWeight();
        startWeight = totalWeight;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //newWeapon refers to the position in the weapons array
    private void SwitchWeapon(int newWeapon)
    {
        if(newWeapon < 1)
        {
            if(newWeapon == -1)
            {
                while(true)
                {
                    activeWeaponNum--;
                    if(activeWeaponNum == 0)
                        activeWeaponNum = 6;
                    if(weaponArr[activeWeaponNum] != null)
                        break;
                }
                activeWeapon.SleepWeapon();
                activeWeapon = weaponArr[activeWeaponNum];
                switchTimer = DateTime.Now.AddMilliseconds(1500);
                weaponText.text = "Switching to " + activeWeapon.GetWeaponName();
                switchAlarm = true;
            }
            else
            {
                while(true)
                {
                    activeWeaponNum++;
                    if(activeWeaponNum == 7)
                        activeWeaponNum = 1;
                    if(weaponArr[activeWeaponNum] != null)
                        break;
                }
                activeWeapon.SleepWeapon();
                activeWeapon = weaponArr[activeWeaponNum];
                switchTimer = DateTime.Now.AddMilliseconds(1500);
                weaponText.text = "Switching to " + activeWeapon.GetWeaponName();
                switchAlarm = true;
            }
        }
        else
        {

            if(weaponArr[newWeapon] == activeWeapon || weaponArr[newWeapon] == null)
            {
                return;
            }

            activeWeapon.SleepWeapon();
            activeWeapon = weaponArr[newWeapon];
            activeWeaponNum = newWeapon;
            switchTimer = DateTime.Now.AddMilliseconds(1500);
            weaponText.text = "Switching to " + activeWeapon.GetWeaponName();
            switchAlarm = true;
        }
    }

    //Each time a player wants to perform an action with their weapon (shoot, reload, switch), check that no other action is being performed/cooling down
    private bool ActionReady()
    {
        if(reloadTimer.CompareTo(DateTime.Now) <= 0 && switchTimer.CompareTo(DateTime.Now) <= 0 && shootTimer.CompareTo(DateTime.Now) <= 0)
        {
            return true;
        }
        return false;
    }

    public void DropAmmo(int weapon, string amount)
    {
        try
        {
            int amt = Int32.Parse(amount);
            weaponArr[weapon].DropAmmo(amt);
        }
        //Do nothing on the catch, if amount was incorrect
        catch (FormatException) {}
    }

    public void DropHealthkit(string amount)
    {
        try
        {
            int amt = Int32.Parse(amount);
            numHealthKits -= amt;
            healthkitText.text = string.Concat("Healthkits: ", numHealthKits);
        }
        //Do nothing on the catch, if amount was incorrect
        catch (FormatException) {}
        
    }

    public Weapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public void UseHealthKit()
    {
        if(numHealthKits <= 0)
            return;
        player.Heal(healthKitHealing);
        numHealthKits--;
        healthkitText.text = string.Concat("Healthkits: ", numHealthKits);
        reloadTimer = DateTime.Now.AddMilliseconds(1000); 
        weaponText.text = "Using Healthkit";  
    }

    public void DropWeapon(int weapon)
    {
        if(weaponArr[weapon] == activeWeapon)
        {
            weaponArr[weapon] = null;
            for(int i = 1; i < 10; i++)
            {
                if(weaponArr[i] != null)
                {
                    SwitchWeapon(i);
                }
            }
        }
        else
            weaponArr[weapon] = null;
    }

    private void UpdateWeight()
    {
        totalWeight = 0;
        for(int i = 1; i < 10; i++)
        {
            Weapon w = weaponArr[i];
            if(w == null)
                continue;
            totalWeight += w.GetWeaponWeight();    
        }

        totalWeight += 1*numHealthKits; //include health kit weight here

        weightText.text = string.Concat("Weight: ", totalWeight);
    }

    public void DisableMouse()
    {
        mouseDisabled = !mouseDisabled;
    }
    public float GetTotalWeight()
    {
        return totalWeight;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(ActionReady())
        {
            //Update the weight from all items
            UpdateWeight();
            playerMovement.UpdateSpeedBonus(1 + (1 - (totalWeight/startWeight)));


            //Once the switch timer is up, spawn the new weapon and reset the bool
            if(switchAlarm)
            {
                activeWeapon.WakeWeapon();
                switchAlarm = false;
            }
            weaponText.text = activeWeapon.GetWeaponText();
            if(Input.GetKey(KeyCode.Mouse0) && !mouseDisabled)
            {
                shootTimer = DateTime.Now.AddMilliseconds(activeWeapon.GetWeaponFireDelay());
                weaponText.text = activeWeapon.FireWeapon();
            }
            else if(Input.GetButtonDown("Reload"))
            {
                reloadTimer = DateTime.Now.AddMilliseconds(activeWeapon.GetReloadDelay());
                weaponText.text = activeWeapon.ReloadWeapon();
            }
            else if(Input.GetButtonDown("Switch Weapon 1"))
                SwitchWeapon(1);
            else if(Input.GetButtonDown("Switch Weapon 2"))
                SwitchWeapon(2);
            else if(Input.GetButtonDown("Switch Weapon 3"))
                SwitchWeapon(3);
            else if(Input.GetButtonDown("Switch Weapon 4"))
                SwitchWeapon(4);
            else if(Input.GetButtonDown("Switch Weapon 5"))
                SwitchWeapon(5);
            else if(Input.GetButtonDown("Switch Weapon 6"))
                SwitchWeapon(6);
            else if(Input.GetButtonDown("Switch Weapon 7"))
                SwitchWeapon(7);
            else if(Input.GetButtonDown("Switch Weapon 8"))
                SwitchWeapon(8);
            else if(Input.GetButtonDown("Switch Weapon 9"))
                SwitchWeapon(9);
            else if(Input.GetButtonDown("Switch Weapon Q"))  
            {  
                Debug.Log("here");
                SwitchWeapon(-1);
            }
            else if (Input.GetButtonDown("Switch Weapon E"))
                SwitchWeapon(0);  
            else if(Input.GetButtonDown("Use Health Kit"))
                UseHealthKit(); 
            //Used to activate management mode
            else if(Input.GetKey(KeyCode.BackQuote))
            {
                switchTimer = DateTime.Now.AddMilliseconds(400);
                mouseDisabled = !mouseDisabled;   
            }
            else if(Input.GetButtonDown("Pause"))
            {
                Debug.Log("In key detect");
                mouseDisabled = !mouseDisabled;
                menuManager.ToggleMenu();
            }
        }
    }
}
