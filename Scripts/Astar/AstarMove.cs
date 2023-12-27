using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
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

    [SerializeField]
    private Transform _playerMoveTransform;

    [SerializeField]
    private Transform _playerRotateTransform;


    public int tileSize = 15;

    private Vector2Int _currentNodeId;
    public float walkSpeed = 1.0f;
    List<Vector2Int> _routeList = new List<Vector2Int>();
    
    private RouteManager routeManager;

    void Start()
    {
        //playerの最初の位置を決める
        _playerMoveTransform.position = new Vector3(0, 1, 0);//15*15の真ん中に
        _currentNodeId = new Vector2Int(7, 7);//↑に合わせてスタートの位置も合わせる

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
                if((int)mapTable[x, z] != 1) //1以外(地面以外)ならSetLockにする
                {
                    //通れない場所
                    SetLock(new Vector2Int(x, z), true);
                }
            }
        }
    }

    private void SetLock(Vector2Int nodeId, bool isLock)
    {
        //RouteManager.Instance.SetLock(nodeId, isLock);
        routeManager.SetLock(nodeId, isLock);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                var hitObject = hit.collider.gameObject;
                var tile = hitObject.GetComponent<Tile>();
                Goto(tile.NodeId);
            }
        }
    }

    void Goto(Vector2Int goalNodeId)
    {
        //if (RouteManager.Instance.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        if(routeManager.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // 移動
            StartMove();
        }

        _currentNodeId = goalNodeId;
    }

    private Coroutine _moveCoroutine;

    private void StartMove()
    {
        //_moveTween?.Kill();
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(_StartMove());
    }

    IEnumerator _StartMove()
    {
        var wait = new WaitForSeconds(walkSpeed);
        for (int i = 0; i < _routeList.Count; i++)
        {
            var nodeId = _routeList[i];
            var goal = new Vector3(nodeId.x - tileSize / 2, 1, nodeId.y - tileSize / 2);
            //進む
            Vector3 targetPosition = new Vector3(goal.x, goal.y, goal.z); // 目的の位置の座標を指定
            _playerMoveTransform.position = targetPosition; // 目的の位置に移動

            //進んでいる方向に回転
            //_playerRotateTransform.localRotation = Quaternion.LookRotation(goal - _playerMoveTransform.localPosition);

            yield return wait;
        }

        _moveCoroutine = null;
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

        //行の数だけループ
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
