using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{

    public Image situationalPool;
    public Image generalPool;
    public string currentPool = "situational";
    public bool activePoolAnim = false;
    public bool swapAnimPlaying = false;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapPoolButtonClick()
    {
        if(!swapAnimPlaying)
        {
            activePoolAnim = !activePoolAnim;
            animator.SetBool("ActivePool", activePoolAnim);
        }
    }

    public void SwapPool()
    {
        if(currentPool == "situational")
        {
            currentPool = "general";
            situationalPool.transform.SetAsFirstSibling();
        }
        else if (currentPool == "general")
        {
            currentPool = "situational";
            generalPool.transform.SetAsFirstSibling();
        }
    }

    public void AnimStart()
    {
        swapAnimPlaying = true;
    }
    public void AnimEnd()
    {
        swapAnimPlaying = false;
    }
}
