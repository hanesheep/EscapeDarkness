using System.Collections;
using TMPro;
using UnityEngine;

public class TalkController : MonoBehaviour
{
    public MessageData message; //ScriptableObjectであるクラス
    bool isPlayerInRange;　　　　//プレイヤーが領域に入ったかどうか
    bool isTalk;　　　　　　　　//トークが開始されたか
    GameObject canvas; 　　　　　//トークUIを含んだCanvasぽぶじぇくと
    GameObject talkPanel;　　　　//対象となるトークUIパネル
    TextMeshProUGUI nameText; 　//対象となるトークUIパネルの名前
    TextMeshProUGUI messageText;　//対象となるトークUIパネルのメッセージ


    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        if(isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation(); //トーク開始

        }
    }

    //トークを開始してゲームスピードをストップさせるメソッド
    void StartConversation()
    {
        isTalk = true;  //トーク中フラグをたてる
        GameManager.gameState = GameState.talk;　//ステータスをtalk
        talkPanel.SetActive(true);　//トークUIパネルを表示
        Time.timeScale = 0;  //ゲーム進行スピードを0

        //TalkProcessCoroutineの発動
        StartCoroutine(TalkProcess());
    }

    //TalkProcessコルーチンの発動
    IEnumerator TalkProcess()
    {
        //対象としたScriptableObject（変数Message）が扱っている配列msgArrayの数だけ繰り返す
        for (int i = 0 ; i < message.msgArray.Length; i++)
        {
            nameText.text = message.msgArray[i].name;
            messageText.text = message.msgArray[i].message;

            yield return new WaitForSecondsRealtime(0.1f); //0.1秒待つ

            while (!Input.GetKeyDown(KeyCode.E))　//Eキーが押されるまで
            {
                yield return null;　　//何もしない
            }
        }
        EndConversation();
    }

    void EndConversation()
    {
        talkPanel.SetActive (false); //パネルを非表示
        GameManager.gameState = GameState.playing;　//ゲームステータスをPlayingに戻す
        isTalk = false;　//トーク中を解除
        Time.timeScale = 1.0f;　//ゲームスピードをもとに戻す
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
