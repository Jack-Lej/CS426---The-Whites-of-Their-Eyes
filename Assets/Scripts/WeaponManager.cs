using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;



public class WeaponManager : MonoBehaviour
{
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

    [SerializeField] protected TMP_Text weaponText;
    [SerializeField] protected TMP_Text testText;

    private Weapon[] weaponArr = new Weapon[10];

    //Weapon currently in use
    private Weapon activeWeapon;

    DateTime reloadTimer;
    bool switchAlarm;
    DateTime switchTimer;
    DateTime shootTimer;

    void Start()
    {
        reloadTimer = DateTime.Now;
        switchTimer = DateTime.Now;
        switchAlarm = false;
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
    }

    //newWeapon refers to the position in the weapons array
    private void SwitchWeapon(int newWeapon)
    {
        if(weaponArr[newWeapon] == activeWeapon || weaponArr[newWeapon] == null)
        {
            testText.text = "Weapon switch the same or null";
            return;
        }

        activeWeapon.SleepWeapon();
        activeWeapon = weaponArr[newWeapon];
        switchTimer = DateTime.Now.AddMilliseconds(1500);
        weaponText.text = "Switching to " + activeWeapon.GetWeaponName();
        switchAlarm = true;
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
    

    // Update is called once per frame
    void Update()
    {
        if(ActionReady())
        {
            if(switchAlarm)
            {
                activeWeapon.WakeWeapon();
                switchAlarm = false;
            }
            weaponText.text = activeWeapon.GetWeaponText();
            if(Input.GetKeyDown("Fire1"))
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
            {
                //switchTimer = DateTime.Now.AddMilliseconds()
                testText.text = "Weapon 1 Switch";
                SwitchWeapon(1);
            }
            else if(Input.GetButtonDown("Switch Weapon 2"))
            {
                testText.text = "Weapon 2 Switch";
                SwitchWeapon(2);
            }
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
        }
    }
}
