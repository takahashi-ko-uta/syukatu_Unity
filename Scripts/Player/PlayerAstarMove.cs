using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAstarMove : MonoBehaviour
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

    [SerializeField]
    private Camera _camera;

    public int tileSize = 15;

    private Vector2Int _currentNodeId;
    private Vector2Int _oldCurrentNodeId;
    public float walkSpeed = 0.1f;
    List<Vector2Int> _routeList = new List<Vector2Int>();

    private RouteManager routeManager;

    private bool isSelect = false;
    
    private int count = 0;

    private bool isMove;
    private bool isRouteLock;


    void Start()
    {
        //playerの最初の位置を決める
        this.transform.position = new Vector3(0, 1, 0);//15*15の真ん中に
        _currentNodeId = new Vector2Int(7, 7);//↑に合わせてスタートの位置も合わせる
        _oldCurrentNodeId = _currentNodeId;

        // Astar初期化
        //RouteManager.Instance.Initialize(tileSize);
        routeManager = new RouteManager();
        routeManager.Initialize(tileSize);

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
    }

    //通れないところを指定
    private void SetLock(Vector2Int nodeId, bool isLock)
    {
        //RouteManager.Instance.SetLock(nodeId, isLock);
        routeManager.SetLock(nodeId, isLock);
    }

    void Update()
    {
        //とりあえず色変える
        if(isSelect == true) { this.GetComponent<Renderer>().material.color = Color.yellow; }

        #region マウスを左クリックしたとき
        //マウスを左クリックしたとき
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask(new string[] { "tile", "player", "house" });
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                //rayに当たったオブジェクト
                var hitObject = hit.collider.gameObject;
                //オブジェクト名を取得して変数に入れる
                string objectName = hit.collider.gameObject.name;
                //クリックしたオブジェクトが何かを名前から調べる
                string targetPlayer = "player";

                //自分をクリックしたとき
                if (hitObject.name == this.gameObject.name)
                {
                    isSelect = true;
                }
                //rayの当たったオブジェクトの名前に"Player"が含まれていたら
                else if (objectName.Contains(targetPlayer) && hitObject.name != this.gameObject.name)
                {
                    isSelect = false;
                }

                //自分が選ばれていたら
                if (isSelect == true)
                {
                    #region 移動
                    //移動
                    if (isRouteLock == false)
                    {
                        var tile = hitObject.GetComponent<Tile>();
                        Goto(tile.NodeId);
                        if (_oldCurrentNodeId != tile.NodeId)
                        {
                            isMove = true;
                            isRouteLock = true;
                            _oldCurrentNodeId = _currentNodeId;
                        }
                    }
                    #endregion 移動
                }
            }
        }
        #endregion マウスを左クリックしたとき


        if (isRouteLock == true) { this.GetComponent<Renderer>().material.color = Color.green; }
        else if(isRouteLock == false) { this.gameObject.GetComponent<Renderer>().material.color= Color.red; }

        //isMove = trueの時に動く
        if (isMove == true) { Move(); }
    }

    void Goto(Vector2Int goalNodeId)
    {
        if (routeManager.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // 移動
            //StartMove();
        }
        _currentNodeId = goalNodeId;
    }



    #region
    //private Coroutine _moveCoroutine;

    //private void StartMove()
    //{
    //    //_moveTween?.Kill();
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
}
