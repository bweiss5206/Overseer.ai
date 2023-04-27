using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityColorByIndex : MonoBehaviour
{
    public Material[] Materials;

    public void SetIndex(int index)
    {
        if (index >= 0 && index < Materials.Length)
        {
            GetComponent<Renderer>().material = Materials[index];
        }
    }
}


