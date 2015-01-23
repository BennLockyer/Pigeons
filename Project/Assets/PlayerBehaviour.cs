using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    void Awake()
    {
    }

	void Start () 
    {
	
	}

	void Update () 
    {
	    // Key Input -- Change Later
        if(Input.GetKey(KeyCode.W) == true)
        {
            rigidbody.MovePosition(rigidbody.position + Vector3.forward * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            rigidbody.MovePosition(rigidbody.position + Vector3.back * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) == true)
        {
            rigidbody.MovePosition(rigidbody.position + Vector3.left * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            rigidbody.MovePosition(rigidbody.position + Vector3.right * Time.deltaTime);
        }
	}

    void FixedUpdate()
    {
    }
}
