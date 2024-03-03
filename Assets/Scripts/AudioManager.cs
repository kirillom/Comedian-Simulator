using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioSource crowdSource;
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

    public IEnumerator MusicDelayedStart()
    {
        yield return new WaitForSeconds(1);
        musicSource.Play();
    }
    public IEnumerator CrowdStop()
    {
        while(crowdSource.volume > 0)
        {
            crowdSource.volume -= 0.01f;
            if(musicSource.volume > 0) musicSource.volume -= 0.02f;
            yield return new WaitForEndOfFrame();
        }
        crowdSource.Stop();
        musicSource.Stop();
        crowdSource.volume = 1;
        musicSource.volume = 1;
    }
}
