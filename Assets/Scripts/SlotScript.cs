
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    public SceneLogic sceneLogic;
    public GameObject attachedBlock;
    private bool isCollided = false;
    public Image slot;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        sceneLogic = GameObject.FindGameObjectWithTag("Scene Logic").GetComponent<SceneLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollided = true;
        StartCoroutine(opacityMinus());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attachedBlock == null)
        {
            isCollided = false;
            StartCoroutine(opacityPlus());
        }
    }
    IEnumerator opacityPlus()
    {
        while (slot.color.r > 0.9f && !isCollided)
        {
            yield return new WaitForEndOfFrame();
            slot.color -= new Color(0.01f, 0.01f, 0.01f, 0);
        }
    }
    IEnumerator opacityMinus()
    {
        while (slot.color.r < 1f && isCollided)
        {
            yield return new WaitForEndOfFrame();
            slot.color += new Color(0.01f, 0.01f, 0.01f, 0);
        }
    }

    public void SetContent(GameObject block)
    {
        attachedBlock = block;
        slot.color = new Color(1, 1, 1);
        GetComponent<RectTransform>().sizeDelta = GetComponent<BoxCollider2D>().size = new Vector2(block.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, 60);
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<RectTransform>().sizeDelta.x / 2, 0);
    }

    public void ResetSlot()
    {
        attachedBlock = null;
        slot.color = new Color(0.9f, 0.9f, 0.9f);
        GetComponent<RectTransform>().sizeDelta = GetComponent<BoxCollider2D>().size = new Vector2(110,60);
        GetComponent<BoxCollider2D>().offset = new Vector2(55, 0);
        StartCoroutine(sceneLogic.SortJokeBlocks());
    }
}
