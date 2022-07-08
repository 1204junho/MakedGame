using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    int angle = 0;
    public float up_down_speed;
   void FixedUpdate(){
       transform.position += Vector3.left * speed*0.01f + Vector3.down * up_down_speed*Mathf.Sin(up_down_speed * ++angle);
        if(transform.position.x < -5){
            gameManager.PlayerHealthChange(-damage);
            gameManager.StartCoroutine("Blast",transform.position + Vector3.up*colliderYPos);
            gameObject.SetActive(false);
        }
   }
}
