using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //注視点にするオブジェクト
    public GameObject targetObject;
    //回転させるスピード
    public float rotateSpeed = 1.0f;

    //初期化
    void Start()
    {

    }

    //更新処理
    void Update()
    {
        // カメラ
        Camera camera = Camera.main;
        // 注視点を設定
        this.transform.LookAt(targetObject.transform);

        //カメラの移動
        Move(camera);
        //カメラのズーム
        Zoom(camera);
    }

    //カメラの移動
    void Move(Camera camera_)
    {
        //マウスの移動量を取得
        float mouseX_delta = Input.GetAxis("Mouse X");
        float mouseY_delta = Input.GetAxis("Mouse Y");
        float angle = 0;

        //ホイールボタンが押されたら
        if (Input.GetMouseButton(2))
        {
            if (mouseX_delta > 0) //マウスを右に動かしたら
            {
                angle = 1 * rotateSpeed;
            }
            else if (mouseX_delta < 0) //マウスを左に動かしたら
            {
                angle = -1 * rotateSpeed;
            }
        }

        //target位置情報
        Vector3 targetPos = targetObject.transform.position;
        //カメラを回転させる
        transform.RotateAround(targetPos, Vector3.up, angle);
    }

    //カメラのズーム
    void Zoom(Camera camera_)
    {
        //ホイールの回転を取得
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel > 0)//上に回転したら
        {
            camera_.gameObject.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f));

        }
        else if(wheel < 0) //下に回転したら
        {
            camera_.gameObject.transform.Translate(new Vector3(0.0f, 0.0f, -1.0f));
        }
        //Debug.Log(camera_.gameObject.transform.Translate.Vector3.z);
    }
}
