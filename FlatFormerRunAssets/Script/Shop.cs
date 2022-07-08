using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public GameObject[] Item;
    public Text HasCoin;
    private void Awake() {
        HasCoin.text = "Coin : " + PlayerPrefs.GetInt("Coin").ToString();
        int ItemSold = PlayerPrefs.GetInt("Item");
        if(ItemSold%2 == 1) Item[0].gameObject.SetActive(false);
        if(ItemSold%4 >= 2) Item[1].gameObject.SetActive(false);
        if(ItemSold%8 >= 4) Item[2].gameObject.SetActive(false);
    }
    public void ItemSell(int num){
        int price = 0;
        switch (num)
        {
            case 0:
                price = 3;
                break;
            case 1:
                price = 6;
                break;
            case 2:
                price = 10;
                break;
            default:
                break;
        }
        if(PlayerPrefs.GetInt("Coin") >= price){
            PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")-price);
            int addItemNum = 1;
            for (int i = 0; i < num; i++) addItemNum*=2;
            PlayerPrefs.SetInt("Item",PlayerPrefs.GetInt("Item")+addItemNum);
            PlayerPrefs.Save();
            Item[num].gameObject.SetActive(false);
            HasCoin.text = "Coin : " + PlayerPrefs.GetInt("Coin").ToString();
        }
    }
}
