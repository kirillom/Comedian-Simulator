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
    public GameObject attachedSlot;
    public SceneLogic sceneLogic;
    public Vector3 origin;
    public TMP_Text text;
    public Image box;
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
    // Start is called before the first frame update
    void Start()
    {
        sceneLogic = GameObject.FindGameObjectWithTag("Scene Logic").GetComponent<SceneLogic>();
        Invoke("DelayedStart", 0.01f);
    }

    void DelayedStart()
    {
        width = text.GetComponent<RectTransform>().sizeDelta.x + 100;
        box.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 80);
        GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x, 80);
        GetComponent<BoxCollider2D>().offset = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        globalPosition = transform.position;
        //destination = origin.transform.position;

        if (isMouseOver)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
            }
            if(transform.position.y < origin.y + 10 && !isDragging && !isMoving)
            {
                transform.position += new Vector3(0, 1, 0);
            }
        }
        else
        {
            if (transform.position.y > origin.y && !isDragging && !isMoving)
            {
                transform.position -= new Vector3(0, 1, 0);
            }
        }
        if(isDragging)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(collidedSlots.Count > 0)
                {
                    if(collidedSlots.Count > 1)
                    {
                        collidedSlots = collidedSlots.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).ToList();
                    }
                    if (attachedSlot != collidedSlots[0] && attachedSlot != null)
                    {
                        attachedSlot.GetComponent<SlotScript>().ResetSlot();
                    }

                    OccupySlot(collidedSlots[0]);
                    
                }
                isDragging = false;
                MoveToDestination();
            }
            else blockDrag();
        }

        if(isMoving)
        {
            BlockMovement();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            
        }
        //Debug.Log(getAngle(destination, transform.position));
    }

    public void OccupySlot(GameObject slot)
    {
        if (slot.GetComponent<SlotScript>().attachedBlock != null && attachedSlot != null && slot.GetComponent<SlotScript>().attachedBlock != gameObject)
        {
            slot.GetComponent<SlotScript>().attachedBlock.GetComponent<BlockScript>().OccupySlot(attachedSlot);
        }

        attachedSlot = slot;

        destination = slot.transform.position;
        slot.GetComponent<SlotScript>().SetContent(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
    private void blockDrag()
    {
        //destination = Input.mousePosition;
        isMoving = false;
        transform.position = Vector2.Lerp(transform.position, new Vector2(Input.mousePosition.x - (width / 2), Input.mousePosition.y), 0.2f);
    }

    public void MoveToDestination()
    {
        float angle = Mathf.Atan2(destination.y - transform.position.y, destination.x - transform.position.x);
        distance = Vector2.Distance(transform.position, destination);
        speed = distance / 300f;
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance / 30f;
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

        if(Vector2.Distance(transform.position, destination) < speed + 0.2f)
        {
            origin = destination;
            transform.position = origin;
            isMoving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidedSlots.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidedSlots.Remove(collision.gameObject);
    }
}
