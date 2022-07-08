using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Rigidbody2D rigid;
    public string ability;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector3(-5,0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
            Destroy(gameObject);
    }
}

