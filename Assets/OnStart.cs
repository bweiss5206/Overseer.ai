using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    public GameObject SimpleAgent;
    // Start is called before the first frame update
    public void Start()
    {
        
         // Reset agent position, rotation
        
             var clones = GameObject.FindGameObjectsWithTag("clone");
             foreach (var clone in clones){
                      Destroy(clone);
                 }
         
         //Reset platform position (5 meters away from the agent in a random direction)
         //platform.transform.position = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * Random.Range(0f, 30f);
        for (int i = 0; i<0; ++i){
            
            GameObject a = Instantiate(SimpleAgent, this.transform);
            a.SetActive(true);
            a.tag = "clone";
            print("clone");
        }
        //platform(clone).SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
