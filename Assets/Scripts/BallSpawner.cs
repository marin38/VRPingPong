using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

    public GameObject BallPrefub;
    public Transform BallSpawn;
    public float spawnInterval = 3f;
    public float ballTimeLife = 15f;
    public float velocity = 20f; 

    private float nextTimeBallSpawn;
    public Vector3 gravity;


	void Start ()
    {
        nextTimeBallSpawn = Time.time + spawnInterval;
        

    }
	

	void Update ()
    {
        Physics.gravity = gravity;
        if (Time.time > nextTimeBallSpawn)
        {
            nextTimeBallSpawn = Time.time + spawnInterval;
            GameObject ball = (GameObject)Instantiate(BallPrefub, BallSpawn.position, BallSpawn.rotation);
            //ball.GetComponent<Rigidbody>().angularVelocity = new Vector3(10000*Mathf.PI, 10000*Mathf.PI, 10000*Mathf.PI);
            ball.GetComponent<Rigidbody>().velocity = BallPrefub.transform.forward * velocity;
            Destroy(ball, ballTimeLife);
        }
	}
}
