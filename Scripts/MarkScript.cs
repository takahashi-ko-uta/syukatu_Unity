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
        //�ŏ��͌����Ȃ��ʒu��
        markPos = new Vector3(0, -20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���W���擾
        Vector3 pos = myTransform.position;
        pos.x = markPos.x;    // x���W��0.01���Z
        pos.y = markPos.y;    // y���W��0.01���Z
        pos.z = markPos.z;    // z���W��0.01���Z

        myTransform.position = pos;  // ���W��ݒ�
    }


}
