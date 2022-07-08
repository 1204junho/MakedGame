using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bar : MonoBehaviour
{
  public float moveVec = 0;
  public float addDir = 0;
  public float power;
  public void ChangeDir(int i = 0){
    addDir = i;
    Debug.Log(i);
  }
  private void FixedUpdate() {
    moveVec += power*addDir;
    transform.position += Vector3.right*moveVec;
  }
}
