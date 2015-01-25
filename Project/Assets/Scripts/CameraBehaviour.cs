using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{
    [SerializeField]
    private float m_fTBound = 10.0f;
    [SerializeField]
    private float m_fBBound = 10.0f;
    [SerializeField]
    private float m_fLBound = 10.0f;
    [SerializeField]
    private float m_fRBound = 10.0f;
    [SerializeField]
    private Vector3 m_v3CameraVelocity;
    [SerializeField]
    private float m_fCameraSpeed = 1.0f;

    private Vector3 m_v3TargetPos;

    // In screen space
    public float TopCameraBound
    {
        get { return transform.position.z + m_fTBound; }
    }
    // In screen space
    public float BottomCameraBound
    {
        get { return transform.position.z - m_fBBound; }
    }
    // In screen space
    public float LeftCameraBound
    {
        get { return transform.position.x - m_fLBound; }
    }
    // In screen space
    public float RightCameraBound
    {
        get { return transform.position.x + m_fRBound; }
    }

    private PlayerBehaviour m_player1Behaviour;
    private PlayerBehaviour m_player2Behaviour;
    private GameObject m_player1Object;
    private GameObject m_player2Object;

	void Start () 
    {
        // Get Player
        m_player1Object = GameObject.FindGameObjectWithTag("Player1");
        m_player2Object = GameObject.FindGameObjectWithTag("Player2");
        m_player1Behaviour = m_player1Object.GetComponent<PlayerBehaviour>();
        m_player2Behaviour = m_player2Object.GetComponent<PlayerBehaviour>();
	}

	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.LoadLevel(0);
        }

        m_v3CameraVelocity = Vector3.zero;

        HandleCameraBounds(m_player1Behaviour, m_player2Behaviour);
        HandleCameraBounds(m_player2Behaviour, m_player1Behaviour);
        Vector3 pos = transform.position;
        Vector3 newPos = pos + m_v3TargetPos;
        Vector3 dir = newPos - pos;
        dir.Normalize();
        m_v3TargetPos = Vector3.zero;
        transform.position = Vector3.Lerp(transform.position, newPos + dir * 0.05f, Time.deltaTime * m_fCameraSpeed);
	}

    void HandleCameraBounds(PlayerBehaviour a_player1, PlayerBehaviour a_player2)
    {
        if (a_player1 != null && a_player2 != null)
        {
            if (a_player1.IsPlayerOutOfTopBound() && !a_player2.IsPlayerOutOfBottomBound())
            {
                m_v3TargetPos.z = (a_player1.transform.position.z + 1.0f)- TopCameraBound;
            }
            if (a_player1.IsPlayerOutOfBottomBound() && !a_player2.IsPlayerOutOfTopBound())
            {
                m_v3TargetPos.z = (a_player1.transform.position.z - 1.0f) - BottomCameraBound;
            }
            if (a_player1.IsPlayerOutOfRightBound() && !a_player2.IsPlayerOutOfLeftBound())
            {
                m_v3TargetPos.x = (a_player1.transform.position.x + 1.0f) - RightCameraBound;
            }
            if (a_player1.IsPlayerOutOfLeftBound() && !a_player2.IsPlayerOutOfRightBound())
            {
                m_v3TargetPos.x = (a_player1.transform.position.x - 1.0f) - LeftCameraBound;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Camera Bounds
        Gizmos.color = Color.red;
        Vector3 tl = Vector3.zero;
        Vector3 tr = Vector3.zero;
        Vector3 bl = Vector3.zero;
        Vector3 br = Vector3.zero;

        tl.x = LeftCameraBound;
        tl.z = TopCameraBound;

        tr.x = RightCameraBound;
        tr.z = TopCameraBound;

        bl.x = LeftCameraBound;
        bl.z = BottomCameraBound;

        br.x = RightCameraBound;    
        br.z = BottomCameraBound;

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(tl, bl);
        Gizmos.DrawLine(tr, br);

        Vector3 targetPos = Vector3.zero;
        GameObject player = GameObject.FindGameObjectWithTag("Player1");
        if (player != null)
        {
            //Gizmos.DrawWireSphere(player.transform.position, 1.0f);
        }

        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player2 != null)
        {
            //Gizmos.DrawWireSphere(player2.transform.position, 1.0f);
        }
    }
}
