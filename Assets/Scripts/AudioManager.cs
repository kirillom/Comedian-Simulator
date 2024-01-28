using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip blockIn;
    public AudioClip blockOut;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySound(string sound)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + sound));
    }
    public void PlaySound(string sound, float volume)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + sound), volume);
    }

    public void CrowdSpeaking()
    {
        audioSource.volume = 1;
        audioSource.Play();
    }
    public IEnumerator CrowdStop()
    {
        while(audioSource.volume > 0)
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();
        audioSource.volume = 1;
    }
}
