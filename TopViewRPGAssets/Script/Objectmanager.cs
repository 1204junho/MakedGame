using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectmanager : MonoBehaviour
{
    public GameObject SlimePrefab,FlyingCubePrefab,CoinPrefab,PotionPrefab,JewelRPrefab,JewelYPrefab;
    GameObject[] Slime,FlyingCube,Coin,Potion,JewelR,JewelY;
    Queue ItemQ = new Queue();
    void Start()
    {
        Slime = Gen(Slime,SlimePrefab,10);
        FlyingCube = Gen(FlyingCube,FlyingCubePrefab,10);
        Coin = Gen(Coin,CoinPrefab,50);
        Potion = Gen(Potion,PotionPrefab,10);
        JewelR = Gen(JewelR,JewelRPrefab,10);
        JewelY = Gen(JewelY,JewelYPrefab,5);
    }
    GameObject[] Gen(GameObject[] Object, GameObject Prefab, int count){
        Object = new GameObject[count];
        for (int i = 0; i < count; i++){
            Object[i] = Instantiate(Prefab);
            Object[i].SetActive(false);
            }
        return Object;
    }
    public GameObject Spawn(string type){
            foreach (var i in Order(type))
                if (!i.activeSelf)
                {
                    i.SetActive(true);
                    return i;
                }
        return null;
    }
    public void DeSpawn(string type){
        foreach (var i in Order(type))
            i.SetActive(false);
    }
    public GameObject Spawn(string type, Vector2 pos){
            foreach (var i in Order(type))
                if (!i.activeSelf)
                {
                    i.SetActive(true);
                    i.transform.position = pos;
                    return i;
                }
        return null;
    }
    public GameObject ItemDrop(string type, Vector2 pos){
            foreach (var i in Order(type))
                if (!i.activeSelf)
                {
                    ItemQ.Enqueue(i);
                    i.SetActive(true);
                    i.transform.position = pos;
                    return i;
                }
        GameObject item = (GameObject)ItemQ.Dequeue();
        item.transform.position = pos;
        ItemQ.Enqueue(item);
        return item;
    }
    GameObject[] Order(string type){
        switch (type)
        {
            case "slime":
                return Slime;
            case "flyingcube":
                return FlyingCube;
            case "coin":
                return Coin;
            case "potion":
                return Potion;
            case "jewelR":
                return JewelR;
            case "jewelY":
                return JewelY;
            default:
                return null;
        }
    }
}
