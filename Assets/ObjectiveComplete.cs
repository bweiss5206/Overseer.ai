using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveComplete : MonoBehaviour
{
    // Start is called before the first frame update
    public void Fade()
{
    Color c = GetComponent<Renderer>().material.color;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
    {
        c.a = alpha;
        GetComponent<Renderer>().material.color = c;
    }
    this.gameObject.tag = "Untagged";
}
}
