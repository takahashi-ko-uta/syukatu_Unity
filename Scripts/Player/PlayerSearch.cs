using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    //索敵範囲に敵がいるか
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
    
    //重なっているとき
    void OnTriggerStay(Collider other)
    {
        //索敵範囲に敵がいたら
        if (other.gameObject.name == "Enemy(Clone)")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.1f);
            GetComponent<Renderer>().material.color = color;

            isAttack = true;
        }
        Debug.Log("Hit: " + other.gameObject.name);
    }

    //離れたとき
    void OnTriggerExit(Collider other) 
    {
        //索敵範囲に敵がいたら
        if (other.gameObject.name == "Enemy(Clone)")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.05f);
            GetComponent<Renderer>().material.color = color;

            isAttack = false;
        }
    }
}
