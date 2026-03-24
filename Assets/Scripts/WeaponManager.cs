using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;


public class WeaponManager : MonoBehaviour
{
    //List of weapons the player has, assignable in Unity editor
    [SerializeField] protected Weapon weapon1;
    [SerializeField] protected Weapon weapon2;
    [SerializeField] protected Weapon weapon3;
    [SerializeField] protected Weapon weapon4;
    [SerializeField] protected Weapon weapon5;
    [SerializeField] protected Weapon weapon6;
    [SerializeField] protected Weapon weapon7;
    [SerializeField] protected Weapon weapon8;
    [SerializeField] protected Weapon weapon9;

    Weapon[] weaponArr = {null, weapon1, weapon2, weapon3, weapon4, weapon5, weapon6, weapon7, weapon8, weapon9};

    //Weapon currently in use
    private Weapon activeWeapon;

    DateTime reloadTimer;
    DateTime switchTimer;
    DateTime shootTimer;

    //newWeapon refers to the position
    private void switchWeapon(int newWeapon)
    {
        if(weaponArr[newWeapon] == activeWeapon || weaponArr[newWeapon] == null)
            return;

        activeWeapon.
        activeWeapon = weaponArr[newWeapon];

    }

    //Each time a player wants to perform an action with their weapon (shoot, reload, switch), check that no other action is being performed/cooling down
    private bool actionReady()
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
