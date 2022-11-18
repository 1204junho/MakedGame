using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject BlockPrefeb, SlimePrefeb, SuperSlimePrefeb, BreakBlockPrefeb, HeartPrefeb;
    public GameObject[] blocks,slimes,superSlimes,breakBlocks,hearts,enemys;
    public float downSpeed;
    public WaitForSeconds BlockSetTime;
    // Start is called before the first frame update
    void Start()
    {
        BlockSetTime = new WaitForSeconds(2.5f);
        for (int i = 0; i < 30; i++){
                blocks[i] = Instantiate(BlockPrefeb);
                blocks[i].SetActive(false);
            }
        for (int i = 0; i < 10; i++){
                slimes[i] = Instantiate(SlimePrefeb);
            }
        for (int i = 0; i < 10; i++){
                superSlimes[i] = Instantiate(SuperSlimePrefeb);
            }
        for (int i = 0; i < 6; i++){
                breakBlocks[i] = Instantiate(BreakBlockPrefeb);
            }
        for (int i = 0; i < 6; i++){
                hearts[i] = Instantiate(HeartPrefeb);
                hearts[i].SetActive(false);
            }
    }
    public void Init(){
        for (int i = 0; i < 10; i++){
                enemys[i] = slimes[i];
            }
        BlockSetTime = new WaitForSeconds(2.5f);
        downSpeed = 0.0625f;
        foreach (var item in blocks)
            item.SetActive(false);
        foreach (var item in breakBlocks)
            item.SetActive(false);
        foreach (var item in hearts)
            item.SetActive(false);
        foreach (var item in enemys)
            item.SetActive(false);
        StartCoroutine("MapFall");
    }
    public void Promote(int i){
        for (; i < 10; i+=2){
                enemys[i] = superSlimes[i];
            }
    }
    private void FixedUpdate() {
        foreach (var block in blocks)
                {
                    if(block.activeSelf){
                        block.transform.position += Vector3.down*downSpeed;
                    }
                }
        foreach (var block in breakBlocks)
                {
                    if(block.activeSelf){
                        block.transform.position += Vector3.down*downSpeed;
                    }
                }
        foreach (var heart in hearts)
                {
                    if(heart.activeSelf){
                        heart.transform.position += Vector3.down*downSpeed;
                    }
                }
    }
    IEnumerator MapFall(){
        for (int i = 0; i < 3; i++)
        {
            blocks[i].transform.position = Vector2.right* (i-1)*3.3f;
            blocks[i].SetActive(true);
        }
        int block_in_row, path;
        while (true)
        {
            block_in_row = 0;
            path = Random.Range(0,5);
            while (block_in_row < 5)
            {
                if (path != block_in_row){
                    foreach (var block in blocks)
                    {
                        if(!block.activeSelf){
                            block.transform.position = Vector2.right* (block_in_row-2)*3.3f + Vector2.up*13;
                            block.SetActive(true);
                            break;
                        }
                    }
                    if(Random.Range(0,3)==0){
                        foreach (var enemy in enemys)
                            {
                                if(!enemy.activeSelf){
                                    enemy.transform.position = Vector2.right* ((block_in_row-2)*3.3f+(Random.value-0.5f)) + Vector2.up*14;
                                    enemy.SetActive(true);
                                    break;
                                }
                            }
                    }
                }
                else 
                    foreach (var block in breakBlocks)
                    {
                        if(!block.activeSelf){
                            block.transform.position = Vector2.right* (block_in_row-2)*3.3f + Vector2.up*13;
                            block.SetActive(true);
                            break;
                        }
                    }
                block_in_row++;
            }
            yield return BlockSetTime;
        }
    }
    
    public void HeartDrop(Vector3 pos){
        foreach (var heart in hearts)
        {
            if(!heart.activeSelf){
                heart.SetActive(true);
                heart.transform.position = pos;
                break;
            }
        }
    }
}
