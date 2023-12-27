using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    //�D�ɏ���Ă��邩
    private bool isOnShip = false;
    public bool IsOnShip { get { return isOnShip; } set { isOnShip = value; } }

    //�������邩
    private bool isLanding = false;
    public bool IsLanding { get {  return isLanding; } set { isLanding = value; } }

    //�������Ă��邩�ǂ���(���ɂ��邩)
    private bool isOnTile = false;
    public bool IsOnTile { get { return isOnTile; } set { isOnTile = value; } }

    //�����̖ړI�n
    private Vector3 destination = new Vector3();
    public Vector3 Destination { get { return destination; } set { destination = value; } }

    public GameObject searchRange;
    private GameObject search;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject�𐶐�
        search = Instantiate(searchRange);
        //search.transform.parent =�@transform;//enemy�ɒǏ]������
    }

    // Update is called once per frame
    void Update()
    {
        //Search��enemy�ɒǏ]������
        search.transform.position = this.transform.position;
        //�D�̏ォ�璅���܂�
        Landing();
        Debug.Log("X:" + (int)this.transform.position.x + " Z:" + (int)this.transform.position.z);
    }

    //�D�̏ォ�璅���܂�
    private void Landing() 
    {
        if(isLanding == true) 
        {
            //�D���������Ă���1�b���enemy������
            Invoke("LandingMove", 1.0f);
        }
    }

    private void LandingMove()
    {
        float speed = 0.5f;
        //�D�̏ォ�璅���܂�
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //�ړI�n�ɂ�����
        if(transform.position == destination) 
        {
            isLanding = false;
            isOnTile = true;
        }
    }

}
