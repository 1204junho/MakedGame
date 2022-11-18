using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlime : Slime
{
    WaitForSeconds delay;
    Collider2D hitBox;
    SpriteRenderer body;
    void Start()
    {
        delay = new WaitForSeconds(moveDelay+Random.value*0.125f);
        half_sec = new WaitForSeconds(.5f);
        player = GameObject.Find("Player");
        hitBox = GetComponent<Collider2D>();
        body = GetComponent<SpriteRenderer>();
    }
    private void OnEnable() {
        StartCoroutine("Rush");
    }
    IEnumerator Rush(){
        if (body == null)
            yield return delay;
        while (true){
            hitBox.enabled = false;
            rigid.velocity = (player.transform.position - transform.position).normalized*speed;
            body.color = Color.cyan - Color.black*0.5f;
            yield return delay;
            body.color = Color.cyan;
            hitBox.enabled = true;
            yield return delay;
            yield return delay;
        }
    }
}
