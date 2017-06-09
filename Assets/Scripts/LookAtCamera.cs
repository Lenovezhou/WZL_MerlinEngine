using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var target = Camera.main.transform.position;
        target.y = this.transform.position.y;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(target - this.transform.position), 10.0f * Time.deltaTime);
    }
}
