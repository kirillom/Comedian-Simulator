using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{
    public Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
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
