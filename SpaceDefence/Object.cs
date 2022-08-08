using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    protected Animator ani;
    protected Rigidbody2D rigid;
    protected SpriteRenderer sprite,healthBar;
    [SerializeField]
    protected int health,bounty;
    public int cost;
    virtual public int Health { get { return health; } set { health = value; healthBar.size = (float)value / 3 * Vector2.right + Vector2.up*0.33f; } }
    public float moveSpeed;
    public Color objectColor;
    protected WaitForSeconds blinkTime;
    private void Awake()
    {
        healthBar = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Health = health;
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = objectColor;
        blinkTime = new(0.125f);
    }
    virtual public void Hit()
    {
        if (health <= 1) {
            if(gameObject.tag != "team1")
                GameManager.Money += bounty;
            Destroy(gameObject);
        }
        else --Health;

    }
}
