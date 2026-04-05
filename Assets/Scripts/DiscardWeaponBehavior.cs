using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DiscardWeaponBehavior : MonoBehaviour
{

    
    [SerializeField] WeaponManager manager;

    //Each weapon has three elemnts in its "drop x" interface; a field to enter how much ammo to drop,
    //a button to drop that much ammo, and a button to drop the weapon and all its ammo at once
    [SerializeField] Button w1AmmoButton;
    [SerializeField] Button w1WeaponButton;
    [SerializeField] TMP_InputField w1Field;

    [SerializeField] Button w2AmmoButton;
    [SerializeField] Button w2WeaponButton;
    [SerializeField] TMP_InputField w2Field;

    [SerializeField] Button w3AmmoButton;
    [SerializeField] Button w3WeaponButton;
    [SerializeField] TMP_InputField w3Field;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        w1AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(1, w1Field.text);
        });
        w1WeaponButton.onClick.AddListener(() =>
        {
            manager.DropWeapon(1);
        });
        w2AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(2, w2Field.text);
        });
        w2WeaponButton.onClick.AddListener(() =>
        {
            manager.DropWeapon(2);
        });
        w3AmmoButton.onClick.AddListener(() =>
        {
            manager.DropAmmo(3, w3Field.text);
        });
        w3WeaponButton.onClick.AddListener(() =>
        {
            manager.DropWeapon(3);
        });
    }

    void Awake()
    {
       
    }


    private void OnTriggerEnter(Collider collider)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
