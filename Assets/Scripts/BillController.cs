using UnityEngine;

public class BillController : MonoBehaviour
{
    public float deleteTime = 2.0f;  //���������܂ł̎���
    public GameObject barrierPrefab; //���ȏ��łƈ��������ɐ�������v���n�u

    
    void Start()
    {
        //DeliteTime�b��Ɂu�o���A�W�J���ď��Łv
        Invoke("FieldExpansion", deleteTime);
    }

    //�o���A�W�J�Ǝ��ȏ��ł��s�����\�b�h
    void FieldExpansion()
    {
        //���D�Ɠ����ꏊ�Ƀo���A����
        Instantiate(barrierPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject);//���D�͏���
    }

    //�G�ƂԂ�������o���A����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FieldExpansion();
        }
    }
}
