using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̊�b�X�e�[�^�X")]
    public float playerSpeed = 3.0f;
    
    float axisH; //�������̓��͏��
    float axisV; //�c�����̓��͏��

    [Header("�v���C���[�̊p�x�v�Z�p")]
    public float angleZ = -90f;�@//�v���C���[�̊p�x�v�Z�p

    [Header("�I��/�I�t�̑ΏۃX�|�b�g���C�g")]
    public GameObject spotLight;�@//�Ώۂ̃X�|�b�g���C�g

    bool inDamage;�@//�_���[�W�����̃t���O�Ǘ�

    //�R���|�[�l���g
    Rigidbody2D rbody;
    Animator anime;

    bool isViartual; //VirtualPad��G���Ă��邩�ǂ����̔��f�t���O

    //��������
    float footstepInterval = 0.3f; //�����Ԋu
    float footstepTimer; //���Ԍv��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�R���|�[�l���g�̎擾
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //�X�|�b�g���C�g���������Ă���΃X�|�b�g���C�g�\��
        if (GameManager.hasSpotLight)
        {
            spotLight.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C��or�G���f�B���O���łȂ���΂Ȃɂ����Ȃ�
        if (!(GameManager.gameState != GameState.playing || GameManager.gameState != GameState.ending)) return;

        Move();              //�㉺���E�̓��͒l�̎擾
        angleZ = GetAngle(); //���̎��̊p�x��ϐ�angleZ�ɔ��f
        Animation();         //angle�𗘗p���ăA�j���[�V����

        //����
        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        //�v���C��or�G���f�B���O���łȂ���΂Ȃɂ����Ȃ�
        if (!(GameManager.gameState != GameState.playing || GameManager.gameState != GameState.ending)) return;

        //�_���[�W�t���O�������Ă����
        if (inDamage)
        {
            //�_�ŉ��o
            //Sin���\�b�h�̊p�x���ɃQ�[���J�n����̌o�ߎ��Ԃ�^����
            float val = Mathf.Sin(Time.time * 50);

            if (val > 0)
            {
                //�`�拗����L��
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //�`�拗���𖳌�
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            //���͂ɂ��velocity������Ȃ��悤�ɂ����Ń��^�[��
            return;
        }
        //���͏󋵂ɉ�����Player�𓮂���
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    //�㉺���E�̓��͒l�̎擾
    public void Move()
    {
        if (!isViartual)
        {
            //axisH��axisV�ɓ��͏���������
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
        }
    }

    //���̎��̃v���C���[�̊p�x���擾
    public float GetAngle()
    {
        //���ݍ��W�̎擾
        Vector2 fromPos = transform.position;

        //���̏u�Ԃ̃L�[���͒l�iaxisH,axisV�j�ɉ������\�z���W�̎擾
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle;

        //��������������̓��͂�����ΐV���Ɋp�x���Z�o
        if (axisH != 0 || axisV !=0)
        {
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            //�������ɍ���Y�A�������ɒ��X��^����Ɗp�x���Z�o�����W�A���`���ŎZ�o
            //�~���̒����ŕ\��
            float rad = Mathf.Atan2(dirY,dirX);

            //���W�A���l���I�C���[�l(�f�O���[)�ɕϊ�
            angle = rad * Mathf.Rad2Deg;
        }

        //�������͂���Ă��Ȃ���΁A�O�t���[���̊p�x���𐘂��u��
        else
        {
            angle = angleZ;
        }

        return angle;
    }

    void Animation()
    {
        //�Ȃ�炩�̓��͂�����ꍇ
        if (axisH != 0 || axisV != 0)
        {

            //�ЂƂ܂�Run�A�j���𑖂点��
            anime.SetBool("run", true);

            //angleZ�𗘗p���ĕ��p�����߂�@�p�����[�^direction int�^
            //int�^��direction ���F0�@��F1�@�E�F2�@���F����ȊO

            if (angleZ > -135f && angleZ < -45f) //������
            {
                anime.SetInteger("direction", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //�E����
            {
                anime.SetInteger("direction", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //�����
            {
                anime.SetInteger("direction", 1);
            }
            else //������
            {
                anime.SetInteger("direction", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else //�������͂��Ȃ��ꍇ
        {
            anime.SetBool("run", false); //����t���O��OFF
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�Ԃ��������肪Enemy��������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage(collision.gameObject); //�_���[�W�����̊J�n
        }
    }

    void GetDamage(GameObject enemy)
    {
        if (GameManager.gameState != GameState.playing) return;

        SoundManager.instance.SEPlay(SEType.Damage); //�_���[�W���󂯂鉹


        GameManager.playerHP--; //�v���C���[��HP��1���炷

        if(GameManager.playerHP > 0)
        {
            //�����܂ł̃v���C���[�̓�������������X�g�b�v
            rbody.linearVelocity = Vector2.zero;
            //�v���C���[�ƓG�̍����擾�����������߂�
            Vector3 v = (transform.position - enemy.transform.position).normalized;
            //���܂��������ɉ������
            rbody.AddForce(v * 4 , ForceMode2D.Impulse);

            //�_�ł��邽�߂̃t���O
            inDamage = true;

            //0.25�b��̎��ԍ��Ńt���O������
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            //�cHP���c���Ă��Ȃ���΃Q�[���I�[�o�[
            GameOver();
        }
    }

    void DamageEnd()
    {
        inDamage = false; //�_�Ń_���[�W�t���O������
        gameObject.GetComponent<SpriteRenderer>().enabled = true; //�v���C���[���m���ɕ\��
    }

    void GameOver()
    {
        //�Q�[���̏�Ԃ�ς���
        GameManager.gameState = GameState.gameover;

        //�Q�[���I�[�o�[���o
        GetComponent<CircleCollider2D>().enabled = false; //�����蔻��̖�����
        rbody.linearVelocity = Vector2.zero; //�������~�߂�
        rbody.gravityScale = 1.0f;�@//�d�͂̕���
        anime.SetTrigger("dead");   //���S�A�j���N���b�v�̔���
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); //��ɒ��ˏグ��
        Destroy(gameObject, 1.0f); //�P�b��ɑ��݂�����
    }

    //�X�|�b�g���C�g�̓���t���O�������Ă����烉�C�g������
    public void SpotLightCheck()
    {
        if(GameManager.hasSpotLight)spotLight.SetActive(true);
    }

    //VirtualPad�̓��͂ɔ������郁�\�b�h
    public void SetAxis(float virH,float virV)
    {
        //�ǂ��炩�̈����ɒl�������Ă����
        if(virH !=0 || virV != 0)
        {
            isViartual = true;
            axisH = virH;
            axisV = virV;
        }
        else //virtualPad���G���Ă��Ȃ��i����������0�j
        {
            isViartual = false;
        }
    }

    //����
    void HandleFootsteps()
    {
        //�v���C���[�������Ă����
        if (axisH != 0 || axisV != 0)
        {
            footstepTimer += Time.deltaTime; //���Ԍv��

            if (footstepTimer >= footstepInterval) //�C���^�[�o���`�F�b�N
            {
                SoundManager.instance.SEPlay(SEType.Walk);
                footstepTimer = 0;
            }
        }
        else //�����Ă��Ȃ���Ύ��Ԍv�����Z�b�g
        {
            footstepTimer = 0f;
        }
    }
}
