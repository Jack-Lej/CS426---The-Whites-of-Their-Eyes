using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    
    public static int numEnemies;

    [SerializeField] int nextLevel;

    [SerializeField] int maxWeight;
    private WeaponManager weaponManager;
    private DiscardWeaponBehavior discardManager;
    [SerializeField] GameObject levelExitBarrier;

    [SerializeField] GameObject nextLevelTrigger;

    private GameObject player;

    private BasicFPCC playerController;


    

    //Used to stop repeating unlocking the next level or displaying the max weight info
    private bool infoUpdated;
    private bool levelUnlocked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infoUpdated = false;
        levelUnlocked = false;
        playerController = FindObjectOfType<BasicFPCC>();
        player = playerController.GetPlayer();
        weaponManager = playerController.GetWeaponManager();
        discardManager = playerController.GetDiscardWeaponBehavior();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(nextLevel == 2)
            {
                DontDestroyOnLoad(player);
                SceneManager.LoadScene("Level 2");
                discardManager.HideLevelWeightText();
                collider.GetComponent<CharacterController>().enabled = false;
                collider.transform.position = new Vector3(-110.49f, 17f, 0f);
                collider.GetComponent<CharacterController>().enabled = true;
            }
            else if(nextLevel == 3)
            {
                DontDestroyOnLoad(player);
                SceneManager.LoadScene("Level 3");
                discardManager.HideLevelWeightText();
                collider.GetComponent<CharacterController>().enabled = false;
                collider.transform.position = new Vector3(-80.5f, 8.55f, -11.7f);
                collider.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                collider.GetComponent<CharacterController>().enabled = true;
            }
            else if(nextLevel == 4)
            {
                playerController.ToggleLockCursor();
                Destroy(player);
                SceneManager.LoadScene("Credits");
            }
            else if(nextLevel == 0)
            {
                playerController.ToggleLockCursor();
                SceneManager.LoadScene("Title Screen");
            }
            
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            if(!infoUpdated)
            {
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
