using UnityEngine;
using UnityEngine.AI;
using System.Collections;
[RequireComponent(typeof(NavMeshAgent))]

public class RivalController : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_targets = null;
    [SerializeField]
    private float m_destinationThreshold = 0.0f;

    private NavMeshAgent m_navAgent = null;

    private int m_targetIndex = 0;

    private Vector3 CurretTargetPosition
    {
        get
        {
            if (m_targets == null || m_targets.Length <= m_targetIndex)
            {
                return Vector3.zero;
            }

            return m_targets[m_targetIndex].position;
        }
    }

    private void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.destination = CurretTargetPosition;
    }

    private void Update()
    {
        if (m_navAgent.remainingDistance <= m_destinationThreshold)
        {
            m_targetIndex = (m_targetIndex + 1) % m_targets.Length;

            m_navAgent.destination = CurretTargetPosition;
        }
    }

} // class ObjectController


//{public Transform target;
//NavMeshAgent agent;

//void Start()//{
//agent = GetComponent<NavMeshAgent>();}
//void Update(){agent.SetDestination(target.position);}}
