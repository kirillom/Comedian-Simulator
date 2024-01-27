using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokePanelScript : MonoBehaviour
{
    public float height;
    public GameObject readyButton;
    public float readyButtonY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RectTransform>().sizeDelta.y != height)
        {
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, new Vector2(1850, height), 0.2f);
            readyButton.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(readyButton.GetComponent<RectTransform>().anchoredPosition, new Vector2(0, height / 2 - 150), 0.2f);
        }
    }
}
