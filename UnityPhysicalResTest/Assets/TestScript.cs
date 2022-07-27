using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dir = transform.TransformDirection(new Vector3(1,1,0));
            Vector3 point = transform.position + dir;

            Debug.Log(point);
        }
    }
}
