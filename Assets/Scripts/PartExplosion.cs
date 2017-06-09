using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模型移动的实现部分
/// </summary>
public class PartExplosion : MonoBehaviour
{
    //移动的速度
    private float speed = 2.0f;
    //移动的位置
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = this.GetComponent<Renderer>().bounds.center - this.transform.position;
    }

    public void MoveOffCenter(int i)
    {
        //i越大，移动的越远，再项目中即，layer1中的模型飞的最远
        this.transform.localPosition = Vector3.Lerp(
            this.transform.localPosition,
            moveDirection * i,
            Time.deltaTime * speed);
    }
}
