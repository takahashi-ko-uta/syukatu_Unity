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
        //GameObject�𐶐�
        search = Instantiate(searchRange);
        search.transform.parent = transform;//player�ɒǏ]������
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
