using System.Collections;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public RoomData roomData;　//親オブジェクトの持っているスクリプトを取得
    MessageData message;　　　　//親オブジェクトが持つScriptableObject情報を取得

    bool isPlayerInRange;　　　　//プレイヤーが領域に入ったかどうか
    bool isTalk;　　　　　　　　//トークが開始されたか
    GameObject canvas; 　　　　　//トークUIを含んだCanvasぽぶじぇくと
    GameObject talkPanel;　　　　//対象となるトークUIパネル
    TextMeshProUGUI nameText; 　//対象となるトークUIパネルの名前
    TextMeshProUGUI messageText;　//対象となるトークUIパネルのメッセージ

    void Start()
    {
        message = roomData.message; //トークデータは親オブジェクトのスクリプトにある変数を参照

        //トークUIオブジェクトなどの情報取得
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        //ドアの領域内にいる、かつ、トーク中でない、かつ、Eきーが押されたら
        if(isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
        {
            //トークの始まり
            StartConversation();
        }
    }

    //トークの始まりとなるメソッド
    void StartConversation()
    {
        isTalk = true;　//トーク中フラグがオン
        GameManager.gameState = GameState.talk;　//ゲームステータスがTalk
        talkPanel.SetActive(true);　　　　　　　//トークUIを表示
        nameText.text = message.msgArray[0].name;　//親オブジェクトから取得したmessageの配列の先頭の名前を表示
        messageText.text = message.msgArray[0].message;//親オブジェクトから取得したmessageの配列の先頭のメッセージを表示
        Time.timeScale = 0; //ゲームの進行をストップ
        StartCoroutine(TalkProcess()); //TalkProcessコルーチンの発動
    }

    //TalkProcessのコルーチンの設計
    IEnumerator TalkProcess()
    {
        SoundManager.instance.SEPlay(SEType.Door); //ドアを開く音

        //フラッシュ入力阻止のため、少し処理を止める
        yield return new WaitForSecondsRealtime(0.1f);

        //Eキーが押されるまで
        while(!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;  //Eキーが押されるまで何もしない
        }

        bool nextTalk = false; //トークをさらに展開するかどうか

        switch (roomData.roomName)
        {
            case "fromRoom1":
                if (GameManager.key1 > 0) //該当するカギを持っていたら
                {
                    GameManager.key1--;　//鍵の消耗
                    nextTalk = true;    //次のトーク展開をさせる
                    GameManager.doorsOpenedState[0] = true; //記録用の施錠状況をtrue
                }
                break;
            case "fromRoom2":
                if (GameManager.key2 > 0) //該当するカギを持っていたら
                {
                    GameManager.key2--;　//鍵の消耗
                    nextTalk = true;    //次のトーク展開をさせる
                    GameManager.doorsOpenedState[1] = true; //記録用の施錠状況をtrue
                }
                break;
            case "fromRoom3":
                if (GameManager.key3 > 0) //該当するカギを持っていたら
                {
                    GameManager.key3--;　//鍵の消耗
                    nextTalk = true;    //次のトーク展開をさせる
                    GameManager.doorsOpenedState[2] = true; //記録用の施錠状況をtrue
                }
                break;
        }
        if (nextTalk)
        {
            SoundManager.instance.SEPlay(SEType.DoorOpen); //ドアを開く音

            //開錠したというたぐいのメッセージを表示する
            nameText.text = message.msgArray[1].name;
            messageText.text = message.msgArray[1].name;

            //フラッシュ入力防止
            yield return new WaitForSecondsRealtime(0.1f);

            //Eキーが押されるまで待つ
            while (!Input.GetKeyDown(KeyCode.E))
            {
                yield return null;  //Eキーが押されるまで何もしない
            }
            roomData.openedDoor = true; //親のスクリプトのドア開錠フラグをON
            roomData.DoorOpenCheck();　　//開錠フラグに応じてドアの表示/非表示
        }
        EndCoversation();
    }

    void EndCoversation()
    {
        talkPanel.SetActive(false); //トークUIを非表示
        GameManager.gameState = GameState.playing;　//ゲームステータスをＰｌａｙｉｎｇ
        isTalk = false;　//トーク中フラグをＯＦＦ
        Time.timeScale = 1.0f;　//ゲーム進行をもとに戻す
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが領域に入ったら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがオン
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーが領域から出たら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがオフ
            isPlayerInRange = false;
        }
    }
}
