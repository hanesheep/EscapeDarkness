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
    public MessageData message;�@//�g�[�N�f�[�^
    public GameObject door;�@//�\��/��\���Ώۂ̃h�A���

    public bool isSavePoint; //�Z�[�u�|�C���g�Ɏg����X�N���v�g�ɂ��邩�ǂ���


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���肪�v���C���[���������Z�[�u�|�C���g�łȂ����
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
