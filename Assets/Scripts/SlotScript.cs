
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    public SceneLogic sceneLogic;
    public GameObject attachedBlock;
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
        slot.color = new Color(1, 1, 1);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        slot.color = new Color(0.9f,0.9f,0.9f);
    }

    public void SetContent(GameObject block)
    {
        attachedBlock = block;
        GetComponent<RectTransform>().sizeDelta = GetComponent<BoxCollider2D>().size = new Vector2(block.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, 80);
        GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<RectTransform>().sizeDelta.x / 2, 0);
        sceneLogic.SortBlocks();
    }

    public void ResetSlot()
    {
        attachedBlock = null;
        GetComponent<RectTransform>().sizeDelta = GetComponent<BoxCollider2D>().size = new Vector2(110,80);
        GetComponent<BoxCollider2D>().offset = new Vector2(55, 0);
        sceneLogic.SortBlocks();
    }
}
