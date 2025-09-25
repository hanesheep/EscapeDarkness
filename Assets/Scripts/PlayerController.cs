using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの基礎ステータス")]
    public float playerSpeed = 3.0f;
    
    float axisH; //横方向の入力情報
    float axisV; //縦方向の入力情報

    [Header("プレイヤーの角度計算用")]
    public float angleZ = -90f;　//プレイヤーの角度計算用

    [Header("オン/オフの対象スポットライト")]
    public GameObject spotLight;　//対象のスポットライト

    bool inDamage;　//ダメージ中かのフラグ管理

    //コンポーネント
    Rigidbody2D rbody;
    Animator anime;

    bool isViartual; //VirtualPadを触っているかどうかの判断フラグ

    //足音判定
    float footstepInterval = 0.3f; //足音間隔
    float footstepTimer; //時間計測

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントの取得
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //スポットライトを所持していればスポットライト表示
        if (GameManager.hasSpotLight)
        {
            spotLight.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイ中orエンディング中でなければなにもしない
        if (!(GameManager.gameState != GameState.playing || GameManager.gameState != GameState.ending)) return;

        Move();              //上下左右の入力値の取得
        angleZ = GetAngle(); //その時の角度を変数angleZに反映
        Animation();         //angleを利用してアニメーション

        //足音
        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        //プレイ中orエンディング中でなければなにもしない
        if (!(GameManager.gameState != GameState.playing || GameManager.gameState != GameState.ending)) return;

        //ダメージフラグが立っている間
        if (inDamage)
        {
            //点滅演出
            //Sinメソッドの角度情報にゲーム開始からの経過時間を与える
            float val = Mathf.Sin(Time.time * 50);

            if (val > 0)
            {
                //描画距離を有効
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //描画距離を無効
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            //入力によるvelocityが入らないようにここでリターン
            return;
        }
        //入力状況に応じてPlayerを動かす
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    //上下左右の入力値の取得
    public void Move()
    {
        if (!isViartual)
        {
            //axisHとaxisVに入力情報を代入する
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
        }
    }

    //その時のプレイヤーの角度を取得
    public float GetAngle()
    {
        //現在座標の取得
        Vector2 fromPos = transform.position;

        //その瞬間のキー入力値（axisH,axisV）に応じた予想座標の取得
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle;

        //もしも何かしらの入力があれば新たに角度を算出
        if (axisH != 0 || axisV !=0)
        {
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            //第一引数に高さY、第二引数に底辺Xを与えると角度を算出をラジアン形式で算出
            //円周の長さで表現
            float rad = Mathf.Atan2(dirY,dirX);

            //ラジアン値をオイラー値(デグリー)に変換
            angle = rad * Mathf.Rad2Deg;
        }

        //何も入力されていなければ、前フレームの角度情報を据え置き
        else
        {
            angle = angleZ;
        }

        return angle;
    }

    void Animation()
    {
        //なんらかの入力がある場合
        if (axisH != 0 || axisV != 0)
        {

            //ひとまずRunアニメを走らせる
            anime.SetBool("run", true);

            //angleZを利用して方角を決める　パラメータdirection int型
            //int型のdirection 下：0　上：1　右：2　左：それ以外

            if (angleZ > -135f && angleZ < -45f) //下方向
            {
                anime.SetInteger("direction", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //右方向
            {
                anime.SetInteger("direction", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //上方向
            {
                anime.SetInteger("direction", 1);
            }
            else //左方向
            {
                anime.SetInteger("direction", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else //何も入力がない場合
        {
            anime.SetBool("run", false); //走るフラグをOFF
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ぶつかった相手がEnemyだったら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage(collision.gameObject); //ダメージ処理の開始
        }
    }

    void GetDamage(GameObject enemy)
    {
        if (GameManager.gameState != GameState.playing) return;

        SoundManager.instance.SEPlay(SEType.Damage); //ダメージを受ける音


        GameManager.playerHP--; //プレイヤーのHPを1減らす

        if(GameManager.playerHP > 0)
        {
            //そこまでのプレイヤーの動きをいったんストップ
            rbody.linearVelocity = Vector2.zero;
            //プレイヤーと敵の差を取得し方向を決める
            Vector3 v = (transform.position - enemy.transform.position).normalized;
            //決まった方向に押される
            rbody.AddForce(v * 4 , ForceMode2D.Impulse);

            //点滅するためのフラグ
            inDamage = true;

            //0.25秒後の時間差でフラグを解除
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            //残HPが残っていなければゲームオーバー
            GameOver();
        }
    }

    void DamageEnd()
    {
        inDamage = false; //点滅ダメージフラグを解除
        gameObject.GetComponent<SpriteRenderer>().enabled = true; //プレイヤーを確実に表示
    }

    void GameOver()
    {
        //ゲームの状態を変える
        GameManager.gameState = GameState.gameover;

        //ゲームオーバー演出
        GetComponent<CircleCollider2D>().enabled = false; //当たり判定の無効化
        rbody.linearVelocity = Vector2.zero; //動きを止める
        rbody.gravityScale = 1.0f;　//重力の復活
        anime.SetTrigger("dead");   //死亡アニメクリップの発動
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); //上に跳ね上げる
        Destroy(gameObject, 1.0f); //１秒後に存在を消去
    }

    //スポットライトの入手フラグが立っていたらライトをつける
    public void SpotLightCheck()
    {
        if(GameManager.hasSpotLight)spotLight.SetActive(true);
    }

    //VirtualPadの入力に反応するメソッド
    public void SetAxis(float virH,float virV)
    {
        //どちらかの引数に値が入っていれば
        if(virH !=0 || virV != 0)
        {
            isViartual = true;
            axisH = virH;
            axisV = virV;
        }
        else //virtualPadが触られていない（引数が両方0）
        {
            isViartual = false;
        }
    }

    //足音
    void HandleFootsteps()
    {
        //プレイヤーが動いていれば
        if (axisH != 0 || axisV != 0)
        {
            footstepTimer += Time.deltaTime; //時間計測

            if (footstepTimer >= footstepInterval) //インターバルチェック
            {
                SoundManager.instance.SEPlay(SEType.Walk);
                footstepTimer = 0;
            }
        }
        else //動いていなければ時間計測リセット
        {
            footstepTimer = 0f;
        }
    }
}
