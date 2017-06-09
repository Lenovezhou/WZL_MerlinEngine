using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 名称显示的UI逻辑
/// </summary>
public class AutoLabel : MonoBehaviour
{

    public float durationOut;                    //UI面板框退出时间
    public AnimationCurve speedOut;              //退出速度
    public float durationIn;                     //出现速度
    public AnimationCurve speedIn;               //出现速度
    public float holdTime = 5;                   //持续时间

    private float targetTime;                    //
    public Vector3[] positions;                  //位置

    /// <summary>
    /// 设置UI对话框的内容
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <param name="inventory"></param>
    public void SetTextInfo(string name, string id, string description, string inventory)
    {
        var textPanel = this.transform.FindChild("Canvas/TextPanel");
        textPanel.FindChild("Name").GetComponent<Text>().text = name;
        textPanel.FindChild("Description").GetComponent<Text>().text = description;
        textPanel.FindChild("ID").GetComponent<Text>().text = id;
        textPanel.FindChild("Inventory").GetComponent<Text>().text = inventory;
    }

    /// <summary>
    /// 释放UI对话框，没用到
    /// </summary>
    public void RefreshDialog()
    {
        this.targetTime = Time.time + this.holdTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="middlePoint"></param>
    /// <param name="end"></param>
    public void Label(Vector3 origin, Vector3 middlePoint, Vector3 end)
    {
        // Move the center to the target
        this.transform.position = origin;
        this.transform.LookAt(Camera.main.transform.position);

        origin = this.transform.InverseTransformPoint(origin);
        middlePoint = this.transform.InverseTransformPoint(middlePoint);
        end = this.transform.InverseTransformPoint(end);

        this.positions = new Vector3[] { origin, origin, origin };
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(positions);
        StartCoroutine(RenderingLine(lineRenderer, origin, middlePoint, end));
    }

    /// <summary>
    /// 整个的UI对话框的出现，显示，消失，包含画线，消除线
    /// </summary>
    /// <param name="lineRenderer"></param>
    /// <param name="origin"></param>
    /// <param name="middlePoint"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private IEnumerator RenderingLine(LineRenderer lineRenderer, Vector3 origin, Vector3 middlePoint, Vector3 end)
    {
        Vector3Curve v3Curve = new Vector3Curve(new Vector3KeyFrame(0.0f, origin), new Vector3KeyFrame(0.4f, middlePoint), new Vector3KeyFrame(1.0f, end));
        yield return StartCoroutine(RenderProgressLine(lineRenderer, v3Curve, 0.4f, this.speedOut, this.durationOut));

        // Display UI
        var animator = CreateCanvas();

        this.targetTime = Time.time + this.holdTime;
        while (Time.time < targetTime)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Close the menu and line
        animator.SetBool("Close", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Closed"));
        animator.gameObject.SetActive(false);

        // Draw back the line
        yield return StartCoroutine(RenderProgressLine(lineRenderer, v3Curve, 0.4f, this.speedIn, this.durationIn));

        Destroy(this.gameObject);
    }

    private Animator CreateCanvas()
    {
        var panel = this.transform.FindChild("Canvas");
        panel.transform.localPosition = positions[2];
        panel.transform.LookAt(2 * this.transform.position - Camera.main.transform.position);
        panel.gameObject.SetActive(true);
        return panel.GetComponent<Animator>();
    }

    private IEnumerator RenderProgressLine(LineRenderer lineRenderer, Vector3Curve v3Curve, float middleTime, AnimationCurve speed, float duration)
    {
        float interval = duration / 20.0f;
        for (float i = 0; i < 1.0f; i += 0.05f)
        {
            lineRenderer.SetPositions(positions);
            float mappedTime = speed.Evaluate(i);

            positions[2] = v3Curve.Evaluate(mappedTime);
            if (mappedTime < middleTime)
            {
                positions[1] = positions[2];
            }
            yield return new WaitForSeconds(duration / 20);
        }
        float mappedTime1 = speed.Evaluate(1.0f);
        positions[2] = v3Curve.Evaluate(mappedTime1);
        if (mappedTime1 < middleTime)
        {
            positions[1] = positions[2];
        }
        lineRenderer.SetPositions(positions);
    }
}
