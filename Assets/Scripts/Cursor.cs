using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 没用到的类，光标类
/// </summary>
public class Cursor : MonoBehaviour
{
    public Transform cursor;
    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out raycastHit, 3.0f, 0x11))
        {
            cursor.transform.position = raycastHit.point + Camera.main.transform.forward.normalized * -0.02f;
        }
        else
        {
            cursor.transform.position = ray.GetPoint(2.0f);
        }
    }
}
