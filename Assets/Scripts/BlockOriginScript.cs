using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockOriginScript : MonoBehaviour
{
    public Image box;
    public TMP_Text text;
    public float width;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        width = text.GetComponent<RectTransform>().sizeDelta.x + 80;
        box.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 60);
        //GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x, 80);
        //GetComponent<BoxCollider2D>().offset = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
