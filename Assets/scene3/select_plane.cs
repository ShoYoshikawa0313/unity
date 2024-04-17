using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class select_plane : MonoBehaviour
{
    [SerializeField] Selected_Plane selected_Plane;
    GameObject target = null;

    // Start is called before the first frame update
    void Start()
    {
        selected_Plane.planes = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                target = hit.collider.gameObject;
                if(target.name.Length >= 5)
                {
                    if (target.name.Substring(0, 5) == "Plane")
                    {
                        if(target.name != "Plane4" && target.name != "Plane22" && target.name != "Plane13" && target.name != "Plane31" && target.name != "Plane40" && target.name != "Plane49")
                        {
                            target.GetComponent<Renderer>().material.color = Color.cyan;
                            selected_Plane.planes += (target.name + " ");
                        }
                    }
                }
            }
        }
    }
}
