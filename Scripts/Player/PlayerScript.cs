using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject searchRange;
    private GameObject search;
    // Start is called before the first frame update
    void Start()
    {
        //GameObjectを生成
        search = Instantiate(searchRange);
        search.transform.parent = transform;//playerに追従させる
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
