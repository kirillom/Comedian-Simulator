using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Image>().color = new Color(1, 1, 1);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
    }
}
