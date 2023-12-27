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
        //GameObjectÇê∂ê¨
        search = Instantiate(searchRange);
        search.transform.parent = transform;//playerÇ…í«è]Ç≥ÇπÇÈ
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
