using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //���G�͈͂ɓG�����邩
    private bool isAttack = false;
    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }


    // Start is called before the first frame update
    void Start()
    {
        Color color = GetComponent<Renderer>().material.color;
        color = new Color(200, 0, 30, 0.05f);
        GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //�d�Ȃ��Ă���Ƃ�
    void OnTriggerStay(Collider other)
    {
        //���G�͈͂ɓG��������
        if (other.gameObject.name == "Enemy(Clone)")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.1f);
            GetComponent<Renderer>().material.color = color;

            isAttack = true;
        }
        Debug.Log("Hit: " + other.gameObject.name);
    }

    //���ꂽ�Ƃ�
    void OnTriggerExit(Collider other) 
    {
        //���G�͈͂ɓG��������
        if (other.gameObject.name == "Player1")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.05f);
            GetComponent<Renderer>().material.color = color;

            isAttack = false;
        }
    }
}
