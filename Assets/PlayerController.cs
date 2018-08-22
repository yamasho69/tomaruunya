using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // このスクリプトで使う変数一覧
    public CharacterController charaCon;       // キャラクターコンポーネント用の変数
    public Animator animCon;  //  アニメーションするための変数
    //プレイヤーを移動させるコンポーネントを入れる
    public Rigidbody rigicon;
    //前進するための力
    public float forwardForce = 3.0f;
    public float rotateSpeed = 80.0f;
    //ジャンプするための力
    public float upForce = 400.0f;
    //動きを減速させる係数
    public float coefficient = 0.95f;
    //加速するための力
    public float kasoku = 3.0f;
    //ブレーキボタン押下の判定
    public bool isBrakeButtonDown = false;
    //アクションボタン押下の判定
    public bool isActionButtonDown = false;
    public bool onJumpPanel = false;
    public bool onDashPanel = false;
    public bool is_jump = false;
    public int checkcount = 0;
    public int goalcount = 0;
    public bool isEnd = false;//終了判定
    //ライバルコントローラーを取得
    public RivalController RivalController;
    //ゲーム終了時に表示するテキスト（追加）
    public GameObject stateText;

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
            onJumpPanel = false;
            onDashPanel = false;
        }
        else
        {
            //ここ重要
            rb.velocity = new Vector3(transform.forward.x * 9.0f, rb.velocity.y, transform.forward.z * 9.0f);
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
        if (onDashPanel && Input.GetButtonDown("Fire1") && forwardForce <= 3.1f)
        {
            //加速する
            this.forwardForce *= this.kasoku;
            this.animCon.speed *= this.kasoku*0.55f;

            Invoke("Gensoku",5.0f);
        }
        this.rigicon.AddForce(this.transform.forward * this.forwardForce);
        if (rb.velocity.magnitude <= 1.0f) { this.animCon.SetBool("is_dush", false); }
        else { this.animCon.SetBool("is_dush", true); };

        if (isBrakeButtonDown == true) { GetMyBrakeButtonDown(); }
        //先にライバルがゴールしたとき
        if (RivalController.rivalgoalcount == 2)
        {   //stateTextにYOU LOSEを表示（追加）
            this.stateText.GetComponent<Text>().text = "YOU LOSE";
            isEnd = true;
        }
        //ゲーム終了ならプレイヤーの動きを減衰する（追加）
        if (this.isEnd)
        {
            this.forwardForce *= this.coefficient;
            this.rotateSpeed *= this.coefficient;
            this.upForce *= this.coefficient;
            this.animCon.speed *= this.coefficient;
        }
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
            Invoke("Gensoku", 2.0f);
        }  // 減速する
        if (collision.gameObject.tag == "Fall")
        {
            this.animCon.SetBool("is_fall", true);
        }
        if (collision.gameObject.tag == "jimen")
        {
            this.animCon.SetBool("is_fall", false);
        }
    }
    public void Gensoku() { forwardForce = 3.0f;
        rotateSpeed = 80.0f;
        this.animCon.speed = 1.0f;
        upForce = 800.0f;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(checkcount);
        Debug.Log(goalcount);

        if (other.gameObject.tag == "cp01")
        {
            if (checkcount == 0)
            {
                checkcount = 1;
            }
        }
        if (other.gameObject.tag == "cp02")
        {
            if (checkcount == 1)
            {
                checkcount = 2;
            }
        }
        if (other.gameObject.tag == "cp03")
        {
            if (checkcount == 2)
            {
                checkcount = 3;
            }
        }
        if (other.gameObject.tag == "cp04")
        {
            if (checkcount == 3)
            {
                checkcount = 4;
            }
        }
        if (other.gameObject.tag == "cp05")
        {
            if (checkcount == 4)
            {
                checkcount = 5;
            }
        }
        if (other.gameObject.tag == "cp06")
        {
            if (checkcount == 5)
            {
                checkcount = 6;
            }
        }
        if (other.gameObject.tag == "cp07")
        {
            if (checkcount == 6)
            {
                checkcount = 7;
            }
        }
        if (other.gameObject.tag == "cp08")
        {
            if (checkcount == 7)
            {
                checkcount = 8;
            }
        }
        if (other.gameObject.tag == "cp09")
        {
            if (checkcount == 8)
            {
                checkcount = 9;
            }
        }
        if (other.gameObject.tag == "cp10")
        {
            if (checkcount == 9)
            {
                checkcount = 10;
            }
        }
        if (other.gameObject.tag == "gp")
        {
            if (checkcount == 10 && goalcount ==0)
            {
                goalcount += 1;
                checkcount = 0;
                
            }
            if (checkcount == 10 && goalcount == 1)
            {
             this.stateText.GetComponent<Text>().text = "GOAL";
             isEnd = true;
            }
        }
    }
}
