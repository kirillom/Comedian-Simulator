
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
    public Vector2 dimensions = new Vector2(110, 60);
    public bool isSlotOptional;
    public Image slot;
    public int index;
    public int type;
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
        if(attachedBlock == null)
        {
            isCollided = true;
            StartCoroutine(opacityMinus());
        }
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
        while (slot.color.a > 0f && !isCollided)
        {
            slot.color -= new Color(0f, 0f, 0f, 0.005f);
            gameObject.transform.localScale -= new Vector3(0f, 0.02f, 0f);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator opacityMinus()
    {
        while (slot.color.a < 0.03f && isCollided)
        {
            slot.color += new Color(0f, 0f, 0f, 0.005f);
            gameObject.transform.localScale += new Vector3(0f, 0.02f, 0f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetContent(GameObject block)
    {
        isCollided = false;
        attachedBlock = block;
        slot.color = new Color(0, 0, 0, 0);
        dimensions = new Vector2(block.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, 60);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        SetDimensions();
        GetComponent<BoxCollider2D>().size = dimensions;
        GetComponent<BoxCollider2D>().offset = new Vector2(dimensions.x / 2, 0);

        if(isSlotOptional)
        {
            SetOptionalSlotWidth(dimensions.x);
        }
        else
        {
            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = dimensions;
        }
    }

    public void ResetSlot()
    {
        attachedBlock = null;
        slot.color = new Color(0, 0, 0, 0);
        dimensions = new Vector2(110, 60);
        SetDimensions();
        if (isSlotOptional)
        {
            SetOptionalSlotWidth(110);
        }
        else
        {
            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = dimensions;
        }
        GetComponent<BoxCollider2D>().size = dimensions;
        GetComponent<BoxCollider2D>().offset = new Vector2(55, 0);
        StartCoroutine(sceneLogic.SortJokeBlocks());
    }

    void SetDimensions()
    {
        GetComponent<SegmentScript>().dimensions = dimensions;
    }

    void SetOptionalSlotWidth(float width)
    {
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width - 70, 60);
        transform.GetChild(0).GetComponent<RawImage>().uvRect = new Rect(0,0, (width - 70) / 75, 1);
        transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(width, 0);
    }
}
