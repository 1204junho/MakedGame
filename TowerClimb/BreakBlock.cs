using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : Enemy
{
    ObjectManager obj;
    private void Start(){
        obj = GameObject.Find("Manager").GetComponent<ObjectManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Flare"){
            if(Random.Range(0,5)== 0)
                obj.HeartDrop(transform.position);
            this.gameObject.SetActive(false);
        }
    }
}
