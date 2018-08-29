using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDownController : MonoBehaviour
{
    public GameObject _textCountdown;
    AudioSource audioSource;//オーディオコンポーネント
    public AudioClip Count;
    public AudioClip Go;

    IEnumerator Start()
    {
        //_textCountdown.text = "";
        //_textCountdown.gameObject.SetActive(true);
        audioSource = gameObject.GetComponent<AudioSource>();

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
        yield return new WaitForSeconds(1.0f);

        this._textCountdown.GetComponent<Text>().text = "";
    }
    public void Update()
    {
        
    }
}
