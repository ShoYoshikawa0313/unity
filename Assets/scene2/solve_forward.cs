using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class solve_forward : MonoBehaviour
{

    string[] rubic = new string[27];

    [SerializeField] Move_data move_data;

    GameObject core;
    Vector3 axis;
    GameObject[] childobjects = new GameObject[8];
    float angle;

    float rotate_speed;

    public TextMeshProUGUI text_obj;

    string[] solved;

    Dictionary<string, int> move_names2int = new Dictionary<string, int>()
    {
        {"U",11},
        {"D",12},
        {"L",13},
        {"R",14},
        {"F",15},
        {"B",16},
        {"U2",21},
        {"D2",22},
        {"L2",23},
        {"R2",24},
        {"F2",25},
        {"B2",26},
        {"U`",31},
        {"D`",32},
        {"L`",33},
        {"R`",34},
        {"F`",35},
        {"B`",36}
    };

    bool find_core(int x, int y, int z)
    {
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.x == x && tmp.transform.position.y == y && tmp.transform.position.z == z)
            {
                core = tmp;
                return true;
            }
        }
        return false;
    }

    bool find_child_object_xy(int z)
    {
        if (core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.z == z && (tmp.transform.position != core.transform.position))
            {
                childobjects[num] = tmp;
                num++;
            }
        }
        if (num == 8) { return true; }
        else { return false; }
    }

    bool find_child_object_yz(int x)
    {
        if (core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.x == x && (tmp.transform.position != core.transform.position))
            {
                childobjects[num] = tmp;
                num++;
            }
        }
        if (num == 8) { return true; }
        else { return false; }
    }

    bool find_child_object_xz(int y)
    {
        if (core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.y == y && (tmp.transform.position != core.transform.position))
            {
                childobjects[num] = tmp;
                num++;
            }
        }
        if (num == 8) { return true; }
        else { return false; }
    }

    void child_rotate()
    {
        int inv = 0;
        if(angle >= 0) { inv = 1; }
        else {  inv = -1; }

        core.transform.RotateAround(core.transform.position, axis,inv * (9));
        for (int i = 0; i < 8; i++)
        {
            childobjects[i].transform.RotateAround(core.transform.position, axis, inv * (9));
        }
        if(rotate_speed >= Mathf.Abs(angle)/9)
        {
            core.transform.rotation = Quaternion.Euler(Mathf.Round(core.transform.eulerAngles.x), Mathf.Round(core.transform.eulerAngles.y), Mathf.Round(core.transform.eulerAngles.z));
            for (int i = 0; i < 8; i++)
            {
                childobjects[i].transform.rotation = Quaternion.Euler(Mathf.Round(childobjects[i].transform.eulerAngles.x), Mathf.Round(childobjects[i].transform.eulerAngles.y), Mathf.Round(childobjects[i].transform.eulerAngles.z));
                childobjects[i].transform.position = new Vector3(Mathf.Round(childobjects[i].transform.position.x), Mathf.Round(childobjects[i].transform.position.y), Mathf.Round(childobjects[i].transform.position.z));
            }
            rotate_speed = 0;
        }
        else
        {
            rotate_speed++;
        }
      
    }

    void rotate_cube(int move)
    {

        if(move%10 == 1) { find_core(0, 0, 1); axis = Vector3.forward; find_child_object_xy(1); }
        else if(move%10 == 2) { find_core(0, 0, -1); axis = Vector3.forward; find_child_object_xy(-1); }
        else if(move%10 == 3) { find_core(-1, 0, 0); axis = Vector3.right; find_child_object_yz(-1); }
        else if(move%10 == 4) { find_core(1, 0, 0); axis = Vector3.right; find_child_object_yz(1); }
        else if(move%10 == 5) { find_core(0, 1, 0); axis = Vector3.up; find_child_object_xz(1); }
        else if(move%10 == 6) { find_core(0, -1, 0); axis = Vector3.up; find_child_object_xz(-1); }

        if(move/10 == 1)
        {
            if (move % 10 == 1) { angle = 90; }
            else if (move % 10 == 2) { angle = -90; }
            else if (move % 10 == 3) { angle = -90; }
            else if (move % 10 == 4) { angle = 90; }
            else if (move % 10 == 5) { angle = 90; }
            else if (move % 10 == 6) { angle = -90; }
        }
        else if (move / 10 == 2)
        {
            if (move % 10 == 1) { angle = 180; }
            else if (move % 10 == 2) { angle = -180; }
            else if (move % 10 == 3) { angle = -180; }
            else if (move % 10 == 4) { angle = 180; }
            else if (move % 10 == 5) { angle = 180; }
            else if (move % 10 == 6) { angle = -180; }
        }
        else if (move / 10 == 3)
        {
            if (move % 10 == 1) { angle = -90; }
            else if (move % 10 == 2) { angle = 90; }
            else if (move % 10 == 3) { angle = 90; }
            else if (move % 10 == 4) { angle = -90; }
            else if (move % 10 == 5) { angle = -90; }
            else if (move % 10 == 6) { angle = 90; }
        }


    }

    void Start()
    {
        Debug.Log(move_data.moves);
        for (int i = 0; i < 27; i++)
        {
            rubic[i] = "rubic" + i;
        }

        core = GameObject.Find("rubic1");
        axis = Vector3.up;
        angle = 0;
        rotate_speed = 0;
        move_data.index = 0;

        solved = move_data.moves.Split(' ');
        text_obj.text = 0.ToString() + " / " + (solved.Length-1).ToString();
    }

    void Update()
    {
        if(rotate_speed > 0)
        {
            child_rotate();
        }
    }

    // Update is called once per frame
    public void Onclick()
    {
        Debug.Log(solved.Length);
        Debug.Log(move_data.index);
        if (solved.Length-1 > move_data.index && rotate_speed == 0)
        {
            rotate_cube(move_names2int[solved[move_data.index]]);
            rotate_speed++;
            move_data.index++;
            text_obj.text = move_data.index.ToString() + " / " + (solved.Length-1).ToString();
        }
    }
}
