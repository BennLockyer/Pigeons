using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    private CameraBehaviour m_pCameraBehaviour;
    [SerializeField]
    private float m_fMoveSpeed;
    private Vector3 m_v3MoveDirection;
    public Vector3 MoveVelocity
    {
        get { return m_v3MoveDirection * m_fMoveSpeed; }
    }


    void Awake()
    {
    }

	void Start () 
    {
        m_pCameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
	}

	void Update () 
    {
        m_v3MoveDirection = Vector3.zero;
	    // Key Input -- Change Later

        if (gameObject.tag == "Player1")
        {
            if (Input.GetKey(KeyCode.W) == true && IsPlayerOutOfTopBound() == false)
            {
                m_v3MoveDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S) == true && IsPlayerOutOfBottomBound() == false)
            {
                m_v3MoveDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A) == true && IsPlayerOutOfLeftBound() == false)
            {
                m_v3MoveDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D) == true && IsPlayerOutOfRightBound() == false)
            {
                m_v3MoveDirection += Vector3.right;
            }
        }
	}

    void FixedUpdate()
    {
        m_v3MoveDirection.Normalize();
        rigidbody.MovePosition(rigidbody.position + m_v3MoveDirection * m_fMoveSpeed * Time.fixedDeltaTime);
    }

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
