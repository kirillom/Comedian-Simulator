using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool isPlayingAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimEnd()
    {
        isPlayingAnim = false;
    }
    public void AnimStart()
    {
        isPlayingAnim = true;
    }

}
