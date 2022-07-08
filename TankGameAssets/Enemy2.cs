using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    int acc = 0;
    private void OnEnable() { acc = 0; }
    void FixedUpdate()
    {
        transform.position += Vector3.left * speed*0.0025f * ++acc;
        if(transform.position.x < -5){
            gameManager.PlayerHealthChange(-damage);
            gameManager.StartCoroutine("Blast",transform.position);
            gameObject.SetActive(false);
        }
    }
}
