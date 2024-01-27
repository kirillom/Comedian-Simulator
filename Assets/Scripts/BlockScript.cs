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
    public Vector3 destination;
    public Vector3 velocity;
    public float width;
    public float speed;
    public float distance;
    private bool isCollidedBlock;
    public GameObject container;
    public GameObject jokePanel;
    public GameObject mainInterface;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Appear");
        animator.SetBool("Hovering", false);
        origin = destination = transform.position;
    }

    public void Initialize(string text, GameObject container, GameObject jokePanel, GameObject mainInterface, SceneLogic sceneLogic, GameObject baseSlot)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().text = text;
        this.container = container;
        this.jokePanel = jokePanel;
        this.mainInterface = mainInterface;
        this.sceneLogic = sceneLogic;
        this.baseSlot = baseSlot;
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
        baseSlot.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, 1);
        baseSlot.transform.GetChild(1).GetComponent<TMP_Text>().color += new Color(0, 0, 0, 1);
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
                if (Input.mousePosition.y < 350 && attachedSlot != null)
                {
                    attachedSlot.GetComponent<SlotScript>().ResetSlot();
                    sceneLogic.audioManager.PlaySound("pop");
                    Destroy(gameObject);
                }
                sceneLogic.wordsPanelAnimator.SetBool("ShowTrashBin", false);
                sceneLogic.wordsPanelAnimator.SetBool("Hovering", false);
                if (collidedSlots.Count > 0)
                {
                    if(collidedSlots.Count > 1)
                    {
                        collidedSlots = collidedSlots.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).ToList();
                    }
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
                isDragging = false;
                MoveToDestination();
            }
            else blockDrag();

            if(Input.mousePosition.y < 350)
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

        if(Input.GetKeyDown(KeyCode.C))
        {
            
        }
    }

    public void OccupySlot(GameObject slot, bool playSound)
    {
        if(attachedSlot == null)
        {
            GameObject newBlock = Instantiate(blockPrefab, baseSlot.transform.position, new Quaternion(0, 0, 0, 0), container.transform);
            newBlock.GetComponent<BlockScript>().Initialize(transform.GetChild(1).GetComponent<TMP_Text>().text, container, jokePanel, mainInterface, sceneLogic, baseSlot);

            if (slot.GetComponent<SlotScript>().attachedBlock != null)
            {
                Destroy(slot.GetComponent<SlotScript>().attachedBlock);
            }
        }
        else
        {
            if (slot.GetComponent<SlotScript>().attachedBlock != null && slot.GetComponent<SlotScript>().attachedBlock != gameObject)
            {
                sceneLogic.audioManager.PlaySound("swap");
                playSound = false;
                slot.GetComponent<SlotScript>().attachedBlock.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, 1);
                slot.GetComponent<SlotScript>().attachedBlock.transform.GetChild(1).GetComponent<TMP_Text>().color += new Color(0, 0, 0, 1);
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
        //destination = Input.mousePosition;
        isMoving = false;
        transform.SetAsLastSibling();
        transform.position = Vector3.Lerp(transform.position, new Vector3(Input.mousePosition.x - (width / 2), Input.mousePosition.y, 0), 0.2f);
    }

    public void MoveToDestination()
    {
        float angle = Mathf.Atan2(destination.y - transform.position.y, destination.x - transform.position.x);
        distance = Vector3.Distance(transform.position, destination);
        speed = distance / 300f;
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * distance / 30f;
        isMoving = true;
    }
    private void BlockMovement()
    {
        float angle = Mathf.Atan2(destination.y - transform.position.y, destination.x - transform.position.x);
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

        transform.position += velocity;

        if(Vector3.Distance(transform.position, destination) < speed + 0.2f)
        {
            origin = destination;
            transform.position = origin;
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
