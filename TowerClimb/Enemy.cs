using UnityEngine;
abstract public class Enemy : MonoBehaviour
{
    public int speed,score;
    public Rigidbody2D rigid;
    public GameManager ui;
    void Awake()
    {
        ui = GameObject.Find("Manager").GetComponent<GameManager>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnDisable() {
        ui.ScoreChange(score);
    }
}
