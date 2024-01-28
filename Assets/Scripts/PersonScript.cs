using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(Random.Range(0f,1f));
        animator.SetTrigger("Jump");
    }
}
