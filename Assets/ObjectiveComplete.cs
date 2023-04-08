using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveComplete : MonoBehaviour
{
    // Start is called before the first frame update
public void Start()
    {
        this.gameObject.tag = "Target";
        Color c = GetComponent<Renderer>().material.color;
        c.a = 1f;
        GetComponent<Renderer>().material.color = c;
    }
    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a collectible, reward and end episode
        if (other.tag == "Player")
        {
            
            StartCoroutine(Fade());
            print("collide");
            //ObjectiveComplete:Fade(other.gameObject);
        }
    }

    IEnumerator Fade()
{
        print("fade");
            
        yield return new WaitForSeconds(1f);
        this.gameObject.tag = "Untagged";
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
    Color c = GetComponent<Renderer>().material.color;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
    {
        c.a = alpha;
        GetComponent<Renderer>().material.color = c;
        yield return new WaitForSeconds(1f);
    }
}
}
