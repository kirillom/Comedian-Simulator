using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLogic : MonoBehaviour
{
    public int fps;
    public List<GameObject> jokeBlocks;
    public Image panel;
    public GameObject wordPrefab;
    public GameObject slotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        NewJoke();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = fps;
    }

    public void NewJoke()
    {
        string joke = "What did the \0 say to the \0 ? \0 !";
        //string joke = "Why did the \0 \0 ? Because \0 \0 ! Also some more text to test.";
        string[] separatedWords = joke.Split(" ");

        foreach (string word in separatedWords)
        {
            if(word == "\0")
            {
                GameObject newSlot = Instantiate(slotPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0), panel.transform);
                jokeBlocks.Add(newSlot);
            }
            else
            {
                GameObject newWord = Instantiate(wordPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), panel.transform);
                newWord.GetComponent<TMP_Text>().text = word;
                jokeBlocks.Add(newWord);
            }
        }

        Invoke("SortBlocks", 0.01f);
    }

    public void SortBlocks()
    {
        for(int i = 0; i < jokeBlocks.Count; i++)
        {
            GameObject block = jokeBlocks[i];
            if (i == 0)
            {
                block.GetComponent<RectTransform>().anchoredPosition = new Vector2(70, 120);
            }
            else
            {
                RectTransform previousBlockPos = jokeBlocks[i - 1].GetComponent<RectTransform>();
                block.GetComponent<RectTransform>().anchoredPosition = new Vector2(previousBlockPos.anchoredPosition.x + previousBlockPos.sizeDelta.x + 30, previousBlockPos.anchoredPosition.y);

                if(block.GetComponent<RectTransform>().anchoredPosition.x + block.GetComponent<RectTransform>().sizeDelta.x > 1700)
                {
                    block.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, block.GetComponent<RectTransform>().anchoredPosition.y - 100);
                }
            }
        }
    }
}
