using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

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
    public static bool[] doorsOpenedState = { false, false, false }; //ドアの開閉状況
    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keysPickedState = { false, false, false };  //鍵の取得状況

    public static int bill = 0;
    public static bool[] itemsPickedState = { false, false, false, false, false, }; //アイテムの取得状況

    public static bool hasSpotLight;  //スポットライトを持っているかどうか

    public static int playerHP = 3;           //プレイヤーのHP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //まずは開始状態にする
        gameState = GameState.playing;

        //シーン名の取得
        Scene currentScene = SceneManager.GetActiveScene();
        // シーンの名前を取得
        string sceneName = currentScene.name;

        switch (sceneName)
        {
            case "Title":
                SoundManager.instance.PlayBgm(BGMType.Title);
                break;
            case "Boss":
                SoundManager.instance.PlayBgm(BGMType.InBoss);
                break;
            case "Opening":
            case "Ending":
                SoundManager.instance.StopBgm();
                break;
            default:
                SoundManager.instance.PlayBgm(BGMType.InGame);
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(gameState == GameState.gameover)
        {
            StartCoroutine(TitleBack());
        }
    }
    IEnumerator TitleBack()
    {
        yield return new WaitForSeconds(5); //５秒待つ
        SceneManager.LoadScene("Title"); //タイトルに戻る
    }
}
