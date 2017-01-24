using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

   // public float movespeed = 5f;

    //public GameObject playPlane;
    public Transform controller;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        //float x = Input.GetAxis("Mouse X") ;
       // float y = Input.GetAxis("Mouse Y") ;
       // float y = Input.GetAxis("Mouse Y") ;

       // GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(x, 0, y));
        GetComponent<Rigidbody>().MovePosition(controller.position + new Vector3(-0.03f, -0.02f, 0));
        GetComponent<Rigidbody>().MoveRotation(controller.rotation * Quaternion.Euler(180, 0, 0));
        //GetComponent<Rigidbody>().MoveRotation( Quaternion.Euler( 180, 0, 0));

    }
}
