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

    private　bool isSearchHouse = false; 
    private int count = 0;
    private bool isMove;
    private bool isRouteLock;

    private Vector2Int goal = new Vector2Int();
    void Start()
    {
        // Astar初期化
        routeManager = new RouteManager();
        routeManager.Initialize(tileSize);

        #region マップを読み込む
        //マップを読み込む
        LoadMapData();

        for (int z = 0; z < mapTable.GetLength(1); z++)
        {
            for (int x = 0; x < mapTable.GetLength(0); x++)
            {
                if ((int)mapTable[x, z] != 1) //1以外(地面以外)ならSetLockにする
                {
                    //通れない場所
                    SetLock(new Vector2Int(x, z), true);
                }
            }
        }
        #endregion マップを読み込む
    }

    //通れないところを指定
    private void SetLock(Vector2Int nodeId, bool isLock)
    {
        routeManager.SetLock(nodeId, isLock);
    }

    void Update()
    {
        #region enemyが着陸していたら(初期設定)
        //enemyが着陸していたら(初期設定)
        enemyScript enemyScript_ = GetComponent<enemyScript>();
        if (enemyScript_.IsOnTile == true)
        {
            //Enemyの最初の位置のIDをセット
            _currentNodeId = ChangeVec3ToId(transform.position);//idに変換しなきゃダメ
            _oldCurrentNodeId = _currentNodeId;

            isSearchHouse = true;
            enemyScript_.IsOnTile = false;
        }
        #endregion

        #region 近くの家まで行く
        //近くの家まで行く
        if (isSearchHouse == true)
        {
            //ゴール(近くの家)を探す
            goal = SearchHouse();
            Goto(goal);
            isMove = true;
            isRouteLock = true;
            isSearchHouse = false;
        }
        #endregion

        //isMove = trueの時に動く
        if (isMove == true) { Move(); }

        //近くの家についたら攻撃
        HouseAttack(goal);
    }

    private void Goto(Vector2Int goalNodeId)
    {
        if (routeManager.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // 移動
            //StartMove();---
        }

        _currentNodeId = goalNodeId;
    }

    //家への攻撃
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

    //plyerへの攻撃
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
    //        //進む
    //        this.transform.position = goal; // 目的の位置に移動

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


    //テキストファイルからマップの読み込み
    private void LoadMapData()
    {
        string[] mapLines = mapText.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //行の数
        int row = mapLines.Length;
        //列の数
        int col = mapLines[0].Split(new char[] { ',' }).Length;
        //初期化
        mapTable = new MAP_TYPE[col, row];

        //追加　行の数だけループ
        for (int y = 0; y < row; y++)
        {
            //1行をカンマ区切りで分割
            string[] mapValues = mapLines[y].Split(new char[] { ',' });
            //列の数だけループ
            for (int x = 0; x < col; x++)
            {
                //mapValuesのx番目をMAP_TYPEにキャストしてmapTable[x,y]番目に代入
                mapTable[x, y] = (MAP_TYPE)int.Parse(mapValues[x]);
            }
        }
    }

    //指定されたタグの中で最も近いものを取得
    private GameObject serchObject(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
 
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        return targetObj;
    }

    private Vector2Int SearchHouse() 
    {
        Vector2Int goal_ = new Vector2Int();
        //ゴール(近くの家)を探す
        GameObject nearHouse = serchObject(gameObject, "house");
        houseScript houseScript_;
        houseScript_ = nearHouse.GetComponent<houseScript>();

        //完全に重なるとルートが作れないため、とりあえずX+1ずらす
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
