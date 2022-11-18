using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public float moveDelay;
    public GameObject player;
    public WaitForSeconds half_sec;
    void Start()
    {
        half_sec = new WaitForSeconds(.5f);
        player = GameObject.Find("Player");
    }    
    IEnumerator Hit(){
        ui.ScoreChange(50);
        rigid.velocity = (transform.position - player.transform.position).normalized*25;
        yield return half_sec;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Flare")
            StartCoroutine("Hit");
    }    
}
