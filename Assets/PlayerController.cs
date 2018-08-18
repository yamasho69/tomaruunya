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
    private float upForce = 1000.0f;
    //動きを減速させる係数
    private float coefficient = 0.95f;
    //加速するための力
    private float kasoku = 2.5f;
    //ブレーキボタン押下の判定
    private bool isBrakeButtonDown = false;
    //アクションボタン押下の判定
    private bool isActionButtonDown = false;
    private bool onJumpPanel = false;
    private bool onDashPanel = false;
    private bool is_jump = false;

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
            rb.velocity = new Vector3(transform.forward.x * 10.0f, rb.velocity.y, transform.forward.z * 10.0f);
            //rb.AddForce(forwardForce * transform.forward, ForceMode.Acceleration);
        }
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * h);
        //前に行く力を加える
        //前に行く力を加える
        var v2 = this.transform.forward * this.forwardForce;
        this.rigicon.AddForce(v2);
        //ジャンプパネルに乗っている時にボタンが押された
        //Jumpステートの場合はJumpにfalseをセットする（追加）
        if (this.animCon.GetCurrentAnimatorStateInfo(0).IsName("A_jump_start"))
        {
            this.animCon.SetBool("is_jump", false);
        }

        if (onJumpPanel && Input.GetButtonDown("Fire1"))
        {
        
            //プレイヤーに上方向の力を加える
            rigicon.AddForce(this.transform.up * this.upForce);
            //ジャンプアニメを再生
            animCon.SetBool("is_jump", true);

        }
        //ダッシュパネルに乗っている時にボタンが押された
        if (onDashPanel && Input.GetButtonDown("Fire1"))
        {
            //加速する
            this.forwardForce *= this.kasoku;
            this.upForce *= this.kasoku;
            this.animCon.speed *= this.kasoku;
        }
        this.rigicon.AddForce(this.transform.forward * this.forwardForce);
        if (rb.velocity.magnitude <= 1.0f) { this.animCon.SetBool("is_dush", false); }
        else { this.animCon.SetBool("is_dush", true); };
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Jump")
        {
            onJumpPanel = true;
        }
        if (other.gameObject.tag == "Dash")
        {
            onDashPanel = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Jump")
        {
            onJumpPanel = false;
        }
        if (other.gameObject.tag == "Dash")
        {
            onDashPanel = false;
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
