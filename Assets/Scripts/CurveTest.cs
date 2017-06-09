using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 没有用到的类
/// </summary>
public class CurveTest : MonoBehaviour
{

    [SerializeField]
    public Vector3Curve curve;

    [SerializeField]
    private AnimationCurve curve1;

    private void Awake()
    {
        this.curve = new Vector3Curve(new Vector3KeyFrame(0, Vector3.zero), new Vector3KeyFrame(0.7f, new Vector3(0.2f, 0.9f, 0.5f)), new Vector3KeyFrame(1.0f, Vector3.one));
    }

    private void Start()
    {
        for (int i = 0; i < curve1.length; i++)
        {
            Debug.Log("tangentMode=" + curve1.keys[i].tangentMode);
        }
    }


}
