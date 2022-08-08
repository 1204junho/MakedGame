using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Object
{
    public float attackDelayTime, range;
    LineRenderer line;
    WaitForSeconds quarterSec, attackDelay;
    AudioSource hitSound;
    Object attackTarget;
    bool isAttack { get { return ani.GetBool("isAttack"); } set {
            if (ani.GetBool("isAttack") == value) return;
            ani.SetBool("isAttack", value);
            if (value) StartCoroutine(Attack());
            else StopCoroutine(Attack());
        }
    }
    
    void Start()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        hitSound = GetComponent<AudioSource>();
        line.endColor = objectColor;
        sprite.flipX = moveSpeed < 0;
        sprite.color = objectColor;
        rigid.velocity = Vector2.right*moveSpeed;
        quarterSec = new(0.25f);
        attackDelay = new(attackDelayTime);
        StartCoroutine(FindTarget());
        StartCoroutine(CheckMove());
    }
    IEnumerator CheckMove()
    {
        WaitForSeconds oneSec = new(1f);
        while (true)
        {
            if (rigid.velocity.x * moveSpeed <= 0) rigid.velocity = Vector2.right * moveSpeed;
            yield return oneSec;
        }
    }
    IEnumerator FindTarget()
    {
        RaycastHit2D ray;
        int dir = moveSpeed > 0 ? 1 : -1;
        while (true)
        {
            ray = Physics2D.Raycast(transform.position + Vector3.right * dir * 0.375f, Vector2.right * dir, range);
            if (ray.collider != null && ray.collider.gameObject.tag != gameObject.tag)
            {
                rigid.velocity = Vector2.zero;
                line.SetPosition(1, Vector3.right * (ray.transform.position.x - transform.position.x));
                attackTarget = ray.collider.gameObject.GetComponent<Object>();
                isAttack = line.enabled = true;
            }
            else {
                rigid.velocity = Vector2.right * moveSpeed;
                isAttack = line.enabled = false;
            }
           yield return quarterSec;
        }
    }
    
    public IEnumerator Attack()
    {
        while (isAttack)
        {
            attackTarget.Hit();
            hitSound.Play();
            yield return attackDelay;
        }
    }
}
