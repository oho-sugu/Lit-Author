using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChangeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.hasChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.hasChanged){
            Debug.Log("Transform changed");
            transform.hasChanged = false;
        }
    }
}
