using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedGameObject : MonoBehaviour
{
    public Camera camera_object; //�J�������擾
    private RaycastHit hit; //���C�L���X�g�������������̂��擾������ꕨ

    private Vector2Int start;
    private Vector2Int goal;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //�}�E�X���N���b�N���ꂽ��
        {
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition); //�}�E�X�̃|�W�V�������擾����Ray�ɑ��

            if (Physics.Raycast(ray, out hit))  //�}�E�X�̃|�W�V��������Ray�𓊂��ĉ����ɓ���������hit�ɓ����
            {
                //�I�u�W�F�N�g�����擾���ĕϐ��ɓ����
                string objectName = hit.collider.gameObject.name; 
                //�N���b�N�����I�u�W�F�N�g�������𖼑O���璲�ׂ�
                string targetPlayer = "player";
                string targetGround = "floor";

                //�R���\�[���ɕ\��
                Debug.Log(objectName); //�I�u�W�F�N�g��
                Debug.Log(hit.transform.position);
               
                if (objectName.Contains(targetPlayer))//�N���b�N�����I�u�W�F�N�g��Player�Ȃ�
                {
                    
                }
                else if (objectName.Contains(targetGround))//�N���b�N�����I�u�W�F�N�g��floor�Ȃ�
                {
                    
                }
            }
            
        }
    }

    
}
