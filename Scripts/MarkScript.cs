using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkScript : MonoBehaviour
{
    private Vector3 markPos;

    public Vector3 MarkPos
    {
        get { return markPos; }

        set { markPos = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //最初は見えない位置に
        markPos = new Vector3(0, -20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;
        pos.x = markPos.x;    // x座標へ0.01加算
        pos.y = markPos.y;    // y座標へ0.01加算
        pos.z = markPos.z;    // z座標へ0.01加算

        myTransform.position = pos;  // 座標を設定
    }


}
