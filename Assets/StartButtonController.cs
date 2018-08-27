using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class StartButtonController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip StartCoroutine;
    public PlayerController PlayerController;


    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        PlayerController playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //マウス左クリック、スペースキー、Aボタン、ジャンプボタンを押した場合
        {
            audioSource.PlayOneShot(StartCoroutine, 1.0f);
            SceneManager.LoadScene("GameScene");//GameSceneシーンをロードする
        }

    }
}
