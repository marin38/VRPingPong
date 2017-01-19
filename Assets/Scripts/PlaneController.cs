using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public float movespeed = 5f;

    public GameObject playPlane;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        float x = Input.GetAxis("Mouse X") * movespeed;
        float y = Input.GetAxis("Mouse Y") * movespeed;

        GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(x, 0, y));
    }
}
