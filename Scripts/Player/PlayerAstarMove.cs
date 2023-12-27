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
        //player�̍ŏ��̈ʒu�����߂�
        this.transform.position = new Vector3(0, 1, 0);//15*15�̐^�񒆂�
        _currentNodeId = new Vector2Int(7, 7);//���ɍ��킹�ăX�^�[�g�̈ʒu�����킹��
        _oldCurrentNodeId = _currentNodeId;

        // Astar������
        //RouteManager.Instance.Initialize(tileSize);
        routeManager = new RouteManager();
        routeManager.Initialize(tileSize);

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
    }

    //�ʂ�Ȃ��Ƃ�����w��
    private void SetLock(Vector2Int nodeId, bool isLock)
    {
        //RouteManager.Instance.SetLock(nodeId, isLock);
        routeManager.SetLock(nodeId, isLock);
    }

    void Update()
    {
        //�Ƃ肠�����F�ς���
        if(isSelect == true) { this.GetComponent<Renderer>().material.color = Color.yellow; }

        #region �}�E�X�����N���b�N�����Ƃ�
        //�}�E�X�����N���b�N�����Ƃ�
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask(new string[] { "tile", "player", "house" });
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                //ray�ɓ��������I�u�W�F�N�g
                var hitObject = hit.collider.gameObject;
                //�I�u�W�F�N�g�����擾���ĕϐ��ɓ����
                string objectName = hit.collider.gameObject.name;
                //�N���b�N�����I�u�W�F�N�g�������𖼑O���璲�ׂ�
                string targetPlayer = "player";

                //�������N���b�N�����Ƃ�
                if (hitObject.name == this.gameObject.name)
                {
                    isSelect = true;
                }
                //ray�̓��������I�u�W�F�N�g�̖��O��"Player"���܂܂�Ă�����
                else if (objectName.Contains(targetPlayer) && hitObject.name != this.gameObject.name)
                {
                    isSelect = false;
                }

                //�������I�΂�Ă�����
                if (isSelect == true)
                {
                    #region �ړ�
                    //�ړ�
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
                    #endregion �ړ�
                }
            }
        }
        #endregion �}�E�X�����N���b�N�����Ƃ�


        if (isRouteLock == true) { this.GetComponent<Renderer>().material.color = Color.green; }
        else if(isRouteLock == false) { this.gameObject.GetComponent<Renderer>().material.color= Color.red; }

        //isMove = true�̎��ɓ���
        if (isMove == true) { Move(); }
    }

    void Goto(Vector2Int goalNodeId)
    {
        if (routeManager.SearchRoute(_currentNodeId, goalNodeId, _routeList))
        {
            // �ړ�
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
}
