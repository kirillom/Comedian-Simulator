using System;
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
    public Vector2 origin;
    public TMP_Text text;
    public Image box;
    private List<GameObject> collidedSlots = new List<GameObject>();
    private bool isMouseOver = false;
    private bool isDragging = false;
    public bool isMoving = false;
    public Vector2 position;
    public Vector2 destination;
    public Vector2 velocity;
    public float speed;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayedStart", 0.01f);
    }

    void DelayedStart()
    {
        box.GetComponent<RectTransform>().sizeDelta = new Vector2(text.GetComponent<RectTransform>().sizeDelta.x + 100, 80);
        GetComponent<BoxCollider2D>().size = new Vector2(box.GetComponent<RectTransform>().sizeDelta.x, 80);
    }

    // Update is called once per frame
    void Update()
    {
        position = GetComponent<RectTransform>().anchoredPosition;
        //destination = origin.GetComponent<RectTransform>().anchoredPosition;

        if (isMouseOver)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
            }
            if(position.y < origin.y + 10 && !isDragging && !isMoving)
            {
                GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 1);
            }
        }
        else
        {
            if (position.y > origin.y && !isDragging && !isMoving)
            {
                GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 1);
            }
        }
        if(isDragging)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(collidedSlots.Count == 1)
                {
                    destination = collidedSlots[0].GetComponent<RectTransform>().anchoredPosition;
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
        //Debug.Log(getAngle(destination, GetComponent<RectTransform>().anchoredPosition));
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
        GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, new Vector2(Input.mousePosition.x - 960, Input.mousePosition.y - 540), 0.2f);
    }

    public void MoveToDestination()
    {
        float angle = Mathf.Atan2(destination.y - position.y, destination.x - position.x);
        distance = Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, destination);
        speed = distance / 300f;
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance / 30f;
        isMoving = true;
    }
    private void BlockMovement()
    {
        float angle = Mathf.Atan2(destination.y - position.y, destination.x - position.x);
        Vector2 velocityDelta = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
        if((Math.Sign(velocity.x) != Math.Sign(velocityDelta.x) || Math.Sign(velocity.y) != Math.Sign(velocityDelta.y)) && Math.Sign(velocity.x) != 0 && Math.Sign(velocity.y) != 0)
        {
            velocityDelta.x = Mathf.Clamp(Math.Abs(velocityDelta.x) * 5 * speed, -10, Math.Abs(velocity.x)) * Math.Sign(velocityDelta.x);
            velocityDelta.y = Mathf.Clamp(Math.Abs(velocityDelta.y) * 5 * speed, -10, Math.Abs(velocity.y)) * Math.Sign(velocityDelta.y);
        }
        else
        {
            velocityDelta *= speed;
        }

        velocity += velocityDelta;

        GetComponent<RectTransform>().anchoredPosition += velocity;

        if(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, destination) < speed + 0.2f)
        {
            origin = destination;
            GetComponent<RectTransform>().anchoredPosition = origin;
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
