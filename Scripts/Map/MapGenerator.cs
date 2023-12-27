using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    //�}�b�v�e�L�X�g
    public TextAsset mapText;

    //�Ƃ̃I�u�W�F�N�g
    public houseScript houseObject;

    //�n�ʂ̃I�u�W�F�N�g
    public Tile tileObject;
    private Tile[,] tiles;

    //�n�ʂ��`�F�b�N����
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
        //�e�L�X�g�ǂݍ���
        LoadMapData();
        //�}�b�v����
        CreateMap();
    }

    private void LoadMapData()
    {
        string[] mapLines = mapText.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //�s�̐�
        int row = mapLines.Length;
        //��̐�
        int col = mapLines[0].Split(new char[] { ',' }).Length;
        //������
        mapTable = new MAP_TYPE[col, row];

        //�s�̐��������[�v
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

    private void CreateMap()
    {
        int count = 0;
        //mapTable�̍s�̃��[�v
        for (int z = 0; z < mapTable.GetLength(1); z++)
        {
            //mapTable�̗�̃��[�v
            for (int x = 0; x < mapTable.GetLength(0); x++)
            {
                //GameObject�𐶐�
                var tile = Instantiate(tileObject) as Tile;

                tiles[x, z] = tile;
                tile.SetNodeId(new Vector2Int(x, z));

                //��������GameObject�̐ݒ�
                switch (mapTable[x, z]) 
                {
                    //�n��
                    case MAP_TYPE.GROUND:
                        tile.transform.position = new Vector3(x - mapSize / 2, 0.0f, z - mapSize / 2);
                        break;

                    //��
                    case MAP_TYPE.SPACE:
                        tile.transform.position = new Vector3(x - mapSize / 2, -3.0f, z - mapSize / 2);
                        break;

                    //��
                    case MAP_TYPE.HOUSE:
                        //tile��1(�n��)�Ɠ���
                        tile.transform.position = new Vector3(x - mapSize / 2, 0.0f, z - mapSize / 2);
                        //����tile�̏��houseObject��z�u
                        var house = Instantiate(houseObject);
                        house.SetNodeId(new Vector2Int(x, z));
                        house.transform.position = new Vector3(x - mapSize / 2, 1.0f, z - mapSize / 2);
                        break;
                }

                //1(�n��)�̐F���`�F�b�N��
                if (count++ % 2 == 0 && (int)mapTable[x,z] == 1)
                {
                    tile.GetComponent<Renderer>().material = _pattern;
                }
            }
        }
    }
}

