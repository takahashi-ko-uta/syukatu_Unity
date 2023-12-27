using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    //マップテキスト
    public TextAsset mapText;

    //家のオブジェクト
    public houseScript houseObject;

    //地面のオブジェクト
    public Tile tileObject;
    private Tile[,] tiles;

    //地面をチェック柄に
    public Material _pattern;

    private int mapSize = 15;//15*15

    enum MAP_TYPE
    {
        SPACE,  //0
        GROUND, //1
        HOUSE   //2
    }

    private MAP_TYPE[,] mapTable;

    void Start()
    {
        tiles = new Tile[mapSize, mapSize];
        //テキスト読み込み
        LoadMapData();
        //マップ生成
        CreateMap();
    }

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

    private void CreateMap()
    {
        int count = 0;
        //mapTableの行のループ
        for (int z = 0; z < mapTable.GetLength(1); z++)
        {
            //mapTableの列のループ
            for (int x = 0; x < mapTable.GetLength(0); x++)
            {
                //GameObjectを生成
                var tile = Instantiate(tileObject) as Tile;

                tiles[x, z] = tile;
                tile.SetNodeId(new Vector2Int(x, z));

                //生成したGameObjectの設定
                switch (mapTable[x, z]) 
                {
                    //地面
                    case MAP_TYPE.GROUND:
                        tile.transform.position = new Vector3(x - mapSize / 2, 0.0f, z - mapSize / 2);
                        break;

                    //空白
                    case MAP_TYPE.SPACE:
                        tile.transform.position = new Vector3(x - mapSize / 2, -3.0f, z - mapSize / 2);
                        break;

                    //家
                    case MAP_TYPE.HOUSE:
                        //tileは1(地面)と同じ
                        tile.transform.position = new Vector3(x - mapSize / 2, 0.0f, z - mapSize / 2);
                        //このtileの上にhouseObjectを配置
                        var house = Instantiate(houseObject);
                        house.SetNodeId(new Vector2Int(x, z));
                        house.transform.position = new Vector3(x - mapSize / 2, 1.0f, z - mapSize / 2);
                        break;
                }

                //1(地面)の色をチェックに
                if (count++ % 2 == 0 && (int)mapTable[x,z] == 1)
                {
                    tile.GetComponent<Renderer>().material = _pattern;
                }
            }
        }
    }
}

