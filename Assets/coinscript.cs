using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinscript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 pos;
    public GameObject npc;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
        Color c = GetComponent<Renderer>().material.color;
        c.a = 0;
        GetComponent<Renderer>().material.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == npc.tag)
        {
            transform.position = startPosition + Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)) * Vector3.forward * UnityEngine.Random.Range(0f, 100f);
            //transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
            print(npc+"collected");
        }
    }
}
