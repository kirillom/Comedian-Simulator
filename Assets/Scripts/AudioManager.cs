using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
