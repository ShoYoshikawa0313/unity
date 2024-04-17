using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The camera added this script will follow the specified object.
/// The camera can be moved by left mouse drag and mouse wheel.
/// </summary>
[ExecuteInEditMode, DisallowMultipleComponent]
public class CameraObject : MonoBehaviour
{

    public GameObject target; // an object to follow
    public GameObject clickedGameObject;

    public Vector3 offset; // offset form the target object

    [SerializeField] private float distance = 10.0f; // distance from following object
    [SerializeField] private float polarAngle = 45.0f; // angle with y-axis
    [SerializeField] private float azimuthalAngle = 45.0f; // angle with x-axis
    [SerializeField] private float mouseXSensitivity = 5.0f;
    [SerializeField] private float mouseYSensitivity = 5.0f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
                if(clickedGameObject.name == "Wall1" || clickedGameObject.name == "Wall2" || clickedGameObject.name == "Wall3" || clickedGameObject.name == "Wall4" || clickedGameObject.name == "Wall5" || clickedGameObject.name == "Wall6") 
                {
                    updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
            }
        }
        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
    }

    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * mouseXSensitivity;
        azimuthalAngle = Mathf.Repeat(x, 360);
        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, 5, 175);

    }
    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp) ,
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }

    public void Start()
    {
        this.target = GameObject.Find("rubic13");
        Application.targetFrameRate = 30;
    }

}