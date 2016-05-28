using UnityEngine;
using System.Collections;

public class SegmentSpawner : MonoBehaviour 
{
    [SerializeField]
    private Transform[] m_PossibleSegments;
    [SerializeField]
    private float m_SegmentLength = 1.0f;
    [SerializeField]
    private Transform m_CameraTransform;
    [SerializeField]
    private int m_SegmentBufferCount = 5;
    [SerializeField]
    private float m_SpawnTriggerDistance = 0.0f;
    [SerializeField]
    private Transform m_SpawnRoot;

    private Transform[] m_Segments;
    private Vector2 m_NextSpawnPoint;
    private int m_PreviousSegmentIndex = 0;

    private void Start()
    {
        m_Segments = new Transform[m_SegmentBufferCount];
        m_NextSpawnPoint = m_CameraTransform.position;
    }

    private void Update()
    {
        UpdateSegmentSpawn();
    }

    private void UpdateSegmentSpawn()
    {
        if (m_CameraTransform.position.x > m_NextSpawnPoint.x - m_SpawnTriggerDistance)
        {
            SpawnSegment(m_NextSpawnPoint);
            m_NextSpawnPoint.x += m_SegmentLength;
        }
    }

    private void SpawnSegment(Vector2 a_Position)
    {
        int segmentIndex = (++m_PreviousSegmentIndex) % m_SegmentBufferCount;
        if(m_Segments[segmentIndex] != null)
        {
            Destroy(m_Segments[segmentIndex].gameObject);
        }
        int possibleSegmentIndex = Random.Range(0, m_PossibleSegments.Length - 1);
        m_Segments[segmentIndex] = Instantiate(m_PossibleSegments[possibleSegmentIndex]);
        m_Segments[segmentIndex].SetParent(m_SpawnRoot, true);
        m_Segments[segmentIndex].position = a_Position; 
        m_PreviousSegmentIndex = segmentIndex;
    }
}
