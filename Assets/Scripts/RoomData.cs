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
    public MessageData message;
    public GameObject door;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
