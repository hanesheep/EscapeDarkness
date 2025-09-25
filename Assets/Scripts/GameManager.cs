using System.Collections;
using UnityEditor.SearchService;
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
    public static bool[] doorsOpenedState = { false, false, false }; //�h�A�̊J��
    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keysPickedState = { false, false, false };  //���̎擾��

    public static int bill = 10;
    public static bool[] itemsPickedState = { false, false, false, false, false, }; //�A�C�e���̎擾��

    public static bool hasSpotLight;  //�X�|�b�g���C�g�������Ă��邩�ǂ���

    public static int playerHP = 3;           //�v���C���[��HP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�܂��͊J�n��Ԃɂ���
        gameState = GameState.playing;
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
        yield return new WaitForSeconds(5); //�T�b�҂�
        SceneManager.LoadScene("Title"); //�^�C�g���ɖ߂�
    }
}
