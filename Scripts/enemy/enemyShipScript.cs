using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class enemyShipScript : MonoBehaviour
{
    public GameObject enemy;

    //オブジェクトの空の入れ物
    private GameObject shell;
    //船の終着点
    private Vector3 endPos = new Vector3();
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    //着陸の目的地
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
        //船に乗るenemyの初期設定
        SetUp();
        //船がスタート位置から目的地に着くまで
        Move();
    }

    private void SetUp()
    {
        //船に乗せるenemyの初期設定をここでやる
        if (isSetUp == true)
        {
            //int enemyNum = UnityEngine.Random.Range(1, 7);//enemyの数(1～6体)
            //int enemyNum = 1;//とりあえず1

            //int enemyJob = UnityEngine.Random.Range(1, 4);//enemyの種類(1～3種)
            //int enemyJob = 1;//とりあえず1

            //GameObject enemyを生成
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
            //船の速さ
            float speed = 1.0f;
            //enemyManagerから受け取ったendPosに向かう
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

            //乗っているenemy
            shell.transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);//まだ1体なのでy軸だけ変える

            //目的地についたら
            if(transform.position == endPos) 
            {
                isMove = false;
                //船に乗っているenemyを降ろす
                enemyScript enemyScript_ = new enemyScript();
                enemyScript_ = shell.GetComponent<enemyScript>();
                enemyScript_.Destination = destination;
                enemyScript_.IsLanding = true;
            }
        }
    }
}
