using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum DoorDirection
{
    up,down
}


public class RoomData : MonoBehaviour
{
    public string roomName;　//出入り口の識別名
    public string nextRoomName;　//シーン切り替え先の行き先
    public string nextScene;　//シーン切り替え先
    public bool openedDoor;　　//ドアの開錠
    public DoorDirection direction;　//ぷれいやー配置時の位置
    public MessageData message;　//トークデータ
    public GameObject door;　//表示/非表示対象のドア情報

    public bool isSavePoint; //セーブポイントに使われるスクリプトにするかどうか


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //相手がプレイヤーかつ自分がセーブポイントでなければ
        if (collision.gameObject.CompareTag("Player")&& !isSavePoint)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        RoomManager.toRoomNumber = nextRoomName;

        SceneManager.LoadScene(nextScene);
    }

    public void DoorOpenCheck()
    {
        if (openedDoor) door.SetActive(false);
    }
}
