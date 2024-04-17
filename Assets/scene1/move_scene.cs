using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move_scene : MonoBehaviour
{
    public void Onclick()
    {
        SceneManager.LoadScene("scene3");
    }
}
