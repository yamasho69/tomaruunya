using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class StartButtonController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip StartVoice;
    //public PlayerController PlayerController;
    //private bool isButtonDown = false;


    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        PlayerController playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetMyButtonUp()
    {
        audioSource.PlayOneShot(StartVoice, 1.0f);
        SceneManager.LoadScene("GameScene");//GameSceneシーンをロードする}
    }
}
