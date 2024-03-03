using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BlockScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public Animator animator;
    public GameObject attachedSlot;
    public GameObject baseSlot;
    public GameObject blockPrefab;
    public SceneLogic sceneLogic;
    public Image colorOverlay;
    public Vector3 origin;
    public TMP_Text text;
    public Image box;
    public Image fill;
    private List<GameObject> collidedSlots = new List<GameObject>();
    private bool isMouseOver = false;
    private bool isDragging = false;
    public bool isMoving = false;
    public Vector3 globalPosition;
    public Vector3 destination = Vector3.zero;
    public Vector3 velocity;
    public float width;
    public float speed;
    public float distance;
    public int type;
    private bool isCollidedBlock;
    public GameObject container;
    public GameObject jokePanel;
    public GameObject mainInterface;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Appear");
        animator.SetBool("Hovering", false);
        origin = destination = transform.localPosition;
    }

    public void Initialize(string text, GameObject container, GameObject jokePanel, GameObject mainInterface, SceneLogic sceneLogic, GameObject baseSlot, int type)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        this.container = container;
        this.jokePanel = jokePanel;
        this.mainInterface = mainInterface;
        this.sceneLogic = sceneLogic;
        this.baseSlot = baseSlot;
        this.type = type;
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        width = text.GetComponent<RectTransform>().sizeDelta.x + 80;
        box.GetComponent<RectTransform>().sizeDelta = fill.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 60);
        GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x, 50);
        GetComponent<BoxCollider2D>().offset = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
    }

    void baseSlotVisible()
    {
        Color typeColor = new Color(0,0,0);
        if(type == 1)
        {
            typeColor = new Color(0.84f, 0.37f, 0.17f, 1);
        }
        else if (type == 2)
        {
            typeColor = new Color(0.38f, 0.67f, 0.18f, 1);
        }
        else if (type == 3)
        {
            typeColor = new Color(0.22f, 0.34f, 0.66f, 1);
        }
        baseSlot.transform.GetChild(0).GetComponent<Image>().color = baseSlot.transform.GetChild(1).GetComponent<TMP_Text>().color = typeColor;
        //animator.enabled = false;
        transform.localScale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        globalPosition = transform.position;
        if (isMouseOver)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                sceneLogic.audioManager.PlaySound("block_out");
                animator.SetBool("Dragging", true);
                isDragging = true;
                if (attachedSlot != null)
                {
                    sceneLogic.wordsPanelAnimator.SetBool("ShowTrashBin", true);
                }
                transform.SetParent(mainInterface.transform, true);
            }
        }
        if(isDragging)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (Input.mousePosition.y < Screen.height / 3 && attachedSlot != null)
                {
                    attachedSlot.GetComponent<SlotScript>().ResetSlot();
                    sceneLogic.audioManager.PlaySound("pop");
                    Destroy(gameObject);
                }
                sceneLogic.wordsPanelAnimator.SetBool("ShowTrashBin", false);
                sceneLogic.wordsPanelAnimator.SetBool("Hovering", false);
                animator.SetBool("Dragging", false);
                if (collidedSlots.Count > 0)
                {
                    bool hasMatchingType = false;
                    for(int i = 0; i < collidedSlots.Count; i++)
                    {
                        if (collidedSlots[i].GetComponent<SlotScript>().type == type)
                        {
                            hasMatchingType = true;
                        }
                        else
                        {
                            collidedSlots.Remove(collidedSlots[i]);
                            i--;
                        }
                    }
                    if(!hasMatchingType)
                    {
                        sceneLogic.audioManager.PlaySound("forbidden");
                    }

                    if (collidedSlots.Count > 1)
                    {
                        collidedSlots = collidedSlots.OrderBy(o => Vector3.Distance(transform.localPosition, o.transform.localPosition)).ToList();
                    }

                    if(collidedSlots.Count > 0)
                    {
                        if (attachedSlot != collidedSlots[0] && attachedSlot != null)
                        {
                            attachedSlot.GetComponent<SlotScript>().ResetSlot();
                        }

                        OccupySlot(collidedSlots[0], true);
                    }
                    else
                    {
                        transform.SetParent(container.transform, true);
                    }

                }
                else
                {
                    transform.SetParent(container.transform, true);
                    sceneLogic.audioManager.PlaySound("click");
                }
                isDragging = false;
                MoveToDestination();
            }
            else blockDrag();

            if(Input.mousePosition.y < Screen.height / 3)
            {
                sceneLogic.wordsPanelAnimator.SetBool("Hovering", true);
            }
            else
            {
                sceneLogic.wordsPanelAnimator.SetBool("Hovering", false);
            }
        }

        if(isMoving)
        {
            BlockMovement();
        }
    }

    public void OccupySlot(GameObject slot, bool playSound)
    {
        if(attachedSlot == null)
        {
            GameObject newBlock = Instantiate(blockPrefab, baseSlot.transform.position, new Quaternion(0, 0, 0, 0), container.transform);
            newBlock.GetComponent<BlockScript>().Initialize(transform.GetChild(1).GetComponent<TMP_Text>().text, container, jokePanel, mainInterface, sceneLogic, baseSlot, type);

            if (slot.GetComponent<SlotScript>().attachedBlock != null)
            {
                Destroy(slot.GetComponent<SlotScript>().attachedBlock);
                sceneLogic.audioManager.PlaySound("swap");
                playSound = false;
            }
        }
        else
        {
            if (slot.GetComponent<SlotScript>().attachedBlock != null && slot.GetComponent<SlotScript>().attachedBlock != gameObject)
            {
                sceneLogic.audioManager.PlaySound("swap");
                playSound = false;
                slot.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().OccupySlot(attachedSlot, false);
            }
        }
        if(playSound) sceneLogic.audioManager.PlaySound("block_in");

        attachedSlot = slot;

        collidedSlots.Remove(attachedSlot);

        container = jokePanel;
        transform.SetParent(container.transform, true);

        slot.GetComponent<SlotScript>().SetContent(gameObject);
        StartCoroutine(sceneLogic.SortJokeBlocks());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        animator.SetBool("Hovering", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        animator.SetBool("Hovering", false);
    }
    private void blockDrag()
    {
        isMoving = false;
        transform.SetAsLastSibling();
        transform.position = Vector3.Lerp(transform.position, new Vector3(Input.mousePosition.x - (width * (Screen.width / 1920f) / 2), Input.mousePosition.y, 0), 0.2f);
    }

    public void MoveToDestination()
    {
        float angle = Mathf.Atan2(destination.y - transform.localPosition.y, destination.x - transform.localPosition.x);
        distance = Vector3.Distance(transform.localPosition, destination);
        speed = distance / 300f;
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * distance / 30f;
        isMoving = true;
    }
    private void BlockMovement()
    {
        float angle = Mathf.Atan2(destination.y - transform.localPosition.y, destination.x - transform.localPosition.x);
        Vector3 velocityDelta = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        
        if((Math.Sign(velocity.x) != Math.Sign(velocityDelta.x) || Math.Sign(velocity.y) != Math.Sign(velocityDelta.y)) && Math.Sign(velocity.x) != 0 && Math.Sign(velocity.y) != 0)
        {
            velocityDelta.x = Mathf.Clamp(Math.Abs(velocityDelta.x) * 7 * speed, -10, Math.Abs(velocity.x)) * Math.Sign(velocityDelta.x);
            velocityDelta.y = Mathf.Clamp(Math.Abs(velocityDelta.y) * 7 * speed, -10, Math.Abs(velocity.y)) * Math.Sign(velocityDelta.y);
        }
        else
        {
            velocityDelta *= speed;
        }

        velocity += velocityDelta;

        transform.localPosition += velocity;

        if(Vector3.Distance(transform.localPosition, destination) < speed + 0.2f)
        {
            origin = destination;
            transform.localPosition = origin;
            isMoving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot" && collision.gameObject != attachedSlot)
        {
            collidedSlots.Add(collision.gameObject);
        }
        else if(collision.gameObject.GetComponent<BlockScript>() != null)
        {
            if (collision.gameObject.GetComponent<BlockScript>().isDragging && attachedSlot != null)
            {
                animator.SetBool("Displaced", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidedSlots.Remove(collision.gameObject);
        animator.SetBool("Displaced", false);
    }

    IEnumerator opacityPlus()
    {
        while(gameObject.transform.localScale.y > 1 && !isCollidedBlock)
        {
            yield return new WaitForEndOfFrame();
            //box.color += new Color(0, 0, 0, 0.1f);
            //text.color += new Color(0, 0, 0, 0.1f);
            gameObject.transform.localScale -= new Vector3(0f, 0.02f, 0f);
        }
    }
    IEnumerator opacityMinus()
    {
        while (gameObject.transform.localScale.y < 1.1f && isCollidedBlock)
        {
            yield return new WaitForEndOfFrame();
            //box.color -= new Color(0, 0, 0, 0.1f);
            //text.color -= new Color(0, 0, 0, 0.1f);
            gameObject.transform.localScale += new Vector3(0f, 0.02f, 0f);
        }
    }
}
