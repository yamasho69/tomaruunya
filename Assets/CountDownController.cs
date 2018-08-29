using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDownController : MonoBehaviour
{
    public GameObject _textCountdown;
    AudioSource audioSource;//オーディオコンポーネント
    public AudioClip Count;
    public AudioClip Go;
    public PlayerController PlayerController;
    public RivalController RivalController;

    IEnumerator Start()
    {
        //_textCountdown.text = "";
        //_textCountdown.gameObject.SetActive(true);
        audioSource = gameObject.GetComponent<AudioSource>();
        //GameObject PlayerController = GameObject.Find("Player");
        //GameObject RivalController = GameObject.Find("Rival");
        //GameObject BGM = GameObject.Find("BGM");

        this._textCountdown.GetComponent<Text>().text = "３";
        audioSource.PlayOneShot(Count, 1.0f);
        yield return new WaitForSeconds(1.0f);

        this._textCountdown.GetComponent<Text>().text = "２";
        audioSource.PlayOneShot(Count, 1.0f);
        yield return new WaitForSeconds(1.0f);

        this._textCountdown.GetComponent<Text>().text = "１";
        audioSource.PlayOneShot(Count, 1.0f);
        yield return new WaitForSeconds(1.0f);

        this._textCountdown.GetComponent<Text>().text = "すたーと㍉";
        audioSource.PlayOneShot(Go, 1.0f);
        GameObject.Find("Player").GetComponent<Animator>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        GameObject.Find("Rival").GetComponent<Animator>().enabled = true;
        GameObject.Find("Rival").GetComponent<RivalController>().enabled = true;
        GameObject.Find("BGM").GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(1.0f);
        this._textCountdown.GetComponent<Text>().text = "";
    }
    public void Update()
    {
        
    }
}
