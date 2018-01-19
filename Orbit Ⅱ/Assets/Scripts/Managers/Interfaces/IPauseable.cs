/// <summary>
/// IPauseable
/// 游戏暂停时需要暂停的类, 需要继承这个接口, 实现暂停与继续方法.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public interface IPauseable
{
    void OnPauseGame(GameEvent type, Component comp, object pram = null);
    void OnUnPauseGame(GameEvent type, Component comp, object pram = null);
}
