using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //õ“G”ÍˆÍ‚É“G‚ª‚¢‚é‚©
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
    
    //d‚È‚Á‚Ä‚¢‚é‚Æ‚«
    void OnTriggerStay(Collider other)
    {
        //õ“G”ÍˆÍ‚É“G‚ª‚¢‚½‚ç
        if (other.gameObject.name == "Enemy(Clone)")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.1f);
            GetComponent<Renderer>().material.color = color;

            isAttack = true;
        }
        Debug.Log("Hit: " + other.gameObject.name);
    }

    //—£‚ê‚½‚Æ‚«
    void OnTriggerExit(Collider other) 
    {
        //õ“G”ÍˆÍ‚É“G‚ª‚¢‚½‚ç
        if (other.gameObject.name == "Player1")
        {
            Color color = GetComponent<Renderer>().material.color;
            color = new Color(200, 0, 30, 0.05f);
            GetComponent<Renderer>().material.color = color;

            isAttack = false;
        }
    }
}
