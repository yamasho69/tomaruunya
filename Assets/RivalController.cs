﻿using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]



public class RivalController : MonoBehaviour
{
    [SerializeField]
    public Transform[] m_targets = null;
    [SerializeField]
    public float m_destinationThreshold = 0.0f;
    public Animator animCon;  //  アニメーションするための変数

    public NavMeshAgent m_navAgent = null;

    public int m_targetIndex = 0;
    public int rivalgoalcount = 0;
    public PlayerController PlayerController;
    //ゲーム終了時に表示するテキスト（追加）
    public GameObject stateText;

    public Vector3 CurretTargetPosition
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

    public void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.destination = CurretTargetPosition;
        //シーン中のstateTextオブジェクトを取得（追加）
        this.stateText = GameObject.Find("GameResultText");
        PlayerController playerController = GetComponent<PlayerController>();
        animCon = GetComponent<Animator>();
        rivalgoalcount = 0;
    }

    public void Update()
    {
        if (m_navAgent.remainingDistance <= m_destinationThreshold)
        {
            m_targetIndex = (m_targetIndex + 1) % m_targets.Length;

            m_navAgent.destination = CurretTargetPosition;
        }
        if (PlayerController.isEnd == true)
           { m_navAgent.Stop();
            this.animCon.SetBool("is_dush", false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "gp")
        {
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
