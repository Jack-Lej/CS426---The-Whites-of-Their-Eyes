using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;



public class WeaponManager : MonoBehaviour
{
    //List of weapons the player has, assignable in Unity editor
    [SerializeField] public static GameObject weapon1;
    [SerializeField]  static Weapon weapon2;
    [SerializeField]  static Weapon weapon3;
    [SerializeField]  static Weapon weapon4;
    [SerializeField]  static Weapon weapon5;
    [SerializeField]  static Weapon weapon6;
    [SerializeField]  static Weapon weapon7;
    [SerializeField]  static Weapon weapon8;
    [SerializeField]  static Weapon weapon9;

    [SerializeField] protected TMP_Text weaponText;

    readonly Weapon[] weaponArr = {null, weapon1, weapon2, weapon3, weapon4, weapon5, weapon6, weapon7, weapon8, weapon9};

    //Weapon currently in use
    private Weapon activeWeapon;

    DateTime reloadTimer;
    DateTime switchTimer;
    DateTime shootTimer;

    //newWeapon refers to the position
    private void SwitchWeapon(int newWeapon)
    {
        if(weaponArr[newWeapon] == activeWeapon || weaponArr[newWeapon] == null)
            return;

        activeWeapon.SleepWeapon();
        activeWeapon = weaponArr[newWeapon];
        switchTimer = DateTime.Now;
        switchTimer.AddMilliseconds(1500);
        weaponText.text = activeWeapon.WakeWeapon();
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reloadTimer = DateTime.Now;
        switchTimer = DateTime.Now;
        shootTimer = DateTime.Now;
        activeWeapon = weapon1;
    }

    // Update is called once per frame
    void Update()
    {
        if(ActionReady())
        {
            if(Input.GetButtonDown("Fire1"))
            {
                weaponText.text = activeWeapon.FireWeapon();
                shootTimer = DateTime.Now.AddMilliseconds(activeWeapon.GetWeaponFireDelay());
            }
            else if(Input.GetButtonDown("Reload"))
            {
                weaponText.text = activeWeapon.ReloadWeapon();
                reloadTimer = DateTime.Now.AddMilliseconds(activeWeapon.GetReloadDelay());
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
        }
    }
}
