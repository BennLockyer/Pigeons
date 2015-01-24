using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    private CameraBehaviour m_pCameraBehaviour;
    [SerializeField]
    private float m_fMoveSpeed;
    private Vector3 m_v3MoveDirection;
	
	private string hMovement;
	private string vMovement;
	
    void Awake()
    {
    }

	void Start () 
    {
        m_pCameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
        if(gameObject.tag == "Player1")
        {
        	hMovement = "Horizontal1";
        	vMovement = "Vertical1";
        }
        if(gameObject.tag == "Player2")
        {
        	hMovement = "Horizontal2";
        	vMovement = "Vertical2";
        }
	}

	void Update () 
    {
    	//maybe use 'getaxis' to cater for more than 8-way direction
    	Vector3 temp = new Vector3(transform.position.x + (2 * Input.GetAxisRaw(hMovement)),transform.position.y,transform.position.z + (2*Input.GetAxisRaw(vMovement)));
    	float distance = Vector3.Distance(transform.position,temp);
    	
    	
    	if(distance > 1.0f)
    	{
    		transform.LookAt(temp);
			float angle = Vector3.Angle(transform.forward,temp-transform.position);
			
 			if(angle < 10.0f)
 			{
		    	RaycastHit hit1;
		    	if(!Physics.Raycast(transform.position,transform.forward,out hit1,1.0f))
		    	{
		    		transform.Translate(Vector3.forward * m_fMoveSpeed * Time.deltaTime);
		    	}
	    	}
	    }
//        m_v3MoveDirection = Vector3.zero;
	    // Key Input -- Change Later
//        if (Input.GetKey(KeyCode.W) == true && IsPlayerOutOfTopBound() == false)
//        {
//            m_v3MoveDirection += Vector3.forward;
//        }
//
//        if (Input.GetKey(KeyCode.S) == true && IsPlayerOutOfBottomBound() == false)
//        {
//            m_v3MoveDirection += Vector3.back;
//        }
//
//        if (Input.GetKey(KeyCode.A) == true && IsPlayerOutOfLeftBound() == false)
//        {
//            m_v3MoveDirection += Vector3.left;
//        }
//
//        if (Input.GetKey(KeyCode.D) == true && IsPlayerOutOfRightBound() == false)
//        {
//            m_v3MoveDirection += Vector3.right;
//        }
	}

//    void FixedUpdate()
//    {
//        m_v3MoveDirection.Normalize();
//        rigidbody.MovePosition(rigidbody.position + m_v3MoveDirection * m_fMoveSpeed * Time.fixedDeltaTime);
//    }

    public bool IsPlayerOutOfTopBound()
    {
        Vector3 topPlayerBound = Camera.main.WorldToScreenPoint(transform.position + Vector3.forward * 1.0f);
        if (topPlayerBound.y >= m_pCameraBehaviour.TopCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfBottomBound()
    {
        Vector3 bottomPlayerBound = Camera.main.WorldToScreenPoint(transform.position + Vector3.back * 1.0f);
        if (bottomPlayerBound.y <= m_pCameraBehaviour.BottomCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfRightBound()
    {
        Vector3 rightPlayerBound = Camera.main.WorldToScreenPoint(transform.position + Vector3.right * 1.0f);
        if (rightPlayerBound.x >= m_pCameraBehaviour.RightCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfLeftBound()
    {
        Vector3 leftPlayerBound = Camera.main.WorldToScreenPoint(transform.position + Vector3.left * 1.0f);
        if (leftPlayerBound.x <= m_pCameraBehaviour.LeftCameraBound)
            return true;
        return false;
    }
}
