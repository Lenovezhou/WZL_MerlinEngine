using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 模型的属性类
/// </summary>
public class PartLabel : MonoBehaviour
{
    //名称
    [SerializeField]
    public string Name;

    //ID
    [SerializeField]
    public string Id;

    //描述
    [SerializeField]
    public string Description;

    //层
    [SerializeField]
    private int layer;

    //属于发动机哪个系统
    [SerializeField]
    private Category category;

    public int GetLayer()
    {
        return this.layer;
    }

    public Category GetCategory()
    {
        return this.category;
    }
}
