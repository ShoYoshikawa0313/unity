using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_color_L : MonoBehaviour
{
    [SerializeField] Selected_Plane selected_Plane;
    string[] planes = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Onclick()
    {
        planes = selected_Plane.planes.Split(" ");
        for (int i=0;i < planes.Length-1;i++)
        {
            GameObject tmp = GameObject.Find(planes[i]);
            tmp.GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0, 1);
        }
        selected_Plane.planes = "";
    }
    
}
