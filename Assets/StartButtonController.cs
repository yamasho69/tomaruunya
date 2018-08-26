using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class StartButtonController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //マウス左クリック、スペースキー、Aボタン、ジャンプボタンを押した場合
        {
            SceneManager.LoadScene("GameScene");//GameSceneシーンをロードする
        }

    }
}
