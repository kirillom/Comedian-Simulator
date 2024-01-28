using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
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
    string audience;
    public int score;
    private string[] audiences = { "Teenagers", "Teachers", "Vegeterians", "Parents", "Old people", "Children", "Pet lovers", "Environmental activists", "Programmers", "Bookworms", "Gym bros", "Gamers", "Foodies", "Entrepreneurs", "Artists", "Students" };
    public Dictionary<string, string[]> nounWords = new Dictionary<string, string[]>
    { 
        ["Teenagers"] = new string[] 
        {
            "school", "teenager", "homework", "friend", "my first love", "parents", "nerd"
        },
        ["Teachers"] = new string[]
        {
            "school", "student", "workbook", "textbook", "principal", "teacher"
        },
        ["Vegeterians"] = new string[]
        {
            "food", "apple", "grass", "chicken"
        },
        ["Parents"] = new string[]
        {
            "child", "son", "daughter", "husband", "wife"
        },
        ["Old people"] = new string[]
        {
            "grandpa", "grandma", "man", "grandchild"
        },
        ["Children"] = new string[]
        {
            "parents", "mom", "dad", "toys", "candy", "baby"
        },
        ["Pet lovers"] = new string[]
        {
            "cat", "dog", "dog toys", "parrot", "cow"
        },
        ["Environmental activists"] = new string[]
        {
            "grass", "tree", "garbage", "trash", "trash bin", "climate"
        },
        ["Programmers"] = new string[]
        {
            "computer", "iphone", "Elon Mask", "mouse", "deadline", "code"
        },
        ["Bookworms"] = new string[]
        {
            "book", "reader", "word", "joke"
        },
        ["Gym bros"] = new string[]
        {
            "bro", "protein", "gym", "gym bro", "steroids", "sis"
        },
        ["Gamers"] = new string[]
        {
            "game", "Mario", "Dota 2", "skill issue"
        },
        ["Foodies"] = new string[]
        {
            "food", "chicken nuggets", "fries", "pizza", "burger", "chips", "fish"
        },
        ["Entrepreneurs"] = new string[]
        {
            "money", "principal", "boss", "team", "leader", ""
        },
        ["Artists"] = new string[]
        {
            "artist", "paint", "painting", "van Gogh", "god"
        },
        ["Students"] = new string[]
        {
            "school", "teacher", "professor", "scientist", "lesson", "workbook", "textbook", "homework"
        },
    };
    public Dictionary<string, string[]> verbWords = new Dictionary<string, string[]>
    {
        ["Teenagers"] = new string[]
        {
            "love", "study", "die", "party", "chat", "game", "swear", "bully", "abuse", "hang out"
        },
        ["Teachers"] = new string[]
        {
            "teach", "shout", "ask a question", "assign a homework"
        },
        ["Vegeterians"] = new string[]
        {
            "eat", "meditate", "photosynthesize"
        },
        ["Parents"] = new string[]
        {
            "love", "berate", "die"
        },
        ["Old people"] = new string[]
        {
            "die", "get old", "complain", "grumble"
        },
        ["Children"] = new string[]
        {
            "play", "laugh", "cry", "yell", "eat sweets"
        },
        ["Pet lovers"] = new string[]
        {
            "pet", "walk", "hug"
        },
        ["Environmental activists"] = new string[]
        {
            "protest", "protect nature", "plant a tree"
        },
        ["Programmers"] = new string[]
        {
            "work 24/7", "chill", "code"
        },
        ["Bookworms"] = new string[]
        {
            "read books", "drink tea", "read novels"
        },
        ["Gym bros"] = new string[]
        {
            "workout", "lift weights", "pump muscles", "get covered in oil"
        },
        ["Gamers"] = new string[]
        {
            "play games", "drink energy drinks", "chat", "stream games", "go outside", "touch grass", "get good"
        },
        ["Foodies"] = new string[]
        {
            "eat", "cook", "make a picnic"
        },
        ["Entrepreneurs"] = new string[]
        {
            "make money", "call a meeting", "work 24/7", "drink a lot of coffee"
        },
        ["Artist"] = new string[]
        {
            "go to museum", "paint", "create a masterpiece", "dream"
        },
        ["Students"] = new string[]
        {
            "get drunk", "study", "date", "party", "bully kids"
        },
    };
    public Dictionary<string, string[]> adjectiveWords = new Dictionary<string, string[]>
    {
        ["Teenagers"] = new string[]
        {
            "young", "smart", "emotionally unstable", "angry", "edgy"
        },
        ["Teachers"] = new string[]
        {
            "boring", "angry", "loud"
        },
        ["Vegeterians"] = new string[]
        {
            "green", "healthy", "hungry"
        },
        ["Parents"] = new string[]
        {
            "tired", "loving", "caring"
        },
        ["Old people"] = new string[]
        {
            "tired", "sick", "wise", "calm"
        },
        ["Children"] = new string[]
        {
            "cute", "loud", "pink", "small", "playful", "sweet"
        },
        ["Pet lovers"] = new string[]
        {
            "playful", "cute", "kind", "caring", "loving", "fluffy", "squishy"
        },
        ["Environmental activists"] = new string[]
        {
            "green", "mindful", "brave", "eco-friendly"
        },
        ["Programmers"] = new string[]
        {
            "smart", "tired", "lazy"
        },
        ["Bookworms"] = new string[]
        {
            "interesting", "boring", "inspiring"
        },
        ["Gym bros"] = new string[]
        {
            "strong", "tough", "big", "muscular"
        },
        ["Gamers"] = new string[]
        {
            "nerdy", "skilled", "lonely"
        },
        ["Foodies"] = new string[]
        {
            "fat", "delicious", "yummy", "smelly"
        },
        ["Entrepreneurs"] = new string[]
        {
            "rich", "poor", "nervous", "generous", "smart", "pushing"
        },
        ["Artist"] = new string[]
        {
            "creative", "inspiring", "talented", "beautiful", "amazing"
        },
        ["Students"] = new string[]
        {
            "young", "smart", "hard", "cheerful", "curious", "poor"
        },
    };
    private string[] jokes = { "Why did the /oa /n /v ? Because the /n is a /n !", "Knock! Knock! Who is there? /oa /n !", "What do you call a /a /n ? /oa /n !", "Why do I like to /v ? Because it's /a !", "What is the best /n ? A /a /n !", "How to /v ? I don't know, you better ask /oa /n !", "What did /oa /n say to /oa /n ? /v !" };
    public AudioManager audioManager;
    public Animator cameraAnimator;
    public string finishedJoke;
    public TMP_Text auditoryText;
    //public const Dictionary<string, string[]> themedNouns;

    public bool isDraggingBlock = false;
    // Start is called before the first frame update
    void Start()
    {
        InitializeJoke();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = fps;
        if(Input.GetKeyDown(KeyCode.R))
        {
            
        }

        if(isDraggingBlock)
        {

        }
    }

    public void InitializeJoke()
    {
        score = 0;
        audience = audiences[Random.Range(0, audiences.Length)];
        auditoryText.text = audience;
        cameraAnimator.SetBool("IsZoomed", true);
        monologueBoxText.text = "";
        finishedJoke = "";
        string joke = jokes[Random.Range(0, jokes.Count())];
        StartCoroutine(NewJoke(joke));
    }

    public void CreateWordPool()
    {
        List<string> nouns = new List<string>();
        List<string> verbs = new List<string>();
        List<string> adjectives = new List<string>();

        int nounIndex = Random.Range(0, nounWords[audience].Count());

        nouns.Add(nounWords[audience][nounIndex]);

        int newNounIndex = Random.Range(0, nounWords[audience].Count());
        while (newNounIndex == nounIndex)
        {
            newNounIndex = Random.Range(0, nounWords[audience].Count());
        }
        nouns.Add(nounWords[audience][newNounIndex]);

        for(int i = 0; i < 8; i++)
        {
            string randomAuditory = audiences[Random.Range(0, audience.Count())];

            nouns.Add(nounWords[randomAuditory]
                [
                Random.Range(0, nounWords[randomAuditory].Count())
                ]);
        }


        int verbIndex = Random.Range(0, verbWords[audience].Count());

        verbs.Add(verbWords[audience][verbIndex]);

        int newVerbIndex = Random.Range(0, verbWords[audience].Count());
        while (newVerbIndex == verbIndex)
        {
            newVerbIndex = Random.Range(0, verbWords[audience].Count());
        }
        verbs.Add(verbWords[audience][newVerbIndex]);

        for (int i = 0; i < 8; i++)
        {
            string randomAuditory = audiences[Random.Range(0, audience.Count())];

            verbs.Add(verbWords[randomAuditory]
                [
                Random.Range(0, verbWords[randomAuditory].Count())
                ]);
        }

        int adjectiveIndex = Random.Range(0, adjectiveWords[audience].Count());

        adjectives.Add(adjectiveWords[audience][adjectiveIndex]);

        int newAdjectiveIndex = Random.Range(0, adjectiveWords[audience].Count());
        while (newAdjectiveIndex == adjectiveIndex)
        {
            newAdjectiveIndex = Random.Range(0, adjectiveWords[audience].Count());
        }
        adjectives.Add(adjectiveWords[audience][newAdjectiveIndex]);

        for (int i = 0; i < 8; i++)
        {
            string randomAuditory = audiences[Random.Range(0, audience.Count())];

            adjectives.Add(adjectiveWords[randomAuditory]
                [
                Random.Range(0, adjectiveWords[randomAuditory].Count())
                ]);
        }


        verbs.Add(verbWords[audience][Random.Range(0, verbWords[audience].Count())]);
        verbs.Add(verbWords[audience][Random.Range(0, verbWords[audience].Count())]);

        adjectives.Add(adjectiveWords[audience][Random.Range(0, adjectiveWords[audience].Count())]);
        adjectives.Add(adjectiveWords[audience][Random.Range(0, adjectiveWords[audience].Count())]);



        InstantiateBlockSlots(nounWordsContainer, ref nounWordBlocks, nouns.ToArray(), nounBlockPrefab, 1);
        InstantiateBlockSlots(verbWordsContainer, ref verbWordBlocks, verbs.ToArray(), verbBlockPrefab, 2);
        InstantiateBlockSlots(adjectiveWordsContainer, ref adjectiveWordBlocks, adjectives.ToArray(), adjectiveBlockPrefab, 3);
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
                if (block.GetComponent<SlotScript>().type == 1)
                {
                    if (nounWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
                    {
                        score++;
                    }
                }
                if (block.GetComponent<SlotScript>().type == 2)
                {
                    if (verbWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
                    {
                        score++;
                    }
                }
                if (block.GetComponent<SlotScript>().type == 3)
                {
                    if (adjectiveWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
                    {
                        score++;
                    }
                }
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
        if(score <= 1)
        {
            CrowdBadReaction();
        }
        if (score == 2)
        {
            CrowdNeutralReaction();
        }
        if (score >= 3)
        {
            CrowdGoodReaction();
        }
        yield return new WaitForSeconds(2f);
        interfaceAnimator.SetBool("MonologueBoxOpen", false);
        InitializeJoke();
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
