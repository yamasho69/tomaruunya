using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // このスクリプトで使う変数一覧
    private CharacterController charaCon;       // キャラクターコンポーネント用の変数
    private Animator animCon;  //  アニメーションするための変数
    //プレイヤーを移動させるコンポーネントを入れる
    private Rigidbody rigicon;
    //前進するための力
    public float forwardForce = 3.0f;
    public float rotateSpeed = 20.0f;
    //ジャンプするための力
    private float upForce = 500.0f;
    //動きを減速させる係数
    private float coefficient = 0.95f;
    //加速するための力
    private float kasoku = 1.50f;
    //ブレーキボタン押下の判定
    private bool isBrakeButtonDown = false;
    //アクションボタン押下の判定
    private bool isActionButtonDown = false;

    // ■最初に1回だけ実行する処理
    void Start()
    {
        charaCon = GetComponent<CharacterController>(); // キャラクターコントローラーのコンポーネントを参照する
        animCon = GetComponent<Animator>(); // アニメーターのコンポーネントを参照する
        //走るアニメーションを開始
        //this.animCon.SetFloat("Speed", 1);
        //Rigidbodyコンポーネントを取得
        this.rigicon = GetComponent<Rigidbody>();
    }
    // ■毎フレーム常に実行する処理
    void Update()
    {
        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(v * forwardForce * transform.forward, ForceMode.Force);   // 上下で前進・後退
        if (CrossPlatformInputManager.GetButton("Fire2"))   // Fire2 = 右クリック、左 Alt
        {
            rb.velocity = rb.velocity * coefficient;
        }
        else
        {
            //ここ重要
            rb.velocity = transform.forward * 10.0f;
            //rb.AddForce(forwardForce * transform.forward, ForceMode.Acceleration);
        }
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * h);
        //前に行く力を加える
        this.rigicon.AddForce(this.transform.forward * this.forwardForce);
        


    }
    void OnCollisionStay(Collider other)
    {
        //ジャンプパネル接触時にアクションボタンを押下したとき
        if (other.gameObject.tag == "Jump" && Input.GetButtonDown("Action"))
        {
            //ジャンプアニメを再生
            animCon.SetBool("Jump", true);
            //プレイヤーに上方向の力を加える
            rigicon.AddForce(this.transform.up * this.upForce);
        }
        //ダッシュパネル接触時にアクションボタンを押下したとき
        if (other.gameObject.tag == "Dash" && Input.GetButtonDown("Action"))
        {
            //加速する
            this.forwardForce *= this.kasoku;
            this.upForce *= this.kasoku;
            this.animCon.speed *= this.kasoku;
        }

    }
    //ブレーキボタンを押下したとき
    public void GetMyBrakeButtonDown()
    {
        //地面に設置していれば
        if (this.transform.position.y < 0.5f)
        {
            //減速する
            this.forwardForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.animCon.speed *= this.coefficient;
        }
    }
    //壁に接触したとき
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            this.forwardForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.animCon.speed *= this.coefficient;
        }  // 減速する
    }
}
