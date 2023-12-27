using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    //船に乗っているか
    private bool isOnShip = false;
    public bool IsOnShip { get { return isOnShip; } set { isOnShip = value; } }

    //着陸するか
    private bool isLanding = false;
    public bool IsLanding { get {  return isLanding; } set { isLanding = value; } }

    //着陸しているかどうか(陸にいるか)
    private bool isOnTile = false;
    public bool IsOnTile { get { return isOnTile; } set { isOnTile = value; } }

    //着陸の目的地
    private Vector3 destination = new Vector3();
    public Vector3 Destination { get { return destination; } set { destination = value; } }

    public GameObject searchRange;
    private GameObject search;
    // Start is called before the first frame update
    void Start()
    {
        //GameObjectを生成
        search = Instantiate(searchRange);
        //search.transform.parent =　transform;//enemyに追従させる
    }

    // Update is called once per frame
    void Update()
    {
        //Searchをenemyに追従させる
        search.transform.position = this.transform.position;
        //船の上から着陸まで
        Landing();
        Debug.Log("X:" + (int)this.transform.position.x + " Z:" + (int)this.transform.position.z);
    }

    //船の上から着陸まで
    private void Landing() 
    {
        if(isLanding == true) 
        {
            //船が着陸してから1秒後にenemyが動く
            Invoke("LandingMove", 1.0f);
        }
    }

    private void LandingMove()
    {
        float speed = 0.5f;
        //船の上から着陸まで
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //目的地についたら
        if(transform.position == destination) 
        {
            isLanding = false;
            isOnTile = true;
        }
    }

}
