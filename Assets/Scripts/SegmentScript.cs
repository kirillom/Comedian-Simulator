using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{
    public Vector3 destination;
    public Vector2 dimensions;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        dimensions = GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != destination)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, destination, 0.2f);
        }
    }
}
