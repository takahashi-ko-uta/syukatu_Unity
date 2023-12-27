using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyAstarMove : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapText;
    enum MAP_TYPE
    {
        SPACE,    //0
        GROUND,   //1
        HOUSE     //2
    }
    private MAP_TYPE[,] mapTable;

    public int tileSize = 15;

    private Vector2Int _currentNodeId;
    private Vector2Int _oldCurrentNodeId;
    public float walkSpeed = 0.1f;
    List<Vector2Int> _routeList = new List<Vector2Int>();

    private RouteManager routeManager;

    private�@bool isSearchHouse = false; 
    private int count = 0;
    private bool isMove;
    private bool isRouteLock;

    private Vector2Int goal = new Vector2Int();
    void Start()
    {
        // Astar������
        routeManager = new RouteManager();
        routeManager.Initialize(tileSize);

        #region �}�b�v��ǂݍ���
        //�}�b�v��ǂݍ���
        LoadMapData();

        for (int z = 0; z < mapTable.GetLength(1); z++)
        {
            for (int x = 0; x < mapTable.GetLength(0); x++)
            {
                if ((int)mapTable[x, z] != 1) //1�ȊO(�n�ʈȊO)�Ȃ�SetLock�ɂ���
                {
                    //�ʂ�Ȃ��ꏊ
                    SetLock(new Vector2Int(x, z), true);
                }
            }
        }
        #endregion �}�b�v��ǂݍ���
    }

    //�ʂ�Ȃ��Ƃ�����w��
    private void SetLock(Vector2Int nodeId, bool isLock)
    {
        routeManager.SetLock(nodeId, isLock);
    }

    void Update()
    {
        #region enemy���������Ă�����(�����ݒ�)
        //enemy���������Ă�����(�����ݒ�)
        enemyScript enemyScript_ = GetComponent<enemyScript>();
        if (enemyScript_.IsOnTile == true)
        {
            //Enemy�̍ŏ��̈ʒu��ID���Z�b�g
            _currentNodeId = ChangeVec3ToId(transform.position);//id�ɕϊ����Ȃ���_��
            _oldCurrentNodeId = _currentNodeId;

            isSearchHouse = true;
            enemyScript_.IsOnTile = false;
        }
        #endregion

        #region �߂��̉Ƃ܂ōs��
        //�߂��̉Ƃ܂ōs��
        if (isSearchHouse == true)
        {
            //�S�[��(�߂��̉�)��T��
            goal = SearchHouse();
            Goto(goal);
            isMove = true;
            isRouteLock = true;
            isSearchHouse = false;
        }
        #endregion

        //isMove = true�̎��ɓ���
        if (isMove == true) { Move(); }

        //�߂��̉Ƃɂ�����U��
        HouseAttack(goal);
    }

    private void Goto(Vector2Int goalNodeId)
    {
        if (routeManager.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // �ړ�
            //StartMove();---
        }

        _currentNodeId = goalNodeId;
    }

    //�Ƃւ̍U��
    private void HouseAttack(Vector2Int goalHouse_) 
    {
        Vector2Int nowId = ChangeVec3ToId(transform.position);
        if (goalHouse_ == nowId)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.clear;
        }
    }

    //plyer�ւ̍U��
    private void PlayerAttack(Vector2Int goalPlayer_)
    {
        Vector2Int nowId = ChangeVec3ToId(transform.position);

    }


    #region
    //private Coroutine _moveCoroutine;

    //private void StartMove()
    //{
    //    if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
    //    _moveCoroutine = StartCoroutine(_StartMove());
    //}

    //IEnumerator _StartMove()
    //{
    //    var wait = new WaitForSeconds(walkSpeed);

    //    for (int i = 0; i < _routeList.Count; i++)
    //    {
    //        var nodeId = _routeList[i];
    //        var goal = new Vector3(nodeId.x - tileSize / 2, 1, nodeId.y - tileSize / 2);
    //        //�i��
    //        this.transform.position = goal; // �ړI�̈ʒu�Ɉړ�

    //        yield return wait;
    //    }
    //    _moveCoroutine = null;
    //}
    #endregion

    private void Move()
    {
        var nodeId = _routeList[count];
        var goal = new Vector3(nodeId.x - tileSize / 2, 1, nodeId.y - tileSize / 2);
        float speed = 2.0f;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, goal, step);

        if (transform.position == goal) { count++; }

        if (_routeList.Count == count)
        {
            count = 0;
            isMove = false;
            isRouteLock = false;
        }
    }


    //�e�L�X�g�t�@�C������}�b�v�̓ǂݍ���
    private void LoadMapData()
    {
        string[] mapLines = mapText.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //�s�̐�
        int row = mapLines.Length;
        //��̐�
        int col = mapLines[0].Split(new char[] { ',' }).Length;
        //������
        mapTable = new MAP_TYPE[col, row];

        //�ǉ��@�s�̐��������[�v
        for (int y = 0; y < row; y++)
        {
            //1�s���J���}��؂�ŕ���
            string[] mapValues = mapLines[y].Split(new char[] { ',' });
            //��̐��������[�v
            for (int x = 0; x < col; x++)
            {
                //mapValues��x�Ԗڂ�MAP_TYPE�ɃL���X�g����mapTable[x,y]�Ԗڂɑ��
                mapTable[x, y] = (MAP_TYPE)int.Parse(mapValues[x]);
            }
        }
    }

    //�w�肳�ꂽ�^�O�̒��ōł��߂����̂��擾
    private GameObject serchObject(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;           //�����p�ꎞ�ϐ�
        float nearDis = 0;          //�ł��߂��I�u�W�F�N�g�̋���
 
        GameObject targetObj = null; //�I�u�W�F�N�g

        //�^�O�w�肳�ꂽ�I�u�W�F�N�g��z��Ŏ擾����
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //���g�Ǝ擾�����I�u�W�F�N�g�̋������擾
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //�I�u�W�F�N�g�̋������߂����A����0�ł���΃I�u�W�F�N�g�����擾
            //�ꎞ�ϐ��ɋ������i�[
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //�ł��߂������I�u�W�F�N�g��Ԃ�
        return targetObj;
    }

    private Vector2Int SearchHouse() 
    {
        Vector2Int goal_ = new Vector2Int();
        //�S�[��(�߂��̉�)��T��
        GameObject nearHouse = serchObject(gameObject, "house");
        houseScript houseScript_;
        houseScript_ = nearHouse.GetComponent<houseScript>();

        //���S�ɏd�Ȃ�ƃ��[�g�����Ȃ����߁A�Ƃ肠����X+1���炷
        Vector2Int nowId = ChangeVec3ToId(this.transform.position);
        Vector2Int houseId = houseScript_.NodeId;

        if (Mathf.Abs(nowId.x - houseId.x) < Mathf.Abs(nowId.y - houseId.y))
        {
            if(nowId.y < houseId.y) 
            {
                goal_ = new Vector2Int(houseId.x, houseId.y - 1);
            }
            else if(nowId.y > houseId.y) 
            {
                goal_ = new Vector2Int(houseId.x, houseId.y + 1);
            }
        }
        else 
        {
            if (nowId.x < houseId.x)
            {
                goal_ = new Vector2Int(houseId.x - 1, houseId.y);
            }
            else if (nowId.x > houseId.x)
            {
                goal_ = new Vector2Int(houseId.x + 1, houseId.y);
            }
        }


        //goal_ = new Vector2Int(houseScript_.NodeId.x + 1, houseScript_.NdodeId.y);
        
        return goal_; 
    }

    private Vector2Int ChangeVec3ToId(Vector3 vec3) 
    {
        Vector2Int convert = new Vector2Int();
        
        convert = new Vector2Int(((int)vec3.x + 7), ((int)vec3.z + 7));

        return convert;
    }

}
