using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    enum CharacterAnimationState
    {
        IDLE_STATE = 0,
        RUN_STATE
    };
    private CharacterAnimationState m_charAnimState;

    private CameraBehaviour m_pCameraBehaviour;
    [SerializeField]
    private float m_fMoveSpeed;
    [SerializeField]
    private Vector3 m_v3PlayerVelocity;
	
	private string hMovement; 
	private string vMovement;

    public GameObject annoyParticles;
    private bool canPlay = true;

    private Animator m_charAnimator;
    private AudioManager audioManager;

	void Start () 
    {
    	audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        m_pCameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
        
        m_charAnimator = gameObject.GetComponentInChildren<Animator>();
        if(m_charAnimator == null)
        {
            Debug.Log("Animator NOT FOUND");
        }

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
        m_charAnimState = CharacterAnimationState.IDLE_STATE;
    	if((gameObject.tag == "Player1" && Input.GetButtonDown("Fire1")) || (gameObject.tag == "Player2" && Input.GetButtonDown("Fire2")))
    	{
			if(gameObject.tag == "Player1")
			{
				GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
				p2.GetComponent<PlayerBehaviour>().CheckAnnoy();
			}
			if(gameObject.tag == "Player2")
			{
				GameObject p2 = GameObject.FindGameObjectWithTag("Player1");
				p2.GetComponent<PlayerBehaviour>().CheckAnnoy();
			}
    	}
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
            m_charAnimState = CharacterAnimationState.RUN_STATE;

 			if(angle < 45.0f)
 			{

                Vector3 playerVelocity = Vector3.zero;

                RaycastHit hit1;
                // Side raycast
                  bool isRightHit = false;
                bool isLeftHit = false;
                if (Physics.Raycast(transform.position, transform.right, out hit1, 1.0f))
                {
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
        m_charAnimator.SetInteger("CharacterState", (int)m_charAnimState);
	}
	
	public void CheckAnnoy()
	{
		if(canPlay)
		{
			annoyParticles.GetComponent<ParticleSystem>().Play();
			audioManager.PlayBuzz();
			StartCoroutine("AnnoyCooldown");
		}
	}
	IEnumerator AnnoyCooldown()
	{
		canPlay = false;
		yield return new WaitForSeconds(0.3f);
		canPlay = true;
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
}
