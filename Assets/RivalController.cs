using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]



public class RivalController : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_targets = null;
    [SerializeField]
    private float m_destinationThreshold = 0.0f;

    private NavMeshAgent m_navAgent = null;

    private int m_targetIndex = 0;
    public int rivalgoalcount = 0;
    //ゲーム終了時に表示するテキスト（追加）
    private GameObject stateText;

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
        //シーン中のstateTextオブジェクトを取得（追加）
        this.stateText = GameObject.Find("GameResultText");
    }

    private void Update()
    {
        if (m_navAgent.remainingDistance <= m_destinationThreshold)
        {
            m_targetIndex = (m_targetIndex + 1) % m_targets.Length;

            m_navAgent.destination = CurretTargetPosition;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "gp")
        {
            if (rivalgoalcount == 0)
            {
                rivalgoalcount += 1;
            }
            if (rivalgoalcount == 1)
            {
                rivalgoalcount += 1;
            }
        }
    }
}
 // class ObjectController


//{public Transform target;
//NavMeshAgent agent;

//void Start()//{
//agent = GetComponent<NavMeshAgent>();}
//void Update(){agent.SetDestination(target.position);}}
