using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = { 0, 0, 0 };�@//�e�����̔z�u�ԍ�
    public static int key1PositionNumber;�@�@//���P�̔z�u�ԍ�
    public static int[] itemsPositionNumber = { 0, 0, 0, 0, 0 };�@//�A�C�e���̔z�u�ԍ�

    public GameObject[] items = new GameObject[5];�@�@//5�̃A�C�e���v���n�u�̓���
    public GameObject room;�@�@�@�@//�h�A�̃v���n�u
    public GameObject dummyDoor;�@//�_�~�[�̃v���n�u
    public GameObject key; �@�@�@�@//�L�[�̃v���n�u

    public static bool positioned;�@�@�@//�����z�u���ς݂��ǂ���
    public static string toRoomNumber = "fromRoom1";�@//Player���z�u�����ׂ��ʒu

    GameObject player;
    
    void Awake()
    {
        //�v���C���[���̎擾
        player = GameObject.FindGameObjectWithTag("Player");

        if (!positioned) //�����z�u���܂��ł����
        {
            StartKeysPosition(); //Key�̏���z�u
            StartItemsPosition(); //�A�C�e���̏����z�u
            positioned = true;   //����z�u�͍ς�
        }
    }

    void StartKeysPosition()
    {
        //�SKey1�X�|�b�g�̎擾
        GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

        //�����_���ɔԍ����擾(�������ȏ����������)
        int rand = Random.Range(1, (keySpots.Length + 1));

        //�S�X�|�b�g���`�F�b�N���ɍs��
        foreach (GameObject spots in keySpots)
        {
            //�ЂƂЂƂ�SpotNum�̒��g���m�F����rand�Ɠ������`�F�b�N
            if(spots.GetComponent<KeySpot>().spotNum == rand)
            {
                //�L�[�P�𐶐�
                Instantiate(key, spots.transform.position,Quaternion.identity);
                //�ǂ̃X�|�b�g�ԍ��ɃL�[��z�u���������L�^
                key1PositionNumber = rand;
            }
        }

        //Key2,Key3�̑ΏۃX�|�b�g
        GameObject keySpot;
        GameObject obj;

        //Key2�X�|�b�g�̎擾
        keySpot = GameObject.FindGameObjectWithTag("KeySpot2");
        //Key�̐�����obj�ւ̊i�[
        obj = Instantiate(key,keySpot.transform.position, Quaternion.identity);
        //��������Key�^�C�v��key2�֕ύX
        obj.GetComponent<KeyData>().keyType = KeyType.key2;

        //Key3�X�|�b�g�̎擾
        keySpot = GameObject.FindGameObjectWithTag("KeySpot3");
        //Key�̐�����obj�ւ̊i�[
        obj = Instantiate(key, keySpot.transform.position, Quaternion.identity);
        //��������Key�^�C�v��key3�֕ύX
        obj.GetComponent<KeyData>().keyType = KeyType.key3;
    }

    void StartItemsPosition()
    {
        //�S���̃A�C�e���X�|�b�g���擾
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            //�����_���Ȑ����̎擾
            //���������A�C�e������U��ς݂̔ԍ����������烉���_����������

            //�X�|�b�g�̑S�`�F�b�N�i�����_���l�ƃX�|�b�g�ԍ��̈�v�j
            //��v���Ă���΁A�����ɃA�C�e���𐶐�

            //�ǂ̃X�|�b�g�ԍ����x�̃A�C�e���Ɋ��蓖�Ă��Ă���̂����L�^

            //���������A�C�e���Ɏ��ʔԍ�������U���Ă���
        }

    }

}
