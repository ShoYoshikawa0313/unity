using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInput : MonoBehaviour
{
    GameObject target;
    GameObject camera;

    Vector2 startPos;

    string[] rubic = new string[27];

    GameObject[] childobjects = new GameObject[8];
    GameObject rubic_core;
    Vector3 direct;
    int positive;

    bool flag;

    float rotate_speed;

    void Start()
    {
        rubic_core = GameObject.Find("rubic1");
        direct = Vector3.up;
        positive = 0;
        flag = false;
        rotate_speed = 0;

        this.camera = GameObject.Find("Camera");
        for(int i=0;i<27;i++)
        {
            rubic[i] = "rubic" + i;
        }
    }

    bool find_core(int x,int y,int z)
    {
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.x == x && tmp.transform.position.y == y && tmp.transform.position.z == z)
            {
                rubic_core = tmp;
                return true;
            }
        }
        return false;
    }

    bool find_child_object_xy(int z)
    {
        if(rubic_core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.z == z && (tmp.transform.position != rubic_core.transform.position))
            {
                childobjects[num] = tmp;
                num++;
            }
        }
        if (num == 8) { return true; }
        else {return false; }
    }

    bool find_child_object_yz(int x)
    {
        if (rubic_core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.x == x && (tmp.transform.position != rubic_core.transform.position))
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
        if (rubic_core.name == "rubic13") { return false; }
        int num = 0;
        GameObject tmp;
        for (int i = 0; i < 27; i++)
        {
            tmp = GameObject.Find(rubic[i]);
            if (tmp.transform.position.y == y && (tmp.transform.position != rubic_core.transform.position))
            {
                childobjects[num] = tmp;
                num++;
            }
        }
        if (num == 8) { return true; }
        else { return false; }
    }

    void child_rotate(Vector3 center,Vector3 axis,float inv)
    {
        rubic_core.transform.RotateAround(center, axis, inv*(9));
        for(int i=0;i<8;i++) 
        {
            childobjects[i].transform.RotateAround(center, axis, inv*(9));    
        }
        if(rotate_speed >= 10)
        {
            rubic_core.transform.rotation = Quaternion.Euler(Mathf.Round(rubic_core.transform.eulerAngles.x), Mathf.Round(rubic_core.transform.eulerAngles.y), Mathf.Round(rubic_core.transform.eulerAngles.z));
            for (int i=0;i<8;i++)
            {
                childobjects[i].transform.rotation = Quaternion.Euler(Mathf.Round(childobjects[i].transform.eulerAngles.x), Mathf.Round(childobjects[i].transform.eulerAngles.y), Mathf.Round(childobjects[i].transform.eulerAngles.z));
                childobjects[i].transform.position = new Vector3(Mathf.Round(childobjects[i].transform.position.x), Mathf.Round(childobjects[i].transform.position.y), Mathf.Round(childobjects[i].transform.position.z));
            }
            flag = false;
            rotate_speed = 0;
        }
        else
        {
            rotate_speed ++;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = null;
            this.startPos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                target = hit.collider.gameObject;
                if(target.name.Substring(0,5) == "rubic")
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && flag == true)
        {
            Vector2 endPos = Input.mousePosition;
            float dx = (endPos.x-startPos.x);
            float dy = (endPos.y-startPos.y);

            Vector3 cam_pos = camera.transform.position;
            float camera_angle_zx = Mathf.Atan2((cam_pos.z),(cam_pos.x)) * Mathf.Rad2Deg;
            float camera_angle_y = Mathf.Atan2(cam_pos.y,Mathf.Sqrt(cam_pos.z*cam_pos.z+cam_pos.x*cam_pos.x)) * Mathf.Rad2Deg;

            // R
            if((camera_angle_zx < 45 && camera_angle_zx > -45) && (camera_angle_y < 45 && camera_angle_y > -45))
            {
                //����]
                if (Mathf.Abs(dx) > Mathf.Abs(dy))
                {
                    find_core(0, (int)target.transform.position.y, 0);
                    if (find_child_object_xz((int)target.transform.position.y))
                    {
                        if (dx > 0) {direct = Vector3.up; positive = -1; }
                        else {direct = Vector3.up; positive = 1; }
                    }
                }
                //�c��]
                else if (Mathf.Abs(dx) < Mathf.Abs(dy))
                {
                    find_core(0, 0, (int)target.transform.position.z);
                    if (find_child_object_xy((int)target.transform.position.z))
                    {
                        if (dy > 0) {direct = Vector3.forward; positive = 1; }
                        else { direct = Vector3.forward; positive = -1; }
                    }
                }
            }
            // U
            else if ((camera_angle_zx < 135 && camera_angle_zx > 45) && (camera_angle_y < 45 && camera_angle_y > -45))
            {
                //����]
                if (Mathf.Abs(dx) > Mathf.Abs(dy))
                {
                    find_core(0, (int)target.transform.position.y, 0);
                    if (find_child_object_xz((int)target.transform.position.y))
                    {
                        if (dx > 0) {direct = Vector3.up; positive = -1; }
                        else {direct = Vector3.up; positive = 1; }
                    }
                }
                //�c��]
                else if (Mathf.Abs(dx) < Mathf.Abs(dy))
                {
                    find_core((int)target.transform.position.x, 0, 0);
                    if (find_child_object_yz((int)target.transform.position.x))
                    {
                        if (dy > 0) {direct = Vector3.right; positive = -1; }
                        else {direct = Vector3.right; positive = 1; }
                    }
                }
            }
            // L
            else if ((camera_angle_zx < -135 || camera_angle_zx > 135) && (camera_angle_y < 45 && camera_angle_y > -45))
            {
                //����]
                if (Mathf.Abs(dx) > Mathf.Abs(dy))
                {
                    find_core(0, (int)target.transform.position.y, 0);
                    if (find_child_object_xz((int)target.transform.position.y))
                    {
                        if (dx > 0) {direct = Vector3.up; positive = -1; }
                        else {direct = Vector3.up; positive = 1; }
                    }
                }
                //�c��]
                else if (Mathf.Abs(dx) < Mathf.Abs(dy))
                {
                    find_core(0, 0, (int)target.transform.position.z);
                    if (find_child_object_xy((int)target.transform.position.z))
                    {
                        if (dy > 0) {direct = Vector3.forward; positive = -1; }
                        else {direct = Vector3.forward; positive = 1; }
                    }
                }
            }
            // D
            else if ((camera_angle_zx > -135 && camera_angle_zx < -45) && (camera_angle_y < 45 && camera_angle_y > -45))
            {
                //����]
                if (Mathf.Abs(dx) > Mathf.Abs(dy))
                {
                    find_core(0, (int)target.transform.position.y, 0);
                    if (find_child_object_xz((int)target.transform.position.y))
                    {
                        if (dx > 0) {direct = Vector3.up; positive = -1; }
                        else {direct = Vector3.up; positive = 1; }
                    }
                }
                //�c��]
                else if (Mathf.Abs(dx) < Mathf.Abs(dy))
                {
                    find_core((int)target.transform.position.x, 0, 0);
                    if (find_child_object_yz((int)target.transform.position.x))
                    {
                        if (dy > 0) {direct = Vector3.right; positive = 1; }
                        else {direct = Vector3.right; positive = -1; }
                    }
                }
            }

            rotate_speed++;
            
        }
        
        if(rotate_speed > 0) 
        {
            child_rotate(rubic_core.transform.position, direct, positive);
        }
    
    }
}