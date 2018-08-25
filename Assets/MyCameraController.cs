using UnityEngine;
using System.Collections;

public class MyCameraController : MonoBehaviour
{
    public PlayerController PlayerController;
    public Animator animCon;  //  アニメーションするための変数
    public bool is_spin = false;

    void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        animCon = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerController.isEnd == true)
        { this.animCon.SetBool("is_spin", true); }
    }
}