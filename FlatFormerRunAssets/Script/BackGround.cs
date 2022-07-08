using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "ReturnBackGround")
        {
            this.transform.position = new Vector3(20,0,0);
        }
    }
}
