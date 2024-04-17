using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Check_color : MonoBehaviour
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

    string check_color()
    {
        string result = "";
        for (int i = 0; i < 54; i++)
        {
            Vector3 tmp_pos = rubic_pos[i];
            for (int j = 0; j < 54; j++)
            {
                GameObject tmp_object = GameObject.Find(rubic_plane[j]);
                if (tmp_pos == tmp_object.transform.position)
                {
                    if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 1, 1, 1)) { result += "U"; }
                    else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 0, 0, 1)) { result += "R"; }
                    else if (tmp_object.GetComponent<Renderer>().material.color == new Color(0, 1, 0, 1)) { result += "F"; }
                    else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 1, 0, 1)) { result += "D"; }
                    else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 0.5f, 0, 1)) { result += "L"; }
                    else if (tmp_object.GetComponent<Renderer>().material.color == new Color(0, 0, 1, 1)) { result += "B"; }
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 54; i++)
        {
            rubic_plane[i] = "Plane" + i;
        }
    }

    // Update is called once per frame
    public void Onclick()
    {
        plane_colors.planes = check_color();
        Debug.Log(plane_colors.planes);
        SceneManager.LoadScene("scene1");
    }
}
