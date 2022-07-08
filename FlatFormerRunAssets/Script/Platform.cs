using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Rigidbody2D rigid;
    public string color;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector3(-5,0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && color == "Purple")
            Invoke("DestroyPlatform",0.4f);
        if(collision.gameObject.tag == "Border")
            DestroyPlatform();
    }
    void DestroyPlatform(){
        Destroy(gameObject);
    }
}