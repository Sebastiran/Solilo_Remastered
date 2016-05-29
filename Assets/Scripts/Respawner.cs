using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField]
    private float m_TerminalHeight = -10.0f;
    [SerializeField]
    private Transform m_SpawnTransform;
    [SerializeField]
    private CameraController m_CameraController;
    [SerializeField]
    private SegmentSpawner m_SegmentSpawner;

	private void Update()
    {
        if(transform.position.y <= m_TerminalHeight)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D a_Other)
    {
        if(a_Other.tag == "Terminal")
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = m_SpawnTransform.position;
        m_CameraController.Reset();
        m_SegmentSpawner.Reset();
    }
}
