using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : Object
{
    public float attackDelayTime, range;
    AudioSource hitSound;
    bool isAttack
    {
        get { return ani.GetBool("isAttack"); }
        set
        {
            if (ani.GetBool("isAttack") == value) return;
            ani.SetBool("isAttack", value);
            if (value) StartCoroutine(Attack());
            else StopCoroutine(Attack());
        }
    }
    WaitForSeconds attackDelay;
    Object attackTarget;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        hitSound = GetComponent<AudioSource>();
        sprite.flipX = moveSpeed < 0;
        sprite.color = objectColor;
        rigid.velocity = Vector2.right * moveSpeed;
        attackDelay = new(attackDelayTime);
        StartCoroutine(CheckMove());
    }
    IEnumerator CheckMove()
    {
        WaitForSeconds halfSec = new(.5f);
        while (true)
        {
            rigid.velocity = Vector2.right * moveSpeed;
            yield return halfSec;
        }
    }
    public IEnumerator Attack()
    {
        while (isAttack && attackTarget != null)
        {
            attackTarget.Hit();
            hitSound.Play();
            yield return attackDelay;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isAttack && collision.gameObject.tag != gameObject.tag)
        {
            attackTarget = collision.gameObject.GetComponent<Object>();
            isAttack = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isAttack = false;
    }
}
