using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    private CameraBehaviour m_pCameraBehaviour;
    [SerializeField]
    private float m_fMoveSpeed;
    [SerializeField]
    private Vector3 m_v3PlayerVelocity;
	
	private string hMovement; 
	private string vMovement;

    private string playerStatus;
	
    void Awake()
    {
        playerStatus = string.Empty;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 1000), playerStatus);
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
    		//transform.LookAt(temp);
    		Vector3 targetDir = temp - transform.position;
    		Vector3 newDir = Vector3.RotateTowards(transform.forward,targetDir,10 * Time.deltaTime,0.0f);
    		transform.rotation = Quaternion.LookRotation(newDir);
			float angle = Vector3.Angle(transform.forward,temp-transform.position);
			
 			if(angle < 45.0f)
 			{
                Vector3 playerVelocity = Vector3.zero;

                RaycastHit hit1;
                // Side raycast
                bool isRightHit = false;
                bool isLeftHit = false;
                if (Physics.Raycast(transform.position, transform.right, out hit1, 1.0f))
                {
                    Debug.Log("right Hit");
                    isRightHit = true;
                    float dist = 0.5f - hit1.distance;
                    playerVelocity += hit1.normal * dist;
                }

                if (Physics.Raycast(transform.position, -transform.right, out hit1, 1.0f))
                {
                    isLeftHit = true;
                    float dist = 0.5f - hit1.distance;
                    playerVelocity += hit1.normal * dist;
                }
                
                if (Physics.Raycast(transform.position, transform.forward, out hit1, 1.0f))
                {
                    Vector3 moveDir = Vector3.zero;
                    float vTest = Vector3.Dot(hit1.normal, Vector3.forward);
                    float hTest = Vector3.Dot(hit1.normal, Vector3.right);

                    float dist = 0.5f - hit1.distance;
                    playerVelocity += hit1.normal * dist;

                    if (!(isLeftHit == true && isRightHit == true))
                    {
                        if (vTest >= 0.95f || vTest <= -0.95f)
                        {
                            moveDir.x = transform.forward.x;
                        }
                        if (hTest >= 0.95f || hTest <= -0.95f)
                        {
                            moveDir.z = transform.forward.z;
                        }

                        playerVelocity += moveDir;
                    }
                }
                else
                {
                    if (!(isLeftHit == true && isRightHit == true))
                        playerVelocity += transform.forward;
                }
                if (CheckBounds() == false)
                {
                    transform.Translate(playerVelocity * m_fMoveSpeed * Time.deltaTime, Space.World);
                }
	    	}
	    }
	}

    bool CheckBounds()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();
        if(IsPlayerOutOfTopBound() && direction.z > 0)
            return true;
        if (IsPlayerOutOfBottomBound() && direction.z < 0)
            return true;
        if (IsPlayerOutOfRightBound() && direction.x > 0)
            return true;
        if (IsPlayerOutOfLeftBound() && direction.x < 0)
            return true;
        return false;
    }

    public bool IsPlayerOutOfTopBound()
    {
        Vector3 topPlayerBound = transform.position + Vector3.forward * 1.0f;
        if (topPlayerBound.z >= m_pCameraBehaviour.TopCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfBottomBound()
    {
        Vector3 bottomPlayerBound = transform.position + Vector3.back * 1.0f;
        if (bottomPlayerBound.z <= m_pCameraBehaviour.BottomCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfRightBound()
    {
        Vector3 rightPlayerBound = transform.position + Vector3.right * 1.0f;
        if (rightPlayerBound.x >= m_pCameraBehaviour.RightCameraBound)
            return true;
        return false;
    }

    public bool IsPlayerOutOfLeftBound()
    {
        Vector3 leftPlayerBound = transform.position + Vector3.left * 1.0f;
        if (leftPlayerBound.x <= m_pCameraBehaviour.LeftCameraBound)
            return true;
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 destPos = transform.position + transform.forward * 0.5f;
        Gizmos.DrawLine(transform.position, destPos);
        Vector3 leftPos = transform.position - transform.right * 0.5f;
        Gizmos.DrawLine(transform.position, leftPos);
        Vector3 rightpos = transform.position + transform.right * 0.5f;
        Gizmos.DrawLine(transform.position, rightpos);
    }
}
