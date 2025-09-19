using UnityEngine;

public class BillData : MonoBehaviour
{
    Rigidbody2D rbody;
    public int itemNum; //�A�C�e���̎��ʔԍ�

    
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Rigiddbody2D�R���|�[�l���g�擾
        rbody.bodyType = RigidbodyType2D.Static; //Rigidbody�̋�����Î~
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.bill++;
            GameManager.itemPickedState[itemNum] = true;

            //�A�C�e���擾�̉��o
            //�R���C�_�[�𖳌���
            GetComponent<CircleCollider2D>().enabled = false;
            //Rigidbody2D�̕���
            rbody.bodyType = RigidbodyType2D.Dynamic;
            //��ɑł��グ��
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            //�������g�𖕏�
            Destroy(gameObject, 0.5f);
        }
    }
}
