using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatecamera : MonoBehaviour
{
    private float x = -1;
    private float y;
    private float z;
    private Vector3 rotateValue;
    // Start is called before the first frame update
    void Start()
    {

        //this.transform.rotate(0,45,0);
    }

    // Update is called once per frame
    void Update()
    {
        rotateValue = new Vector3(x, y, z);
        transform.eulerAngles = transform.eulerAngles - rotateValue; 
    }
}
