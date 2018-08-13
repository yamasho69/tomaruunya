/*
このプログラムは次の機能を有します。
	• キャラはプレイヤーの操作によって平面方向で移動する。
	• キャラの移動方向はカメラが向いている方向（＝画面の前方）を基準とする。
	• キャラは他の Collidor(衝突判定物) を避ける。つまり壁や柱の中に入り込まない。
	• キャラは自身の Collidorが他の Collidor から垂直方向に離れると、他のCollidor に接触するまで落下する。
	• キャラは落下中、移動方向は変更できない。
	• キャラは 一定角度の Collidor または一定量の段差のある Collidor があったとき、登ることができる。
	• キャラが止まっているとき、進行方向は即時に変更することができるが、移動中の場合は徐々に進行方向が変わる。
	• 移動のキーが押されてすぐは歩行となり、押され続けて一定の時間がたつと走行に変化する。
	• 移動のキーが離されるとキャラの移動は止まる。
	• 移動のスティックの傾きによってキャラは静止、歩行、走行に変化する。
	• キャラは静止、歩行、走行、落下の状態に合わせて、モーションを変化させる。
*/
using UnityEngine;
using System.Collections;

public class BasicCharCont : MonoBehaviour {
    private float walkSpeed = 2.0f; // 歩行速度
    private float runSpeed = 4.0f; // 走行速度

    private Vector3 movement;   // 移動するベクター

    private float gravity = 20.0f; // キャラへの重力
    private float speedSmoothing = 10.0f;   // 回頭するときの滑らかさ
    private float rotateSpeed = 500.0f; // 回頭の速度
    private float runAfterSeconds = 0.1f;   // 走り終わったとき止まるまでの時間(秒)

    private Vector3 moveDirection = Vector3.zero;   // カレント移動方向
    private float verticalSpeed = 0.0f;    // カレント垂直方向速度
    private float moveSpeed = 0.0f;    // カレント水平方向速度

    private CollisionFlags collisionFlags;    //  controller.Move が返すコリジョンフラグ：キャラが何かにぶつかったとき使用

    private float walkTimeStart = 0.0f;    // 歩き始める速度

    // Use this for initialization
    void Start () {
        moveDirection = transform.TransformDirection(Vector3.forward);  // キャラの移動方向をキャラの向いている方向にセットする
    }
	
	// Update is called once per frame
	void Update () {
        Transform cameraTransform = Camera.main.transform;  // カメラの向いている方向を得る
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);  // camera の x-z 平面から forward ベクターを求める 
        forward.y = 0;  // Y方向は無視：キャラは水平面しか移動しないため
        forward = forward.normalized;   // 方向を正規化する
        Vector3 right = new Vector3(forward.z, 0, -forward.x);    // 右方向ベクターは常にforwardに直交

        float v = Input.GetAxisRaw("Vertical"); // マウスもしくはコントローラスティックの垂直方向の値
        float h = Input.GetAxisRaw("Horizontal");   // マウスもしくはコントローラスティックの水平方向の値

        Vector3 targetDirection = h * right + v * forward;  // カメラと連動した進行方向を計算：視点の向きが前方方向

        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)   // キャラは接地しているか？：宙に浮いていないとき
        {
            if (targetDirection != Vector3.zero) // キャラは順方向を向いていないか？：つまり回頭している場合
            {
                if (moveSpeed < walkSpeed * 0.9) // ゆっくり移動か？
                {
                    moveDirection = targetDirection.normalized; // 止まっているときは即時ターン
                }
                else  // 移動しているときはスムースにターン
                {
                    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    moveDirection = moveDirection.normalized;
                }
            }

            float curSmooth = speedSmoothing * Time.deltaTime;     // 向きをスムースに変更
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f); // 最低限のスピードを設定

            // 歩く速度と走る速度の切り替え：最初は歩いてで時間がたつと走る
            if (Time.time - runAfterSeconds > walkTimeStart)
                targetSpeed *= runSpeed;
            else
                targetSpeed *= walkSpeed;

            // 移動速度を設定
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

            Animator animator = GetComponent<Animator>();   // Animator コンポーネントを得る
            animator.SetFloat("spd", moveSpeed); // Animator に移動速度のパラメータを渡す
            animator.SetBool("fall", false);    // Animator に落下フラグのパラメータを渡す：落下していない

            if (moveSpeed < walkSpeed * 0.3)    // まだ歩きはじめ
                walkTimeStart = Time.time;  // その時間を保存しておく

            verticalSpeed = 0.0f;   // 垂直方向の速度をゼロに設定
        }
        else // 宙に浮いている
        {
            verticalSpeed -= gravity * Time.deltaTime;  // 重力を適応
            if (verticalSpeed < -4.0)   // 落ちる速度が一定を超えたら
            {
                Animator animator = GetComponent<Animator>();   // Animator コンポーネントを得る
                animator.SetBool("fall", true);   // Animator に落下フラグのパラメータを渡す：落下している
            }
        }

        movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);   // キャラの移動量を計算
        movement *= Time.deltaTime;

        CharacterController controller = GetComponent<CharacterController>();   // キャラクターコントローラコンポーネントを取得
        collisionFlags = controller.Move(movement);   // キャラを移動をキャラクターコントローラに伝える

        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)   // 宙に浮いていない場合
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);       // 移動方向に回頭：浮いていると回頭しない
        }
    }
}