using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有需要由Chunk来管理的GameObject的类都需要实现这个接口
/// </summary>
public interface IChunkObject 
{
    Vector2 GetPosition();
    ID Save();

    ///  <summary>
    /// 当物体进入可以刷新的范围, 确保重复调用不会出错!
    /// </summary>
    void EnterUpdate();

    /// <summary>
    /// 当物体退出可以刷新的范围, 确保重复调用不会出错!
    /// </summary>
    void ExitUpdate();
}