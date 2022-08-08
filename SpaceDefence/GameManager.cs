using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform backGround;
    public GameObject unit;
    public TMP_Text MoneyText, TimerText, moneyGenText;
    static TMP_Text moneyText;
    static int money;
    int diff, nowTime = 0, genMoney = 200;
    public void IncreaseMoneyGen()
    {

        if (genMoney * (3+diff) > Money) return;
        Money -= genMoney * (3 + diff);
        genMoney += 200;
        moneyGenText.text = genMoney * (3 + diff) + "";
    }
    public static int Money {
        get { return money; }
        set { money = value; moneyText.text = value + ""; }
    }
    Object unitInfo;
    private void Start()
    {
        diff = PlayerPrefs.GetInt("Difficult");
        moneyText = MoneyText;
        Money = 500;
        moneyGenText.text = genMoney * (3 + diff) + "";
        StartCoroutine(MoneyGen());
        StartCoroutine(EnemySummon());
        StartCoroutine(Timer());
    }
    public void GameClear()
    {
        PlayerPrefs.SetInt("Score", (120-nowTime)*(diff+1)*100);
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    IEnumerator Timer()
    {
        WaitForSeconds oneSec = new(1f);
        while (true)
        {
            yield return oneSec;
            ++nowTime;
            if (nowTime % (30 / (diff + 1)) == 0 || nowTime > 120) { SummonRight("UltraSaber"); TimerText.text = "oh..."; }
            else TimerText.text = ((120 - nowTime) / 60 > 9 ? "" : "0") + (120 - nowTime) / 60 + ":" + ((120 - nowTime) % 60 > 9 ? "" : "0") + (120 - nowTime) % 60;
        }
    }
    private void Update()
    {
        backGround.Rotate(Vector3.back*0.125f);
    }
    IEnumerator MoneyGen()
    {
        WaitForSeconds fiveSec = new(3f+diff);
        while (true)
        {
            yield return fiveSec;
            Money += genMoney;
        }
    }
    IEnumerator EnemySummon()
    {
        WaitForSeconds spawnDelay = new(4f-diff);
        while (true)
        {
            yield return spawnDelay;
            if (Random.value > 0.3f) SummonRight("Ranger");
            else SummonRight("Saber");
        }
    }
    public void Buff()
    {
        if (Money < 500) return;
        Money -= 500;
        foreach (Object unit in FindObjectsOfType(typeof(Object)) as Object[])
        {
            if (unit.gameObject.tag == "team1") ++unit.Health;
        }
    }
    public void SummonLeft(string unitName)
    {
        unit = Resources.Load<GameObject>(unitName);
        int cost = unit.GetComponent<Object>().cost;
        if (Money < cost) return;
        Money -= cost;
        unit = Instantiate(unit, Vector3.left * 6 + Vector3.down, transform.rotation);
        unit.tag = "team1";
        unit.name = unit.tag + "_" + unitName;
        unitInfo = unit.GetComponent<Object>();
        unitInfo.objectColor = Color.blue * 0.5f + Color.white * 0.5f;
        if (unitInfo.moveSpeed < 0) unitInfo.moveSpeed *= -1;
        
    }
    public void SummonRight(string unitName)
    {
        unit = Resources.Load<GameObject>(unitName);
        unit = Instantiate(unit, Vector3.right * 6 + Vector3.down, transform.rotation);
        unit.tag = "team2";
        unit.name = unit.tag + "_" + unitName;
        unitInfo = unit.GetComponent<Object>();
        if(unitInfo.moveSpeed > 0) unitInfo.moveSpeed *= -1;
        unitInfo.objectColor = Color.red * 0.5f + Color.white * 0.5f;
    }
}
