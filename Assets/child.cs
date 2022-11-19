using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class child : MonoBehaviour
{
    public GameObject childobj;
    // Start is called before the first frame update
    void Start()
    {
        childobj.transform.position = new Vector3(10, 0 ,0); 
    }

    
}
