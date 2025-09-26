using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public string sceneName; //切り替えたいScene名を指名

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
        
        //引数に指定した名前のシーンに切り替えしてくれるメソッドの呼び出し
        SceneManager.LoadScene("Boss");
         }

    }
}
