using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokePanelScript : MonoBehaviour
{
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RectTransform>().sizeDelta.y != height)
        {
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, new Vector2(1800, height), 0.2f);
        }
    }
}
