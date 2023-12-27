using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedGameObject : MonoBehaviour
{
    public Camera camera_object; //カメラを取得
    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物

    private Vector2Int start;
    private Vector2Int goal;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

            if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
            {
                //オブジェクト名を取得して変数に入れる
                string objectName = hit.collider.gameObject.name; 
                //クリックしたオブジェクトが何かを名前から調べる
                string targetPlayer = "player";
                string targetGround = "floor";

                //コンソールに表示
                Debug.Log(objectName); //オブジェクト名
                Debug.Log(hit.transform.position);
               
                if (objectName.Contains(targetPlayer))//クリックしたオブジェクトがPlayerなら
                {
                    
                }
                else if (objectName.Contains(targetGround))//クリックしたオブジェクトがfloorなら
                {
                    
                }
            }
            
        }
    }

    
}
