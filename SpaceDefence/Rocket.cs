using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Object
{
    [SerializeField]
    new UnityEngine.UI.Image healthBar;
    [SerializeField]
    int maxHealth;
    [SerializeField]
    GameManager gm;
    override public int Health { get { return health; } set { if (value > maxHealth) return; health = value; healthBar.fillAmount = (float)value / maxHealth; } }
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        maxHealth = Health = health;
    }
    override public void Hit()
    {
        --Health;
        if (health <= 0)
        {
            if (gameObject.tag == "team1") UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            else gm.GameClear();
        }
    }
}
