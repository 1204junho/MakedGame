using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int damage;
    public int gold;
    public int colliderYPos;
    public GameManager gameManager;
    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void FixedUpdate()
    {
        transform.position += Vector3.left * speed*0.01f;
        if(transform.position.x < -5){
            gameManager.PlayerHealthChange(-damage);
            gameManager.StartCoroutine("Blast",transform.position + Vector3.up*colliderYPos);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
       other.gameObject.SetActive(false);
       other.gameObject.transform.position = Vector3.left*6 + Vector3.down*-2.675f;
       gameManager.CoinAmountChange(gold);
       gameManager.StartCoroutine("Blast",transform.position + Vector3.up*colliderYPos);
       gameObject.SetActive(false);
   }
}
