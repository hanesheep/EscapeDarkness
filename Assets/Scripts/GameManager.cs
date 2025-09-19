using UnityEngine;
using UnityEngine.UIElements;

//ゲーム状態を管理する列挙型
public enum GameState
{
    playing,
    talk,
    gameover,
    gameclear,
    ending
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState;  //ゲームのステータス
    public static bool[] doorOpenState = { false, false, false }; //ドアの開閉状況
    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keypickedState = { false, false, false };  //鍵の取得状況

    public static int bill = 10;
    public static bool[] itemPickedState = { false, false, false, false, false, }; //アイテムの取得状況

    public static bool hasSpotLight;  //スポットライトを持っているかどうか

    public static int playerHP = 3;           //プレイヤーのHP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //まずは開始状態にする
        gameState = GameState.playing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
