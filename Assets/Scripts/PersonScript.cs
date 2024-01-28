using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;
    public Material material7;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        int material = Random.Range(1, 8);
        MeshRenderer mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        switch(material)
        {
            case 1:
                mesh.material = material1;
                break;
            case 2:
                mesh.material = material2;
                break;
            case 3:
                mesh.material = material3;
                break;
            case 4:
                mesh.material = material4;
                break;
            case 5:
                mesh.material = material5;
                break;
            case 6:
                mesh.material = material6;
                break;
            case 7:
                mesh.material = material7;
                break;
        }
        StartCoroutine(Idle());
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
    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        animator.SetTrigger("Idle");
    }
}
