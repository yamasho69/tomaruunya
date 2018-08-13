using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController: MonoBehaviour
{
    // このスクリプトで使う変数一覧
    private CharacterController charaCon;       // キャラクターコンポーネント用の変数
    private Animator animCon;  //  アニメーションするための変数
    public float idoSpeed = 5.0f;         // 移動速度（Public＝インスペクタで調整可能）
    public float kaitenSpeed = 1200.0f;   // プレイヤーの回転速度（Public＝インスペクタで調整可能）

    private Vector3 movePower = Vector3.zero;    // キャラクター移動量（未使用）
    private float jumpPower = 10.0f;        // キャラクター跳躍力（未使用）
    private const float gravityPower = 9.8f;         // キャラクター重力（未使用）

    public void Hit()        // ヒット時のアニメーションイベント（今のところからっぽ。ないとエラーが出る）
    {
    }


    // ■最初に1回だけ実行する処理
    void Start()
    {
        charaCon = GetComponent<CharacterController>(); // キャラクターコントローラーのコンポーネントを参照する
        animCon = GetComponent<Animator>(); // アニメーターのコンポーネントを参照する
    }


    // ■毎フレーム常に実行する処理
    void Update()
    {
        // 右・左
        float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        // 上・下
        float y = CrossPlatformInputManager.GetAxisRaw("Vertical");


        // ▼▼▼移動処理▼▼▼
        if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 0 && CrossPlatformInputManager.GetAxisRaw("Horizontal") == 0) //  テンキーや3Dスティックの入力（GetAxis）がゼロの時の動作
        {
            animCon.SetBool("Run", false);  //  Runモーションしない
        }

        else //  テンキーや3Dスティックの入力（GetAxis）がゼロではない時の動作
        {
            var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  //  カメラが追従するための動作
            Vector3 direction = cameraForward * CrossPlatformInputManager.GetAxisRaw("Vertical") + Camera.main.transform.right * CrossPlatformInputManager.GetAxisRaw("Horizontal");  //  テンキーや3Dスティックの入力（GetAxis）があるとdirectionに値を返す
            animCon.SetBool("Run", true);  //  Runモーションする

            MukiWoKaeru(direction);  //  向きを変える動作の処理を実行する（後述）
            IdoSuru(direction);  //  移動する動作の処理を実行する（後述）
        }


        // ▼▼▼アクション処理▼▼▼
        animCon.SetBool("Action", Input.GetKey("x") || Input.GetButtonDown("Action"));  //  キーorボタンを押したらアクションを実行
        animCon.SetBool("Brake", Input.GetKey("space") || Input.GetButtonDown("Brake"));  //  キーorボタンを押したらジャンプを実行
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
