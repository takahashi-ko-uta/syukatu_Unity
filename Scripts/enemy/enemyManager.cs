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
            Vector3 startPos = new Vector3();   //船のスタート位置
            Vector3 destination = new Vector3();//目的地(enemyで使う)
            Vector3 endPos = new Vector3();     //船の終着点

            //出現する方角を決める(1～4の乱数を生成)
            int direction = UnityEngine.Random.Range(1, 5);

            //スポーンの座標の乱数
            float random = UnityEngine.Random.Range(-21, 21);

            //directionよって、startPosとendPosを決める
            switch (direction)
            {
                case 1: //+z方向 
                    startPos = new Vector3(random, 0, 30);
                    endPos = new Vector3(ChangeRandomToEndPos(random), 0, 8);
                    destination = new Vector3(ChangeRandomToEndPos(random), 1, 7);
                    break;
                case 2: //+x方向
                    startPos = new Vector3(30, 0, random);
                    endPos = new Vector3(8, 0, ChangeRandomToEndPos(random));
                    destination = new Vector3(7, 1, ChangeRandomToEndPos(random));
                    break;
                case 3: //-z方向
                    startPos = new Vector3(random, 0, -30);
                    endPos = new Vector3(ChangeRandomToEndPos(random), 0, -8);
                    destination = new Vector3(ChangeRandomToEndPos(random), 1, -7);
                    break;
                case 4: //-x方向
                    startPos = new Vector3(-30, 0, random);
                    endPos = new Vector3(-8, 0, ChangeRandomToEndPos(random));
                    destination = new Vector3(-7, 1, ChangeRandomToEndPos(random));
                    break;
            }

            //startPosの位置にオブジェクトenemyShipを配置
            GameObject shell = Instantiate(ship, startPos, Quaternion.identity);

            //enemyShipにendPosを渡し、isMoveをtureへ
            enemyShipScript shipScript = new enemyShipScript();
            shipScript = shell.GetComponent<enemyShipScript>();
            shipScript.EndPos = endPos;
            shipScript.Destination = destination;
            shipScript.IsSetUp = true;
        }
    }

    //randomをendPos用に変える
    private float ChangeRandomToEndPos(float random_) 
    {
        float value;
        if (UnityEngine.Mathf.Sign(random_) == 1) //randomが正の場合
        {
            value = Mathf.Floor(random_ / 3);
        }
        else //randomが負の場合
        {
            value = Mathf.Ceil(random_ / 3);
        }

        return value;
    }
}
