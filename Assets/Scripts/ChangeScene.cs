using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public string sceneName; //�؂�ւ�����Scene�����w��

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
        
        //�����Ɏw�肵�����O�̃V�[���ɐ؂�ւ����Ă���郁�\�b�h�̌Ăяo��
        SceneManager.LoadScene("Boss");
         }

    }
}
