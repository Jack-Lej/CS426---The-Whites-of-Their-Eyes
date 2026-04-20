using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public float duration = 2;
    public float strength;
    private bool isDissolving = false;
    private Material dissolveMaterial;
    [SerializeField] Character character;

    public void StartDissolve()
    {
        StartCoroutine(Dissolver());
    }

    // public IEnumerator Dissolver()
    // {
    //     if (dissolveMaterial == null)
    //     {
    //         Debug.LogError("Dissolve: dissolveMaterial is null, cannot dissolve.");
    //         yield break;
    //     }

    //     float timeElapsed = 0;
    //     while (timeElapsed < duration)
    //     {
    //         timeElapsed += Time.deltaTime;
    //         strength = Mathf.Lerp(0, 1, timeElapsed / duration);
    //         dissolveMaterial.SetFloat("_visble_amount", strength);
    //         yield return null;
    //     }
        
    // }

    public IEnumerator Dissolver()
    {
        if (dissolveMaterials.Count == 0)
        {
            Debug.LogError("Dissolve: No dissolvable materials found.");
            yield break;
        }

        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            strength = Mathf.Lerp(0, 1, timeElapsed / duration);
            foreach (Material mat in dissolveMaterials)
                mat.SetFloat("_visble_amount", strength);
            yield return null;
        }
    }

    private List<Material> dissolveMaterials = new List<Material>();

    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        foreach (Renderer rend in renderers)
        {
            // Check each material on the renderer (some objects have multiple materials)
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_visble_amount"))
                {
                    dissolveMaterials.Add(mat);
                    Debug.Log("Found dissolvable material on: " + rend.gameObject.name);
                }
            }
        }

        if (dissolveMaterials.Count == 0)
            Debug.LogWarning("Dissolve: No dissolvable materials found on " + gameObject.name + " or its children.");

        character = GetComponent<Character>();
    }
    private void Update()
    {
        if(character != null)
        {
            if(character.currentHealth <= 0 && !isDissolving)
            {
                isDissolving = true;
                Debug.Log("DISSOLVING STARTED!");
                StartDissolve();
            }
        }
    }

    
}
