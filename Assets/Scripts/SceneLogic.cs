using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLogic : MonoBehaviour
{
    public int fps;
    public List<GameObject> jokeBlocks;
    public List<GameObject> wordBlocks;
    public Image panel;
    public Image wordsPanel;
    public GameObject wordPrefab;
    public GameObject slotPrefab;
    public GameObject blockPrefab;
    public GameObject blockOriginPrefab;
    // Start is called before the first frame update
    void Start()
    {
        NewJoke();
        CreateWordPool();
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
                newSlot.GetComponent<SlotScript>().index = jokeBlocks.Count - 1;
            }
            else
            {
                GameObject newWord = Instantiate(wordPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), panel.transform);
                newWord.GetComponent<TMP_Text>().text = word;
                jokeBlocks.Add(newWord);
            }
        }

        Invoke("SortJokeBlocks", 0.01f);
    }

    public void SortJokeBlocks()
    {
        for(int i = 0; i < jokeBlocks.Count; i++)
        {
            GameObject block = jokeBlocks[i];
            Vector3 destination;
            if (i == 0)
            {
                block.GetComponent<SegmentScript>().destination = new Vector3(130, 380, 0);
            }
            else
            {
                Vector3 previousBlockPos = jokeBlocks[i - 1].GetComponent<SegmentScript>().destination;

                destination = new Vector3(previousBlockPos.x + jokeBlocks[i - 1].GetComponent<RectTransform>().sizeDelta.x + 30, previousBlockPos.y, 0);

                block.GetComponent<SegmentScript>().destination = destination;

                if (destination.x + block.GetComponent<RectTransform>().sizeDelta.x > 1700)
                {
                    destination = new Vector3(120, destination.y - 100);
                    block.GetComponent<SegmentScript>().destination = destination;
                }

                if(block.GetComponent<SlotScript>() != null && block.GetComponent<SlotScript>().attachedBlock != null)
                {
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination = destination;
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                }
            }
        }
    }

    public void CreateWordPool()
    {
        string[] words = { "dog", "stupid man", "burning house", "depression" };

        foreach (string word in words)
        {
            GameObject newWord = Instantiate(blockOriginPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), wordsPanel.transform);
            newWord.transform.GetChild(1).GetComponent<TMP_Text>().text = word;
            wordBlocks.Add(newWord);
        }

        Invoke("SortWordBlocks", 0.02f);
    }

    public void SortWordBlocks()
    {
        for (int i = 0; i < wordBlocks.Count; i++)
        {
            GameObject origin = wordBlocks[i];
            if (i == 0)
            {
                origin.transform.position = new Vector3(130, 150, 0);
            }
            else
            {
                GameObject previousorigin = wordBlocks[i - 1];

                origin.transform.position = new Vector3(previousorigin.transform.position.x + previousorigin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x + 30, previousorigin.transform.position.y, 0);

                if (origin.transform.position.x + origin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x > 1700)
                {
                    origin.transform.position = new Vector3(120, origin.transform.position.y - 100);
                }
            }
            GameObject block = Instantiate(blockPrefab, origin.transform.position, new Quaternion(0, 0, 0, 0), wordsPanel.transform);
            block.transform.GetChild(1).GetComponent<TMP_Text>().text = origin.transform.GetChild(1).GetComponent<TMP_Text>().text;
        }
    }
}
