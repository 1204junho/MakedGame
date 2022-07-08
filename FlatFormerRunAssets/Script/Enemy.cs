using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    int speed = -1;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector3(0,-1);
        Falling();
    }
    public void Falling(){
        speed--;
        rigid.velocity = new Vector3(0,speed);
        Invoke("Falling",0.5f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
            Destroy(gameObject);
    }
}
