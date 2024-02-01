using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{

    public Image nounPool;
    public Image verbPool;
    public Image adjectivePool;
    public Image colorOverlay;
    public Image swapButtonIcon;
    //1 - noun 2 - verb 3 - adjective
    public int currentPool = 1;
    public bool swapAnimPlaying = false;
    public bool appearAnimPlaying = false;
    public SceneLogic sceneLogic;
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
        if(!swapAnimPlaying && !appearAnimPlaying)
        {
            sceneLogic.audioManager.PlaySound("shuffle");
            currentPool++;
            if (currentPool > 3) currentPool = 1;

            foreach(GameObject block in GameObject.FindGameObjectsWithTag("Block"))
            {
                if(block.GetComponent<BlockScript>().isMoving)
                {
                    block.GetComponent<BlockScript>().isMoving = false;
                    block.transform.position = block.GetComponent<BlockScript>().destination;
                }
            }
            animator.SetInteger("ActivePool", currentPool);
            animator.SetTrigger("SwapButtonPressed");
        }
    }

    public void SwapPool()
    {
        switch (currentPool)
        {
            case 1:
                nounPool.transform.SetAsLastSibling();
                swapButtonIcon.sprite = Resources.Load<Sprite>("Sprites/Person_icon");
                break;
            case 2:
                verbPool.transform.SetAsLastSibling();
                swapButtonIcon.sprite = Resources.Load<Sprite>("Sprites/Verb_icon");
                break;
            case 3:
                adjectivePool.transform.SetAsLastSibling();
                swapButtonIcon.sprite = Resources.Load<Sprite>("Sprites/Star_icon");
                break;
        }
        colorOverlay.transform.SetAsLastSibling();
    }

    public void PoolAnimStart()
    {
        swapAnimPlaying = true;
    }
    public void PoolAnimEnd()
    {
        swapAnimPlaying = false;
        animator.SetInteger("ActivePool", 0);
    }

    public void PanelAppearAnimEnd()
    {
        sceneLogic.InitializeBlockSlots();
    }
    public void PanelDisappearAnimEnd()
    {

    }

    public void MouseOverReadyButton()
    {
        animator.SetBool("ReadyButtonHovered", true);
    }
    public void MouseOutReadyButton()
    {
        animator.SetBool("ReadyButtonHovered", false);
    }
}
