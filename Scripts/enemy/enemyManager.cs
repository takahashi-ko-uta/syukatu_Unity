using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public GameObject ship;

    private Vector3 SpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            Vector3 startPos = new Vector3();   //�D�̃X�^�[�g�ʒu
            Vector3 destination = new Vector3();//�ړI�n(enemy�Ŏg��)
            Vector3 endPos = new Vector3();     //�D�̏I���_

            //�o��������p�����߂�(1�`4�̗����𐶐�)
            int direction = UnityEngine.Random.Range(1, 5);

            //�X�|�[���̍��W�̗���
            float random = UnityEngine.Random.Range(-21, 21);

            //direction����āAstartPos��endPos�����߂�
            switch (direction)
            {
                case 1: //+z���� 
                    startPos = new Vector3(random, 0, 30);
                    endPos = new Vector3(ChangeRandomToEndPos(random), 0, 8);
                    destination = new Vector3(ChangeRandomToEndPos(random), 1, 7);
                    break;
                case 2: //+x����
                    startPos = new Vector3(30, 0, random);
                    endPos = new Vector3(8, 0, ChangeRandomToEndPos(random));
                    destination = new Vector3(7, 1, ChangeRandomToEndPos(random));
                    break;
                case 3: //-z����
                    startPos = new Vector3(random, 0, -30);
                    endPos = new Vector3(ChangeRandomToEndPos(random), 0, -8);
                    destination = new Vector3(ChangeRandomToEndPos(random), 1, -7);
                    break;
                case 4: //-x����
                    startPos = new Vector3(-30, 0, random);
                    endPos = new Vector3(-8, 0, ChangeRandomToEndPos(random));
                    destination = new Vector3(-7, 1, ChangeRandomToEndPos(random));
                    break;
            }

            //startPos�̈ʒu�ɃI�u�W�F�N�genemyShip��z�u
            GameObject shell = Instantiate(ship, startPos, Quaternion.identity);

            //enemyShip��endPos��n���AisMove��ture��
            enemyShipScript shipScript = new enemyShipScript();
            shipScript = shell.GetComponent<enemyShipScript>();
            shipScript.EndPos = endPos;
            shipScript.Destination = destination;
            shipScript.IsSetUp = true;
        }
    }

    //random��endPos�p�ɕς���
    private float ChangeRandomToEndPos(float random_) 
    {
        float value;
        if (UnityEngine.Mathf.Sign(random_) == 1) //random�����̏ꍇ
        {
            value = Mathf.Floor(random_ / 3);
        }
        else //random�����̏ꍇ
        {
            value = Mathf.Ceil(random_ / 3);
        }

        return value;
    }
}
