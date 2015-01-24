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

    // In screen space
    public float TopCameraBound
    {
        get { return Screen.height / 2 + m_fTBound; }
    }
    // In screen space
    public float BottomCameraBound
    {
        get { return Screen.height / 2 - m_fBBound; }
    }
    // In screen space
    public float LeftCameraBound
    {
        get { return Screen.width / 2 - m_fLBound; }
    }
    // In screen space
    public float RightCameraBound
    {
        get { return Screen.width / 2 + m_fRBound; }
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
        m_v3CameraVelocity = Vector3.zero;

        HandleCameraBounds(m_player1Behaviour, m_player2Behaviour);
        HandleCameraBounds(m_player2Behaviour, m_player1Behaviour);

        transform.position = transform.position + m_v3CameraVelocity * m_fCameraSpeed * Time.deltaTime;
	}

    void HandleCameraBounds(PlayerBehaviour a_player1, PlayerBehaviour a_player2)
    {
        if (a_player1 != null && a_player2 != null)
        {
            if (a_player1.IsPlayerOutOfTopBound() && !a_player2.IsPlayerOutOfBottomBound())
            {
                if (a_player1.transform.forward.z > 0.0f)
                    m_v3CameraVelocity.z += a_player1.transform.forward.z;
            }
            if (a_player1.IsPlayerOutOfBottomBound() && !a_player2.IsPlayerOutOfTopBound())
            {
                if(a_player1.transform.forward.z < 0.0f)
                    m_v3CameraVelocity.z += a_player1.transform.forward.z;
            }
            if (a_player1.IsPlayerOutOfRightBound() && !a_player2.IsPlayerOutOfLeftBound())
            {
                if (a_player1.transform.forward.x > 0.0f)
                    m_v3CameraVelocity.x += a_player1.transform.forward.x;
            }
            if (a_player1.IsPlayerOutOfLeftBound() && !a_player2.IsPlayerOutOfRightBound())
            {
                if (a_player1.transform.forward.x < 0.0f)
                    m_v3CameraVelocity.x += a_player1.transform.forward.x;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Camera Bounds
        Gizmos.color = Color.red;
        Vector3 tl = Vector3.one;
        Vector3 tr = Vector3.one;
        Vector3 bl = Vector3.one;
        Vector3 br = Vector3.one;

        tl.x = LeftCameraBound;
        tl.y = TopCameraBound;

        tr.x = RightCameraBound;
        tr.y = TopCameraBound;

        bl.x = LeftCameraBound;
        bl.y = BottomCameraBound;

        br.x = RightCameraBound;
        br.y = BottomCameraBound;

        tl = camera.ScreenToWorldPoint(tl);
        tr = camera.ScreenToWorldPoint(tr);
        bl = camera.ScreenToWorldPoint(bl);
        br = camera.ScreenToWorldPoint(br);

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(tl, bl);
        Gizmos.DrawLine(tr, br);

        GameObject player = GameObject.FindGameObjectWithTag("Player1");
        if (player != null)
        {
            Vector3 playerScreenPos = camera.WorldToScreenPoint(player.transform.position);
            Gizmos.DrawWireSphere(camera.ScreenToWorldPoint(playerScreenPos), 1.0f);
        }

        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player2 != null)
        {
            Vector3 playerScreenPos = camera.WorldToScreenPoint(player2.transform.position);
            Gizmos.DrawWireSphere(camera.ScreenToWorldPoint(playerScreenPos), 1.0f);
        }
    }
}
