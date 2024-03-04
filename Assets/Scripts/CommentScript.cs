using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CommentScript : MonoBehaviour
{
    public TMP_Text text;
    public Animator animator;
    public SceneLogic sceneLogic;
    public List<GameObject> commentObjects;
    // Start is called before the first frame update
    void Start()
    {
        sceneLogic = GameObject.FindGameObjectWithTag("Scene Logic").GetComponent<SceneLogic>();
        commentObjects = new List<GameObject>(sceneLogic.commentObjects);
        commentObjects.Remove(gameObject);
        StartCoroutine(DelayedStart());
    }
    IEnumerator DelayedStart()
    {
        bool isCollided = false;
        yield return new WaitForEndOfFrame();
        do
        {
            //this was way overcomplicated
            transform.localPosition = new Vector2(Random.Range(-750, 750), Random.Range(-450, 0));
            float scale = 0.7f + Mathf.Abs(transform.localPosition.y) / 700;
            transform.localScale = new Vector2(scale, scale);
            foreach(GameObject comment in commentObjects)
            {
                if (Vector2.Distance(transform.localPosition, comment.transform.localPosition) < 300)
                {
                    isCollided = true;
                    break;
                }
                else
                {
                    isCollided = false;
                }
            }
        }
        while (isCollided);
        Invoke("Destroy", 6);
    }

    public void SetText(int reaction)
    {
        string[] badComments = new string[] { "This is the worst joke I ever heard.", "Is this even a joke?", "This was terrible.", "I feel offended!", "My mom says you're going to jail for this!", "I might have had a heart attack!", "This is why I hate comedians.", "Am I the only one laughing?!", "I thought we'd stay out of politics!", "Hey! There are children listening!", "I can't believe how unfunny this was.", "Jesus save us.", "May I know where you live?" };
        string[] neutralComments = new string[] { "I guess it's quite good.", "I don't know if I should laugh or cry.", "Keep it up!", "I'm not giving you money.", "Was that a dad joke?", "My pet stone liked that!", "Didn't laugh.", "Way to go son.", "Did my aunt tell you this?", "Give me something funnier.", "I'm actually laughing!", "I shouldn't be laughing.", "Why would you say that!?", "A corny joke.", "I will tell this to my grandma!", "This's quite political, but I like it.", "I want this to be at my funeral.", "My dad would approve of this.", "I'm stealing this joke." };
        string[] goodComments = new string[] { "This is the best joke I ever heard!", "Actually, this is not funny at all.", "We're going to hell for this!", "I can't stop laughing!", "Why is this so funny?!", "The perfect dad joke!", "Oh my gosh!!!", "Marry me.", "HEHEHEHA!!!", "I'm laughing so hard right now!", "This is way too funny!", "I'm telling this everyone!", "Capture this!", "This is comedy gold!", "This cured my lung disease!", "Even my cat laughs!", "You're not legally allowed to be this funny!", "I'm crying tears of joy.", "I want to give you everything I have." };
        switch (reaction)
        {
            case 0:
                text.text = badComments[Random.Range(0, badComments.Length)];
                break;
            case 1:
                text.text = neutralComments[Random.Range(0, neutralComments.Length)];
                break;
            case 2:
                text.text = goodComments[Random.Range(0, goodComments.Length)];
                break;
        }
    }
    void Destroy()
    {
        sceneLogic.commentObjects.Remove(gameObject);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
