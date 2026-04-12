using UnityEngine;

public class SFX : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip actionClip;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayActionSound() {
        if (audioSource != null && actionClip != null)
            audioSource.PlayOneShot(actionClip);
    }

    public void pauseAudio() {
        if (audioSource != null)
            audioSource.Pause();
    }
}