using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class enemyShipScript : MonoBehaviour
{
    public GameObject enemy;

    //�I�u�W�F�N�g�̋�̓��ꕨ
    private GameObject shell;
    //�D�̏I���_
    private Vector3 endPos = new Vector3();
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    //�����̖ړI�n
    private Vector3 destination = new Vector3();
    public Vector3 Destination { get { return destination; } set { destination = value; } }

    private bool isSetUp = false;
    public bool IsSetUp { get { return isSetUp; } set { isSetUp = value; } }

    private bool isMove = false;
    public bool IsMove { get { return isMove; } set { isMove = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�D�ɏ��enemy�̏����ݒ�
        SetUp();
        //�D���X�^�[�g�ʒu����ړI�n�ɒ����܂�
        Move();
    }

    private void SetUp()
    {
        //�D�ɏ悹��enemy�̏����ݒ�������ł��
        if (isSetUp == true)
        {
            //int enemyNum = UnityEngine.Random.Range(1, 7);//enemy�̐�(1�`6��)
            //int enemyNum = 1;//�Ƃ肠����1

            //int enemyJob = UnityEngine.Random.Range(1, 4);//enemy�̎��(1�`3��)
            //int enemyJob = 1;//�Ƃ肠����1

            //GameObject enemy�𐶐�
            Vector3 enemyPos = transform.position;
            shell = Instantiate(enemy, new Vector3(enemyPos.x, 0, enemyPos.z), Quaternion.identity);


            if (isSetUp == true)
            {
                isSetUp = false;
                isMove = true;
            }
        }
    }

    private void Move()
    {
        if (isMove == true)
        {
            //�D�̑���
            float speed = 1.0f;
            //enemyManager����󂯎����endPos�Ɍ�����
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

            //����Ă���enemy
            shell.transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);//�܂�1�̂Ȃ̂�y�������ς���

            //�ړI�n�ɂ�����
            if(transform.position == endPos) 
            {
                isMove = false;
                //�D�ɏ���Ă���enemy���~�낷
                enemyScript enemyScript_ = new enemyScript();
                enemyScript_ = shell.GetComponent<enemyScript>();
                enemyScript_.Destination = destination;
                enemyScript_.IsLanding = true;
            }
        }
    }
}
