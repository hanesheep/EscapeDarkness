using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum DoorDirection
{
    up,down
}


public class RoomData : MonoBehaviour
{
    public string roomName;�@//�o������̎��ʖ�
    public string nextRoomName;�@//�V�[���؂�ւ���̍s����
    public string nextScene;�@//�V�[���؂�ւ���
    public bool openedDoor;�@�@//�h�A�̊J��
    public DoorDirection direction;�@//�Ղꂢ��[�z�u���̈ʒu
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
