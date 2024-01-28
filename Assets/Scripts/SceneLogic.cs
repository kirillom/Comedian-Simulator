using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class SceneLogic : MonoBehaviour
{
    public int fps;
    public List<GameObject> jokeBlocks;
    public List<GameObject> nounWordBlocks;
    public List<GameObject> verbWordBlocks;
    public List<GameObject> adjectiveWordBlocks;
    public GameObject jokePanel;
    public GameObject blockPanel;
    public GameObject mainInterface;
    public GameObject nounWordsContainer;
    public GameObject verbWordsContainer;
    public GameObject adjectiveWordsContainer;
    public GameObject wordPrefab;
    public GameObject slotPrefab;
    public GameObject optionalSlotPrefab;
    public GameObject nounBlockPrefab;
    public GameObject verbBlockPrefab;
    public GameObject adjectiveBlockPrefab;
    public GameObject blockOriginPrefab;
    public GameObject crowd;
    public TMP_Text monologueBoxText;
    public Animator wordsPanelAnimator;
    public Animator interfaceAnimator;
    public JokePanelScript jokePanelScript;
    public string[] audiences = { "teenagers", "teachers", "bikers", "" };
    public AudioManager audioManager;
    public Animator cameraAnimator;
    public string finishedJoke;
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
            InitializeJoke();
        }

        if(isDraggingBlock)
        {

        }
    }

    public void InitializeJoke()
    {
        cameraAnimator.SetBool("IsZoomed", true);
        monologueBoxText.text = "";
        finishedJoke = "";
        string joke = "Why did the /oa /n /v ? Because /a /n !";
        StartCoroutine(NewJoke(joke));
    }

    public void CreateWordPool()
    {
        string[] nounWords = { "dog", "man", "house", "depression", "moon", "parents", "bike", "chicken nuggets" };
        string[] verbWords = { "burn", "die", "survive", "obey", "ask", "freeze", "study", "do", "make" };
        string[] adjectiveWords = { "very", "hot", "burning", "stupid", "democratic", "dark", "absurd", "dying", "laughing" };
        InstantiateBlockSlots(nounWordsContainer, ref nounWordBlocks, nounWords, nounBlockPrefab, 1);
        InstantiateBlockSlots(verbWordsContainer, ref verbWordBlocks, verbWords, verbBlockPrefab, 2);
        InstantiateBlockSlots(adjectiveWordsContainer, ref adjectiveWordBlocks, adjectiveWords, adjectiveBlockPrefab, 3);
    }

    public IEnumerator NewJoke(string joke)
    {
        yield return new WaitForSeconds(3);
        audioManager.CrowdSpeaking();
        mainInterface.GetComponent<InterfaceScript>().appearAnimPlaying = true;
        //string joke = "What did the \0 say to the \0 ? \0 !";
        //Why didn't the n v ? Because n didn't know how to v !
        string[] separatedWords = joke.Split(" ");

        interfaceAnimator.SetBool("ActivePanel", true);

        foreach (string word in separatedWords)
        {
            if(word == "/n" || word == "/v" || word == "/a")
            {
                GameObject newSlot = Instantiate(slotPrefab, new Vector3(-1000,-1000,0), new Quaternion(0,0,0,0), jokePanel.transform);
                jokeBlocks.Add(newSlot);
                newSlot.GetComponent<SlotScript>().index = jokeBlocks.Count - 1;

                if(word == "/n")
                {
                    newSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.84f, 0.37f, 0.17f, 1);
                    newSlot.GetComponent<SlotScript>().type = 1;
                }
                else if (word == "/v")
                {
                    newSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.38f, 0.67f, 0.18f, 1);
                    newSlot.GetComponent<SlotScript>().type = 2;
                }
                else if (word == "/a")
                {
                    newSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.22f, 0.34f, 0.66f, 1);
                    newSlot.GetComponent<SlotScript>().type = 3;
                }
            }
            else if(word == "/oa")
            {
                GameObject newSlot = Instantiate(optionalSlotPrefab, new Vector3(-1000, -1000, 0), new Quaternion(0, 0, 0, 0), jokePanel.transform);
                newSlot.GetComponent<SlotScript>().isSlotOptional = true;
                jokeBlocks.Add(newSlot);
                newSlot.GetComponent<SlotScript>().index = jokeBlocks.Count - 1;
                newSlot.transform.GetChild(0).GetComponent<RawImage>().color = newSlot.transform.GetChild(1).GetComponent<Image>().color = newSlot.transform.GetChild(2).GetComponent<Image>().color = new Color(57f / 255f, 87f / 255f, 168f / 255f, 1);
                newSlot.GetComponent<SlotScript>().type = 3;
            }
            else
            {
                GameObject newWord = Instantiate(wordPrefab, new Vector3(-1000, -1000, 0), new Quaternion(0, 0, 0, 0), jokePanel.transform);
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
        bool hasUppered = false;
        for (int i = 0; i < jokeBlocks.Count; i++)
        {
            GameObject block = jokeBlocks[i];
            Vector3 destination;
            if (i == 0)
            {
                block.GetComponent<SegmentScript>().destination = new Vector2(-830, 130);
            }
            else
            {
                Vector2 previousBlockPos = jokeBlocks[i - 1].GetComponent<SegmentScript>().destination;

                destination = new Vector2(previousBlockPos.x + jokeBlocks[i - 1].GetComponent<SegmentScript>().dimensions.x + 15, previousBlockPos.y);

                if (destination.x + block.GetComponent<SegmentScript>().dimensions.x > 830)
                {
                    destination = new Vector2(-830, destination.y - 70);
                    //block.GetComponent<SegmentScript>().destination = destination;
                }
                if (destination.y < block.GetComponent<SegmentScript>().destination.y && !hasUppered)
                {
                    bool isThereSimilar = false;
                    foreach (GameObject gameObject in jokeBlocks)
                    {
                        if(gameObject.GetComponent<SegmentScript>().destination.y == destination.y)
                        {
                            isThereSimilar = true;
                        }
                    }
                    if(!isThereSimilar)
                    {
                        hasUppered = true;
                        jokePanelScript.height += 70;
                    }
                }
                else if (destination.y > block.GetComponent<SegmentScript>().destination.y && !hasLowered)
                {
                    hasLowered = true;
                    jokePanelScript.height -= 70;
                }
                block.GetComponent<SegmentScript>().destination = destination;

                if (block.GetComponent<SlotScript>() != null && block.GetComponent<SlotScript>().attachedBlock != null)
                {
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination = block.transform.TransformPoint(Vector3.zero) + (destination - block.transform.localPosition);
                }
            }
        }
        if (hasLowered)
        {
            foreach (GameObject gameObject in jokeBlocks)
            {
                if (gameObject.GetComponent<SlotScript>() != null && gameObject.GetComponent<SlotScript>().attachedBlock != null)
                {
                    gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination -= new Vector3(0, 70, 0);
                    gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                }
            }
        }
        else if (hasUppered)
        {
            foreach (GameObject gameObject in jokeBlocks)
            {
                if (gameObject.GetComponent<SlotScript>() != null && gameObject.GetComponent<SlotScript>().attachedBlock != null)
                {
                    gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination += new Vector3(0, 70, 0);
                    gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                }
            }
        }
        else
        {
            foreach (GameObject gameObject in jokeBlocks)
            {
                if (gameObject.GetComponent<SlotScript>() != null && gameObject.GetComponent<SlotScript>().attachedBlock != null)
                {
                    gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
                }
            }
        }
    }

    public void InstantiateBlockSlots(GameObject container, ref List<GameObject> wordBlocks, string[] words, GameObject blockPrefab, int type)
    {
        foreach (string word in words)
        {
            GameObject newWord = Instantiate(blockOriginPrefab, new Vector3(-1000, -1000, 0), new Quaternion(0, 0, 0, 0), container.transform);
            newWord.transform.GetChild(1).GetComponent<TMP_Text>().text = word;
            wordBlocks.Add(newWord);
        }

        StartCoroutine(SortWordBlocks(container, wordBlocks, blockPrefab, type));
    }

    public IEnumerator SortWordBlocks(GameObject container, List<GameObject> wordBlocks, GameObject blockPrefab, int type)
    {
        for (int i = 0; i < wordBlocks.Count; i++)
        {
            audioManager.PlaySound("xylophone", 0.1f);
            yield return new WaitForSeconds(0.15f);
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
            block.GetComponent<BlockScript>().Initialize(origin.transform.GetChild(1).GetComponent<TMP_Text>().text, container, blockPanel, mainInterface, this, origin, type);
        }
        audioManager.audioSource.pitch = 1;
        mainInterface.GetComponent<InterfaceScript>().appearAnimPlaying = false;
    }

    public void FinishJoke()
    {
        foreach (GameObject block in jokeBlocks)
        {
            if(block.tag == "Slot" && block.GetComponent<SlotScript>().attachedBlock != null)
            {
                finishedJoke += block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text + " ";
            }
            else if (block.tag != "Slot")
            {
                finishedJoke += block.GetComponent<TMP_Text>().text + " ";
            }
        }
        StartCoroutine(audioManager.CrowdStop());
        audioManager.PlaySound("done");
        interfaceAnimator.SetBool("ActivePanel", false);
        cameraAnimator.SetBool("IsZoomed", false);
        StartCoroutine(Purge());
        StartCoroutine(SayJoke(finishedJoke));
        jokeBlocks.Clear();
        nounWordBlocks.Clear();
        verbWordBlocks.Clear();
        adjectiveWordBlocks.Clear();
    }
    IEnumerator Purge()
    {
        yield return new WaitForSeconds(1);
        jokePanelScript.height = 20;
        foreach (Transform child in nounWordsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in verbWordsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in adjectiveWordsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in jokePanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in blockPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    IEnumerator SayJoke(string joke)
    {
        yield return new WaitForSeconds(3);
        interfaceAnimator.SetBool("MonologueBoxOpen", true);
        yield return new WaitForSeconds(0.5f);
        int i = 0;
        string result = "";
        int soundIterator = 0;
        while (i < joke.Length)
        {
            if (joke[i] == '?' || joke[i] == '.' || joke[i] == ',')
            {
                result += joke[i];
                i++;
                monologueBoxText.text = result;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                result += joke[i];
                i++;
            }
            if(soundIterator == 0)
            {
                audioManager.PlaySound("speech", 0.2f);
                audioManager.audioSource.pitch = Random.Range(0.5f, 0.8f);
            }
            soundIterator++;
            if (soundIterator > 2) soundIterator = 0;
            
            monologueBoxText.text = result;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        audioManager.audioSource.pitch = 1;
        audioManager.PlaySound("badumts");
        CrowdGoodReaction();
        yield return new WaitForSeconds(2f);
        interfaceAnimator.SetBool("MonologueBoxOpen", false);
    }

    public void CrowdBadReaction()
    {

    }
    public void CrowdNeutralReaction()
    {
        audioManager.PlaySound("crowd_clapping");
    }
    public void CrowdGoodReaction()
    {
        audioManager.PlaySound("crowd_laughing");
        foreach (Transform person in crowd.transform)
        {
            StartCoroutine(person.gameObject.GetComponent<PersonScript>().Jump());
        }
    }
}
