using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delete_message : MonoBehaviour
{
    public void Onclick()
    {
        GameObject obj = GameObject.Find("error_message(Clone)");
        if(obj != null)
        {
            Destroy(obj);
        }

    }
}
