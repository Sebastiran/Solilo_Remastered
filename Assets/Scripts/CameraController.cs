using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float m_StartSpeed = 1.0f;
    [SerializeField]
    private float m_SpeedIncrement = 0.1f;
    [SerializeField]
    private Transform m_Transform;
    [SerializeField]
    private Transform m_StartPoint;

    private float m_Speed;

    private void Awake()
    {
        m_Speed = m_StartSpeed;
    }
    
    private void FixedUpdate()
    {
        m_Transform.position += (Vector3)new Vector2(m_Speed, 0.0f);
        m_Speed += m_SpeedIncrement;
    }

    public void Reset()
    {
        m_Speed = m_StartSpeed;
        m_Transform.position = new Vector3(m_StartPoint.position.x, m_Transform.position.y, m_Transform.position.z);
    }
}
