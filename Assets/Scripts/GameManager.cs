using UnityEngine;
using UnityEngine.UIElements;

//�Q�[����Ԃ��Ǘ�����񋓌^
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
    public static GameState gameState;  //�Q�[���̃X�e�[�^�X
    public static bool[] doorOpenState = { false, false, false }; //�h�A�̊J��
    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keypickedState = { false, false, false };  //���̎擾��

    public static int bill = 10;
    public static bool[] itemPickedState = { false, false, false, false, false, }; //�A�C�e���̎擾��

    public static bool hasSpotLight;  //�X�|�b�g���C�g�������Ă��邩�ǂ���

    public static int playerHP = 3;           //�v���C���[��HP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�܂��͊J�n��Ԃɂ���
        gameState = GameState.playing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
