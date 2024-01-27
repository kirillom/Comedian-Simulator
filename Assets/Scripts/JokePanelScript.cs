using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JokePanelScript : MonoBehaviour
{
    public float height;
    public GameObject readyButton;
    public float readyButtonY;
    public Sprite frame1;
    public Sprite frame2;
    bool flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Flip", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RectTransform>().anchoredPosition.y != height)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, new Vector2(0, height), 0.2f);
            readyButton.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(readyButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(0, height + 60), 0.2f);
        }
    }
    void Flip()
    {
        flipped = !flipped;
        if (flipped) GetComponent<Image>().sprite = frame1;
        else GetComponent<Image>().sprite = frame2;
        Invoke("Flip", 0.2f);
    }
}
