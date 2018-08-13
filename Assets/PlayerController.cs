using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController: MonoBehaviour
{
    // このスクリプトで使う変数一覧
    private CharacterController charaCon;       // キャラクターコンポーネント用の変数
    private Animator animCon;  //  アニメーションするための変数
    //プレイヤーを移動させるコンポーネントを入れる
    private Rigidbody RigiCon;
    public float idoSpeed = 5.0f;         // 移動速度（Public＝インスペクタで調整可能）
    public float kaitenSpeed = 1200.0f;   // プレイヤーの回転速度（Public＝インスペクタで調整可能）
    //前進するための力
    private float forwardForce = 800.0f;
    //ジャンプするための力
    private float upForce = 500.0f;
    //動きを減速させる係数
    private float coefficient = 0.95f;
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
        this.animCon.SetFloat("Speed", 1);
        //Rigidbodyコンポーネントを取得
        this.RigiCon = GetComponent<Rigidbody>();
    }
    // ■毎フレーム常に実行する処理
    void Update()
    {
        // ▼▼▼移動処理▼▼▼
        if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 0 && CrossPlatformInputManager.GetAxisRaw("Horizontal") == 0) //  テンキーや3Dスティックの入力（GetAxis）がゼロの時の動作
        {
            animCon.SetBool("A_dash", false);  //  Runモーションしない
        }
        else //  テンキーや3Dスティックの入力（GetAxis）がゼロではない時の動作
        {
            var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  //  カメラが追従するための動作
            Vector3 direction = Camera.main.transform.right * CrossPlatformInputManager.GetAxisRaw("Horizontal");  //  テンキーや3Dスティックの入力（GetAxis）があるとdirectionに値を返す
            animCon.SetBool("A_dash", true);  //  Runモーションする

            MukiWoKaeru(direction);  //  向きを変える動作の処理を実行する（後述）
            IdoSuru(direction);  //  移動する動作の処理を実行する（後述）
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //ジャンプパネル接触時にアクションボタンを押下したとき
        if (other.gameObject.tag == "Jump" && Input.GetButtonDown("Action"))
        {
            //ジャンプアニメを再生
            animCon.SetBool("Jump", true);
            //プレイヤーに上方向の力を加える
            RigiCon.AddForce(this.transform.up * this.upForce);
        }
        //ダッシュパネル接触時にアクションボタンを押下したとき
        if (other.gameObject.tag == "Dash" && Input.GetButtonDown("Action"))
        {　
        　　//加速する

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


    // ■向きを変える動作の処理
    void MukiWoKaeru(Vector3 mukitaiHoukou)
    {
        Quaternion q = Quaternion.LookRotation(mukitaiHoukou);          // 向きたい方角をQuaternion型に直す
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q,  kaitenSpeed * 1/850);   // 向きを q に向けてじわ～っと変化させる.
    }
    // ■移動する動作の処理
    void IdoSuru(Vector3 idosuruKyori)
    {
        charaCon.Move(idosuruKyori * Time.deltaTime * idoSpeed);   // プレイヤーの移動距離は時間×移動スピードの値
    }
}
