using System;
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
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.PostProcessing;
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
    public GameObject gameOverScreen;
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
    public GameObject commentPrefab;
    public GameObject commentsContainer;
    public TMP_Text monologueBoxText;
    public Text gameOverText;
    public Text cringeJoke;
    public Text scoreText;
    public Animator comedianAnimator;
    public Animator gameOverAnimator;
    public Animator wordsPanelAnimator;
    public Animator interfaceAnimator;
    public JokePanelScript jokePanelScript;
    string audience;
    public float jokeScore;
    private int presentScore = 0;
    public int score = 0;
    public bool isLastTry = false;
    public List<GameObject> commentObjects;
    private string[] audiences = { "Teenagers", "Parents", "Children", "Pet lovers", "Conservationists", "Programmers", "Gym bros", "Gamers", "Foodies", "Students", "Content creators", "General" };
    Dictionary<string, string[]> nounWords = new Dictionary<string, string[]>
    {
        ["General"] = new string[]
        {
            "girl", "boy", "god", "woman", "man", "human", "lovecraftian horror beyond comprehension", "Jesus"
        },
        ["Teenagers"] = new string[]
        {
            "teenager", "best friend", "first love", "chick", "idiot", "K E V I N"
        },
        ["Parents"] = new string[]
        {
            "husband", "wife", "mommy", "daddy", "baby"
        },
        ["Children"] = new string[]
        {
            "mommy", "daddy", "dinosaur", "baby", "best friend", "teddy bear"
        },
        ["Pet lovers"] = new string[]
        {
            "cat", "dog", "hamster", "horse"
        },
        ["Conservationists"] = new string[]
        {
            "environmental activist", "vegeterian", "protester"
        },
        ["Programmers"] = new string[]
        {
            "Elon Mask", "programmer", "boss", "gamedev", "software engineer"
        },
        ["Gym bros"] = new string[]
        {
            "gym bro", "man", "bro", "bodybuilder"
        },
        ["Gamers"] = new string[]
        {
            "gamer", "Mario", "noob", "gamedev"
        },
        ["Foodies"] = new string[]
        {
            "chicken nugget", "pizza", "chicken", "pig"
        },
        ["Students"] = new string[]
        {
            "professor", "nerd", "principal"
        },
        ["Content creators"] = new string[]
        {
            "streamer", "tiktoker", "redditor", "discord mod", "MrBeast"
        },
    };
    Dictionary<string, string[]> verbWords = new Dictionary<string, string[]>
    {
        ["General"] = new string[]
        {
            "die", "sin", "get into a car crash", "be gay", "do crimes", "collaborate with Chinese government"
        },
        ["Teenagers"] = new string[]
        {
            "fall in love", "get depressed", "swear uncontrollably", "bully kids", "hang out", "fall into existential crisis", "get drunk", "rizz the skibidi baka in Ohio", "cry from loneliness"
        },
        ["Parents"] = new string[]
        {
            "sleep well", "go on a vacation", "complain about today's news", "marry me", "debate politics"
        },
        ["Children"] = new string[]
        {
            "play with dolls", "laugh uncontrollably", "cry without a reason", "yell at people", "kick me"
        },
        ["Pet lovers"] = new string[]
        {
            "pet somebody without permission", "go on a walk", "hug animals"
        },
        ["Conservationists"] = new string[]
        {
            "protest against the government", "meditate", "photosynthesize", "hibernate"
        },
        ["Programmers"] = new string[]
        {
            "hack pentagon", "go on a vacation", "drink too much coffee", "fix someone else's mistakes", "do nothing"
        },
        ["Gym bros"] = new string[]
        {
            "lift weights", "pump muscles", "get covered in oil", "flex", "take a shower"
        },
        ["Gamers"] = new string[]
        {
            "go outside", "touch grass", "be on copium", "snipe people", "refuse to shower", "wish ill on others"
        },
        ["Foodies"] = new string[]
        {
            "eat too much", "cook pasta", "get overweight"
        },
        ["Students"] = new string[]
        {
            "get drunk", "bully kids", "study math", "skip classes", "do nothing"
        },
        ["Content creators"] = new string[]
        {
            "spread misinformation", "do tiktok dances"
        },
    };
    Dictionary<string, string[]> adjectiveWords = new Dictionary<string, string[]>
    {
        ["General"] = new string[]
        {
            "feminine", "masculine", "non-binary", "mentally ill"
        },
        ["Teenagers"] = new string[]
        {
            "young", "emotionally unstable", "edgy", "depressed", "freaking", "hot", "mentally ill"
        },
        ["Parents"] = new string[]
        {
            "overprotective", "wise", "anxious", "sleep-deprived"
        },
        ["Children"] = new string[]
        {
            "cute", "pink", "small", "playful", "stupid", "angry"
        },
        ["Pet lovers"] = new string[]
        {
            "playful", "cute", "fluffy", "squishy"
        },
        ["Conservationists"] = new string[]
        {
            "green", "carbon-neutral", "eco-friendly", "burning", "dying"
        },
        ["Programmers"] = new string[]
        {
            "smart", "exhausted", "minimum wage", "broke"
        },
        ["Gym bros"] = new string[]
        {
            "strong", "tough", "big", "muscular"
        },
        ["Gamers"] = new string[]
        {
            "better than you", "angry", "unskilled", "toxic"
        },
        ["Foodies"] = new string[]
        {
            "fat", "delicious", "smelly", "hot"
        },
        ["Students"] = new string[]
        {
            "smart", "broke", "boring", "stupid"
        },
        ["Content creators"] = new string[]
        {
            "world-famous", "charismatic", "goofy", "funny"
        },
    };
    private string[] jokes = { "Knock-knock! Who is there? It's me, /oa /n !", "What do you call a /a /n ? A /oa /n !", "Why do I like to /v ? Because it makes me /a !", "What is the best /n ? A /a /n !", "How to /v properly? I don't know, you better ask /oa /n !", "What did the /oa /n say to the /oa /n ? \"Hey, you should /v !\"", "Why doesn't the /oa /n want to /v ? They just think they're too /a for that!", "How to make a /oa /n /v ? Just /v .", "One time I yelled \"Let's /v !\" in public and a /oa /n beat me up.", "If only you knew how hard it is to live with a /oa /n ! They are always trying to /v !", "Life can be hard at times, but it always warms my heart to know there's a /oa /n waiting for me at home.", "How /a do you have to be to work at this job? Just being here makes me /a !", "The other day I saw a /oa /n eating a /oa /n . Man, people are wild these days!", "What to do to make a /oa /n fall in love with you? /v !", "Lately I was having a talk with a /oa /n and I mentioned how I love to /v , but they said it's gross. Am I wrong here?", "Remember lads, never /v with a /oa /n . Last time I tried it was a disaster!", "Whenever I try to talk to any /oa /n they always start to /v . What's wrong with me?", "I asked a girl if she wanted to /v with me. She said I'm gross and should better find a /oa /n for myself.", "So this girl I like said I'm /a and /a . Is she into me bros?", "To /v and then /v - the best evening routine for me." };
    //private string[] jokes = { "So I'm grabbing my daily /n at a McDonald's, right, when this /a /n comes straight to me and just starts to /v . LOL too bad they were so /a they couldn't actually /v but hey when they were done I'm just like \"Hey dude you wanna have this /oa /n ?\" And they were like \"Nah man I better go and /v\" but there ain't no way this kid did me like that so I just basically pull out my /a /n and start throwing it at everyone around, yeah. So yeah this is basically how I ended up in jail." };
    public AudioManager audioManager;
    public Animator cameraAnimator;
    public string finishedJoke;
    public TMP_Text auditoryText;
    public bool isDraggingBlock = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Beginning());
        InitializeJoke();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = fps;
    }

    public IEnumerator Beginning()
    {
        cameraAnimator.SetTrigger("Start");
        audioManager.PlaySound("prelude");
        yield return new WaitForSeconds(3);
        StartCoroutine(audioManager.CrowdStop());
        yield return new WaitForSeconds(1);
        interfaceAnimator.SetBool("MonologueBoxOpen", true);
        yield return new WaitForSeconds(0.5f);
        int i = 0;
        string result = "";
        int soundIterator = 0;
        string joke = "Comedy night starts...\0 Now!";
        while (i < joke.Length)
        {
            if (joke[i] == '\0')
            {
                i++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                result += joke[i];
                i++;
            }
            if (soundIterator == 0)
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
        yield return new WaitForSeconds(2f);
        interfaceAnimator.SetBool("MonologueBoxOpen", false);
        InitializeJoke();
    }

    public void InitializeJoke()
    {
        if (isLastTry) interfaceAnimator.SetTrigger("Warning");
        jokeScore = 0;
        audience = audiences[Random.Range(0, audiences.Length - 1)];
        auditoryText.text = audience;
        cameraAnimator.SetBool("IsZoomed", true);
        finishedJoke = "";
        string joke = jokes[Random.Range(0, jokes.Count())];
        StartCoroutine(NewJoke(joke));
    }

    public void InitializeBlockSlots()
    {
        monologueBoxText.text = "";

        List<string> nouns = new List<string>();
        List<string> verbs = new List<string>();
        List<string> adjectives = new List<string>();

        InstantiateBlockSlots(nounWordsContainer, ref nounWordBlocks, CreateWordPool(nounWords, audience), nounBlockPrefab, 1);
        InstantiateBlockSlots(verbWordsContainer, ref verbWordBlocks, CreateWordPool(verbWords, audience), verbBlockPrefab, 2);
        InstantiateBlockSlots(adjectiveWordsContainer, ref adjectiveWordBlocks, CreateWordPool(adjectiveWords, audience), adjectiveBlockPrefab, 3);

        StartCoroutine(audioManager.MusicDelayedStart());
    }

    public string[] CreateWordPool(Dictionary<string, string[]> wordsDictionary, string audience)
    {
        List<string> result = new List<string>();
        string pendingWord;
        string randomAudience;

        for (int i = 0; i < 2; i++)
        {
            do
            {
                pendingWord = wordsDictionary[audience][Random.Range(0, wordsDictionary[audience].Count())];
            }
            while (IsWordDuplicated(pendingWord, result));

            result.Add(pendingWord);
        }

        for (int i = 0; i < 4; i++)
        {
            do
            {
                randomAudience = audiences[Random.Range(0, audiences.Length)];
                pendingWord = wordsDictionary[randomAudience][Random.Range(0, wordsDictionary[randomAudience].Count())];
            }
            while (IsWordDuplicated(pendingWord, result));

            result.Add(pendingWord);
        }

        return result.OrderBy(x => Random.Range(0, 10000)).ToArray();

        bool IsWordDuplicated(string pendingWord, List<string> result)
        {
            foreach (string word in result)
            {
                if (word == pendingWord)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public IEnumerator NewJoke(string joke)
    {
        yield return new WaitForSeconds(3);
        audioManager.crowdSource.Play();
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
                if(block.GetComponent<SegmentScript>().destination == Vector3.zero)
                {
                    block.GetComponent<SegmentScript>().destination = new Vector2(-830, 130);
                }
                destination = new Vector2(previousBlockPos.x + jokeBlocks[i - 1].GetComponent<SegmentScript>().dimensions.x + 15, previousBlockPos.y);

                if (destination.x + block.GetComponent<SegmentScript>().dimensions.x > 830)
                {
                    destination = new Vector2(-830, destination.y - 70);
                }
                if (i == jokeBlocks.Count - 1)
                {
                    if (destination.y < block.GetComponent<SegmentScript>().destination.y)
                    {
                        jokePanelScript.height += 70;
                    }
                    else if (destination.y > block.GetComponent<SegmentScript>().destination.y)
                    {
                        jokePanelScript.height -= 70;
                    }
                }
                
                block.GetComponent<SegmentScript>().destination = destination;

                if (block.GetComponent<SlotScript>() != null && block.GetComponent<SlotScript>().attachedBlock != null)
                {
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().destination = destination;
                }
            }
        }
        foreach (GameObject gameObject in jokeBlocks)
        {
            if (gameObject.GetComponent<SlotScript>() != null && gameObject.GetComponent<SlotScript>().attachedBlock != null)
            {
                gameObject.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().MoveToDestination();
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
                origin.transform.localPosition = new Vector3(-850, 100, 0);
            }
            else
            {
                GameObject previousOrigin = wordBlocks[i - 1];

                origin.transform.localPosition = new Vector3(previousOrigin.transform.localPosition.x + previousOrigin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x + 25, previousOrigin.transform.localPosition.y, 0);

                if (origin.transform.localPosition.x + origin.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x > 820)
                {
                    origin.transform.localPosition = new Vector3(-850, origin.transform.localPosition.y - 80);
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
        finishedJoke = "";
        int blocksCount = 0;
        foreach (GameObject block in jokeBlocks)
        {
            if(block.tag == "Slot")
            {
                if(block.GetComponent<SlotScript>().attachedBlock != null)
                {
                    finishedJoke += block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text + " ";
                    block.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().isMoving = false;
                    CalculatejokeScore(block);
                    blocksCount++;
                }
                else
                {
                    if (!block.GetComponent<SlotScript>().isSlotOptional)
                    {
                        audioManager.PlaySound("forbidden");
                        foreach (GameObject _block in jokeBlocks)
                        {
                            if (_block.tag == "Slot" && !_block.GetComponent<SlotScript>().isSlotOptional && _block.GetComponent<SlotScript>().attachedBlock == null)
                            {
                                StartCoroutine(_block.GetComponent<SlotScript>().ShakeAnim());
                            }
                        }
                        return;
                    }
                }
            }
            else if (block.tag != "Slot")
            {
                finishedJoke += block.GetComponent<TMP_Text>().text + " ";
            }
        }
        jokeScore = jokeScore / blocksCount;

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
    public void CalculatejokeScore(GameObject block)
    {
        if (block.GetComponent<SlotScript>().type == 1)
        {
            if (nounWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
            {
                jokeScore++;
            }
        }
        if (block.GetComponent<SlotScript>().type == 2)
        {
            if (verbWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
            {
                jokeScore++;
            }
        }
        if (block.GetComponent<SlotScript>().type == 3)
        {
            if (adjectiveWords[audience].Contains(block.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().text))
            {
                jokeScore++;
            }
        }
    }
    IEnumerator Purge()
    {
        yield return new WaitForSeconds(1);
        jokePanelScript.height = -50;
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
        while (i < joke.Length - 1)
        {
            if (joke[i] == '?' || joke[i] == '.' || joke[i] == '!')
            {
                result += joke[i];
                i++;
                monologueBoxText.text = result;
                yield return new WaitForSeconds(0.5f);
            }
            else if (joke[i] == ',')
            {
                result += joke[i];
                i++;
                monologueBoxText.text = result;
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                if (!(joke[i] == ' ' && joke[i + 1] is '?' or '!' or '.' or ','))
                {
                    result += joke[i];
                }
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
        if (jokeScore > 0.5f)
        {
            StartCoroutine(CrowdGoodReaction());
        }
        else if (jokeScore > 0.34f)
        {
            StartCoroutine(CrowdNeutralReaction());
        }
        else
        {
            StartCoroutine(CrowdBadReaction());
        }
    }

    public IEnumerator CrowdBadReaction()
    {
        audioManager.PlaySound("crickets");
        if (!isLastTry)
        {
            isLastTry = true;
            yield return new WaitForSeconds(1);
            for(int i = 0; i < Random.Range(2, 5); i++)
            {
                GameObject newComment = Instantiate(commentPrefab, Vector3.zero, Quaternion.identity, commentsContainer.transform);
                newComment.GetComponent<CommentScript>().SetText(0);
                commentObjects.Add(newComment);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2f);
            interfaceAnimator.SetBool("MonologueBoxOpen", false);
            yield return new WaitForSeconds(2f);
            interfaceAnimator.SetBool("IsLastTry", true);
            InitializeJoke();
        }
        else
        {
            isLastTry = false;
            interfaceAnimator.SetBool("IsLastTry", false);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                GameObject newComment = Instantiate(commentPrefab, Vector3.zero, Quaternion.identity, commentsContainer.transform);
                newComment.GetComponent<CommentScript>().SetText(0);
                commentObjects.Add(newComment);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(2f);
            interfaceAnimator.SetBool("MonologueBoxOpen", false);
            yield return new WaitForSeconds(1f);
            StartCoroutine(Death());
        }
    }
    public IEnumerator CrowdNeutralReaction()
    {
        isLastTry = false;
        interfaceAnimator.SetBool("IsLastTry", false);
        audioManager.PlaySound("crowd_clapping");
        score += Random.Range(100, 500);
        StartCoroutine(UpdateScore());
        yield return new WaitForSeconds(1);
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            GameObject newComment = Instantiate(commentPrefab, Vector3.zero, Quaternion.identity, commentsContainer.transform);
            newComment.GetComponent<CommentScript>().SetText(1);
            commentObjects.Add(newComment);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);
        interfaceAnimator.SetBool("MonologueBoxOpen", false);
        yield return new WaitForSeconds(2);
        InitializeJoke();
    }
    public IEnumerator CrowdGoodReaction()
    {
        isLastTry = false;
        interfaceAnimator.SetBool("IsLastTry", false);
        audioManager.PlaySound("crowd_laughing");
        foreach (Transform person in crowd.transform)
        {
            StartCoroutine(person.gameObject.GetComponent<PersonScript>().Jump());
        }
        score += Random.Range(500, 1000);
        StartCoroutine(UpdateScore());
        yield return new WaitForSeconds(1);
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            GameObject newComment = Instantiate(commentPrefab, Vector3.zero, Quaternion.identity, commentsContainer.transform);
            newComment.GetComponent<CommentScript>().SetText(2);
            commentObjects.Add(newComment);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);
        interfaceAnimator.SetBool("MonologueBoxOpen", false);
        yield return new WaitForSeconds(2);
        InitializeJoke();
    }
    public IEnumerator UpdateScore()
    {
        interfaceAnimator.SetBool("IsScoreVisible", true);
        while(presentScore != score)
        {
            if(score == 0)
            {
                presentScore = Math.Clamp(presentScore - 7, 0, 999999999);
            }
            else
            {
                presentScore = Math.Clamp(presentScore + 7, 0, score);
            }
            scoreText.text = "Earnings: " + Convert.ToString(presentScore) + "$";
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        interfaceAnimator.SetBool("IsScoreVisible", false);
    }
    public IEnumerator TimeRanOut()
    {
        StartCoroutine(audioManager.CrowdStop());
        interfaceAnimator.SetBool("ActivePanel", false);
        StartCoroutine(Purge());
        jokeBlocks.Clear();
        nounWordBlocks.Clear();
        verbWordBlocks.Clear();
        adjectiveWordBlocks.Clear();
        cameraAnimator.SetBool("IsZoomed", false);
        yield return new WaitForSeconds(2);
        StartCoroutine(Death());
    }
    public IEnumerator Death()
    {
        isLastTry = false;
        interfaceAnimator.SetBool("IsLastTry", false);
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                gameOverText.text = "YOU'RE BAD";
                break;
            case 2:
                gameOverText.text = "GET GOOD";
                break;
            case 3:
                gameOverText.text = "COMEDY IS DEAD";
                break;
        }
        random = Random.Range(1, 8);
        switch (random)
        {
            case 1:
                cringeJoke.text = "\"Why did the golfer change his pants?\r\nBecause he got a hole in one!\"";
                break;
            case 2:
                cringeJoke.text = "\"I bought a ceiling fan the other day.\r\nComplete waste of money. He just stands there applauding and saying \"Ooh, I love how smooth it is.\"";
                break;
            case 3:
                cringeJoke.text = "\"What do you call a can opener that doesn’t work?\r\nA can’t opener!\"";
                break;
            case 4:
                cringeJoke.text = "\"Why did the scarecrow win an award?\r\nHe was outstanding in his field.\"";
                break;
            case 5:
                cringeJoke.text = "\"Ever tried to eat a clock?\r\nIt’s time-consuming.\"";
                break;
            case 6:
                cringeJoke.text = "\"Why are cats bad storytellers?\r\nBecause they only have one tale.\"";
                break;
            case 7:
                cringeJoke.text = "\"Two cannibals are eating a clown.\r\nOne says to the other: \"Does this taste funny to you?\"";
                break;
        }
        yield return new WaitForSeconds(1);
        if(score != 0)
        {
            score = 0;
            StartCoroutine(UpdateScore());
        }
        random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                comedianAnimator.SetTrigger("Death1");
                break;
            case 2:
                comedianAnimator.SetTrigger("Death2");
                break;
            case 3:
                comedianAnimator.SetTrigger("Death3");
                break;
        }
        yield return new WaitForSeconds(2);
        gameOverScreen.SetActive(true);
        gameOverAnimator.SetBool("GameOver", true);
        audioManager.PlaySound("death");
    }
    public void Revive()
    {
        audioManager.PlaySound("resurrection");
        StartCoroutine(Revive(true));
    }
    public IEnumerator Revive(bool a)
    {
        yield return new WaitForSeconds(3);
        gameOverScreen.SetActive(false);
        comedianAnimator.SetTrigger("Revive");
        yield return new WaitForSeconds(2);
        audioManager.PlaySound("ahem");
        yield return new WaitForSeconds(1);
        InitializeJoke();
    }
}
