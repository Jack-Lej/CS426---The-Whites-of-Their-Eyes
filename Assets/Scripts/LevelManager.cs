using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    
    public static int numEnemies;

    [SerializeField] int nextLevel;

    [SerializeField] int maxWeight;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] DiscardWeaponBehavior discardManager;
    [SerializeField] GameObject levelExitBarrier;

    [SerializeField] GameObject nextLevelTrigger;


    

    //Used to stop repeating unlocking the next level or displaying the max weight info
    private bool infoUpdated;
    private bool levelUnlocked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infoUpdated = false;
        levelUnlocked = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if(collider.gameObject.tag == "Player")
        {
           
            if(nextLevel == 2)
            {
                
                SceneManager.LoadScene("Level 2");
                collider.GetComponent<CharacterController>().enabled = false;
                collider.transform.position = new Vector3(-120, 2, 1);
                collider.GetComponent<CharacterController>().enabled = true;
            }
            else if(nextLevel == 3)
            {
                SceneManager.LoadScene("Level 3");
                collider.GetComponent<CharacterController>().enabled = false;
                collider.transform.position = new Vector3(-90, 2, 0);
                collider.GetComponent<CharacterController>().enabled = true;
            }
            
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if(numEnemies <= 0)
        {
            if(!infoUpdated)
            {
                Debug.Log("In info");
                discardManager.DisplayLevelWeightText(maxWeight);
                infoUpdated = true;
            }

            if(!levelUnlocked && maxWeight >= weaponManager.GetTotalWeight())
            {
                Destroy(levelExitBarrier);
                levelUnlocked = true;
            }
        }
    }
}
