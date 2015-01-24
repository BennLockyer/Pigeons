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

    [SerializeField]
    private int m_iGridSize;

	void Start () 
    { 
	
	}

	void Update () 
    {
        m_v3CameraVelocity = Vector3.zero;

        // Get Player
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        HandleCameraBounds(player1);
        HandleCameraBounds(player2);
	}

    void FixedUpdate()
    {
        Debug.Log(m_v3CameraVelocity.ToString());
        m_v3CameraVelocity.Normalize();
        transform.position = transform.position + m_v3CameraVelocity * m_fCameraSpeed * Time.fixedDeltaTime;
    }

    void HandleCameraBounds(GameObject player)
    {
        if (player != null)
        {
            Vector3 topPlayerBound =    camera.WorldToScreenPoint(player.transform.position + Vector3.forward * 1.0f);
            Vector3 bottomPlayerBound = camera.WorldToScreenPoint(player.transform.position + Vector3.back * 1.0f);
            Vector3 rightPlayerBound =  camera.WorldToScreenPoint(player.transform.position + Vector3.right * 1.0f);
            Vector3 leftPlayerBound =   camera.WorldToScreenPoint(player.transform.position + Vector3.left * 1.0f);

            if (topPlayerBound.y > TopCameraBound)
            {
                m_v3CameraVelocity += Vector3.forward;
            }
            if (bottomPlayerBound.y < BottomCameraBound)
            {
                m_v3CameraVelocity += Vector3.back;
            }
            if (rightPlayerBound.x > RightCameraBound)
            {
                m_v3CameraVelocity += Vector3.right;
            }
            if (leftPlayerBound.x < LeftCameraBound)
            {
                m_v3CameraVelocity += Vector3.left;
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
