using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLogic : MonoBehaviour
{
    public int fps;
    public List<GameObject> jokeBlocks;
    public List<GameObject> nounWordBlocks;
    public List<GameObject> verbWordBlocks;
    public List<GameObject> adjectiveWordBlocks;
    public GameObject jokePanel;
    public GameObject mainInterface;
    public GameObject nounWordsContainer;
    public GameObject verbWordsContainer;
    public GameObject adjectiveWordsContainer;
    public GameObject wordPrefab;
    public GameObject slotPrefab;
    public GameObject optionalSlotPrefab;
    public GameObject blockPrefab;
    public GameObject blockOriginPrefab;
    public Animator wordsPanelAnimator;
    public Animator interfaceAnimator;
    public JokePanelScript jokePanelScript;
    public string[] audiences = { "teenagers", "teachers", "bikers", "" };
    public AudioManager audioManager;
    //public const Dictionary<string, string[]> themedNouns;

    public bool isDraggingBlock = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = fps;
        if(Input.GetKeyDown(KeyCode.R))
        {
            //what do you call a _ _ ? _ !
            string joke = "Why did the /n /n ? Because /o /o ! Also some more text to test.";
            NewJoke(joke);
        }

        if(isDraggingBlock)
        {

        }
    }

    public void CreateWordPool()
    {
        string[] nounWords = { "dog", "man", "house", "depression", "moon", "parents", "bike", "chicken nuggets" };
        string[] verbWords = { "burn", "die", "survive", "obey", "ask", "freeze", "study", "do", "make" };
        string[] adjectiveWords = { "very", "hot", "burning", "stupid", "democratic", "dark", "absurd", "dying", "laughing" };
        InstantiateBlockSlots(nounWordsContainer, ref nounWordBlocks, nounWords);
        InstantiateBlockSlots(verbWordsContainer, ref verbWordBlocks, verbWords);
        InstantiateBlockSlots(adjectiveWordsContainer, ref adjectiveWordBlocks, adjectiveWords);
    }

    public void NewJoke(string joke)
    {
        mainInterface.GetComponent<InterfaceScript>().appearAnimPlaying = true;
        //string joke = "What did the \0 say to the \0 ? \0 !";
        string[] separatedWords = joke.Split(" ");

        interfaceAnimator.SetBool("ActivePanel", true);

        foreach (string word in separatedWords)
        {
            if(word == "/n")
            {
                GameObject newSlot = Instantiate(slotPrefab, new Vector3(-100,-100,0), new Quaternion(0,0,0,0), jokePanel.transform);
                jokeBlocks.Add(newSlot);
                newSlot.GetComponent<SlotScript>().index = jokeBlocks.Count - 1;
            }
            else if(word == "/o")
            {
                GameObject newSlot = Instantiate(optionalSlotPrefab, new Vector3(-100, -100, 0), new Quaternion(0, 0, 0, 0), jokePanel.transform);
                newSlot.GetComponent<SlotScript>().isSlotOptional = true;
                jokeBlocks.Add(newSlot);
                newSlot.GetComponent<SlotScript>().index = jokeBlocks.Count - 1;
            }
            else
            {
                GameObject newWord = Instantiate(wordPrefab, new Vector3(-100, -100, 0), new Quaternion(0, 0, 0, 0), jokePanel.transform);
                newWord.GetComponent<TMP_Text>().text = word;
                jokeBlocks.Add(newWord);
            }
        }

        StartCoroutine(SortJokeBlocks());
    }

    public IEnumerator SortJokeBlocks()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        bool hasLowered = false;
        for (int i = 0; i < jokeBlocks.Count; i++)
        {
            GameObject block = jokeBlocks[i];
            Vector3 destination;
            if (i == 0)
            {
                block.GetComponent<SegmentScript>().destination = new Vector2(-830, 70);
            }
            else
            {
                Vector3 previousBlockPos = jokeBlocks[i - 1].GetComponent<SegmentScript>().destination;

                destination = new Vector3(previousBlockPos.x + jokeBlocks[i - 1].GetComponent<SegmentScript>().dimensions.x + 15, previousBlockPos.y);

                if (destination.x + block.GetComponent<SegmentScript>().dimensions.x > 830)
                {
                    jokePanelScript.height += 120;
                    for(int j = 0; j < i; j++)
                    {
                        Vector3 oldDestination = jokeBlocks[j].GetComponent<SegmentScript>().destination;
                        oldDestination = new Vector2(oldDestination.x, oldDestination.y + 65);
                        jokeBlocks[j].GetComponent<SegmentScript>().destination = oldDestination;
                        if (jokeBlocks[j].GetComponent<SlotScript>() != null && jokeBlocks[j].GetComponent<SlotScript>().attachedBlock != null)
                        {
                            jokeBlocks[j].GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination = jokeBlocks[j].transform.TransformPoint(Vector3.zero) + (oldDestination - jokeBlocks[j].transform.localPosition);
                            jokeBlocks[j].GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                        }
                    }
                    destination = new Vector3(-830, destination.y);
                }
                else if (destination.y < block.GetComponent<SegmentScript>().destination.y && !hasLowered)
                {
                    hasLowered = true;
                    jokePanelScript.height -= 120;
                }

                block.GetComponent<SegmentScript>().destination = destination;

                Debug.Log(block.GetComponent<SegmentScript>().destination.y);
                Debug.Log(block.transform.localPosition.y);

                if (block.GetComponent<SlotScript>() != null && block.GetComponent<SlotScript>().attachedBlock != null)
                {
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination = block.transform.TransformPoint(Vector3.zero) + (destination - block.transform.localPosition);
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                }
            }
        }
    }

    public void InstantiateBlockSlots(GameObject container, ref List<GameObject> wordBlocks, string[] words)
    {
        foreach (string word in words)
        {
            GameObject newWord = Instantiate(blockOriginPrefab, new Vector3(-100, -100, 0), new Quaternion(0, 0, 0, 0), container.transform);
            newWord.transform.GetChild(1).GetComponent<TMP_Text>().text = word;
            wordBlocks.Add(newWord);
        }

        StartCoroutine(SortWordBlocks(container, wordBlocks));
    }

    public IEnumerator SortWordBlocks(GameObject container, List<GameObject> wordBlocks)
    {
        for (int i = 0; i < wordBlocks.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject origin = wordBlocks[i];
            if (i == 0)
            {
                origin.transform.position = new Vector3(110, 280, 0);
            }
            else
            {
                GameObject previousOrigin = wordBlocks[i - 1];

                origin.transform.position = new Vector3(previousOrigin.transform.position.x + previousOrigin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x + 25, previousOrigin.transform.position.y, 0);

                if (origin.transform.position.x + origin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x > 1700)
                {
                    origin.transform.position = new Vector3(110, origin.transform.position.y - 80);
                }
            }
            GameObject block = Instantiate(blockPrefab, origin.transform.position, new Quaternion(0, 0, 0, 0), container.transform);
            block.GetComponent<BlockScript>().Initialize(origin.transform.GetChild(1).GetComponent<TMP_Text>().text, container, jokePanel, mainInterface, this, origin);
        }
        mainInterface.GetComponent<InterfaceScript>().appearAnimPlaying = false;
    }
}
