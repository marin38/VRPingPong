using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestAngularVelocity : MonoBehaviour {
    public GameObject ball;
    // Use this for initialization
    void Start () {
        ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 1, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
