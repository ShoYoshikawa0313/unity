using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_Color_Scene1: MonoBehaviour
{
    [SerializeField] Plane_Colors plane_colors;

    Vector3[] rubic_pos = new Vector3[54]
    {
        //UP
        new Vector3(-1,-1,1.483f),new Vector3(0,-1,1.483f),new Vector3(1,-1,1.483f),
        new Vector3(-1,0,1.483f),new Vector3(0,0,1.483f),new Vector3(1,0,1.483f),
        new Vector3(-1,1,1.483f),new Vector3(0,1,1.483f),new Vector3(1,1,1.483f),
        //RIGHT
        new Vector3(1.483f,1,1),new Vector3(1.483f,0,1),new Vector3(1.483f,-1,1),
        new Vector3(1.483f,1,0),new Vector3(1.483f,0,0),new Vector3(1.483f,-1,0),
        new Vector3(1.483f,1,-1),new Vector3(1.483f,0,-1),new Vector3(1.483f,-1,-1),
        //FRONT
        new Vector3(-1,1.483f,1),new Vector3(0,1.483f,1),new Vector3(1,1.483f,1),
        new Vector3(-1,1.483f,0),new Vector3(0,1.483f,0),new Vector3(1,1.483f,0),
        new Vector3(-1,1.483f,-1),new Vector3(0,1.483f,-1),new Vector3(1,1.483f,-1),
        //DOWN
        new Vector3(-1,1,-1.483f),new Vector3(0,1,-1.483f),new Vector3(1,1,-1.483f),
        new Vector3(-1,0,-1.483f),new Vector3(0,0,-1.483f),new Vector3(1,0,-1.483f),
        new Vector3(-1,-1,-1.483f),new Vector3(0,-1,-1.483f),new Vector3(1,-1,-1.483f),
        //LEFT
        new Vector3(-1.483f,-1,1),new Vector3(-1.483f,0,1),new Vector3(-1.483f,1,1),
        new Vector3(-1.483f,-1,0),new Vector3(-1.483f,0,0),new Vector3(-1.483f,1,0),
        new Vector3(-1.483f,-1,-1),new Vector3(-1.483f,0,-1),new Vector3(-1.483f,1,-1),
        //BACK
        new Vector3(1,-1.483f,1),new Vector3(0,-1.483f,1),new Vector3(-1,-1.483f,1),
        new Vector3(1,-1.483f,0),new Vector3(0,-1.483f,0),new Vector3(-1,-1.483f,0),
        new Vector3(1,-1.483f,-1),new Vector3(0,-1.483f,-1),new Vector3(-1,-1.483f,-1),
    };

    string[] rubic_plane = new string[54];

    int[] color_code = new int[54];

    void str2num(string str, int[] num)
    {
        for (int i = 0; i < 54; i++)
        {
            switch (str[i])
            {
                case 'U':
                    num[i] = 0;
                    break;
                case 'R':
                    num[i] = 1;
                    break;
                case 'F':
                    num[i] = 2;
                    break;
                case 'D':
                    num[i] = 3;
                    break;
                case 'L':
                    num[i] = 4;
                    break;
                case 'B':
                    num[i] = 5;
                    break;
            }
        }
    }

    void change_color()
    {
        for (int i = 0; i < 54; i++)
        {
            Vector3 tmp_pos = rubic_pos[i];
            for (int j = 0; j < 54; j++)
            {
                GameObject tmp_object = GameObject.Find(rubic_plane[j]);
                if (tmp_pos == tmp_object.transform.position)
                {
                    switch(color_code[i])
                    {
                        case 0: tmp_object.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1); break;
                        case 1: tmp_object.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1); break;
                        case 2: tmp_object.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1); break;
                        case 3: tmp_object.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1); break;
                        case 4: tmp_object.GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0, 1); break;
                        case 5: tmp_object.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1); break;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(plane_colors.planes);
        for (int i = 0; i < 54; i++)
        {
            rubic_plane[i] = "Plane" + i;
        }

        if(plane_colors.planes.Length != 0)
        {
            str2num(plane_colors.planes, color_code);
            change_color();
        }
        plane_colors.planes = "";
    }
}
