using UnityEngine;

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
        Move();              //上下左右の入力値の取得
        angleZ = GetAngle(); //その時の角度を変数angleZに反映
        Animation();         //angleを利用してアニメーション
    }

    private void FixedUpdate()
    {
        //入力状況に応じてPlayerを動かす
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    //上下左右の入力値の取得
    public void Move()
    {
        //axisHとaxisVに入力情報を代入する
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
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
}
