/// <summary>
/// 处理游戏事件!!!(委托)
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// 在这里定义游戏中的各种事件名
    /// </summary>
    public enum GameEvent
    {
        GAME_INIT,
        GAME_END,
        GAME_PAUSE,
        GAME_UNPAUSE
        //some other events
    };

    /// <summary>
    /// 声明一个事件委托类型(类似函数指针),参数如下:
    /// EventType: 事件类型
    /// Component: 事件主体(发送者)
    /// param: 其他要传递的参数
    /// </summary>
    public delegate void OnEvent(GameEvent eventType, Component sender, object param = null);

    public class EventManager : MonoSingleton<EventManager>
    {
        private Dictionary<GameEvent,List<OnEvent>> listeners = new Dictionary<GameEvent,List<OnEvent>>();

        /// <summary>
        /// 添加监听者(要执行的函数)
        /// </summary>
        /// <param name="eventType">Event type.</param>
        /// <param name="listener">Listener.</param>
        public void AddListener(GameEvent eventType, OnEvent listener)
        {
            List<OnEvent> listenerList = null;
            if (listeners.TryGetValue(eventType, out listenerList))
            {
                //List exists
                listenerList.Add(listener);
                return;
            }
            listenerList = new List<OnEvent>();
            listenerList.Add(listener);
            listeners.Add(eventType,listenerList);  
        }
        /// <summary>
        /// 当事件发生, 奔走相告
        /// </summary>
        /// <param name="eventType">Event type.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="param">Parameter.</param>
        public void PostEvent(GameEvent eventType, Component sender, object param = null)
        {
            List<OnEvent> listenerList = null;
            if (!listeners.TryGetValue(eventType, out listenerList))
                return;
            foreach (OnEvent e in listenerList)
            {
                if(e != null)
                    e(eventType, sender, param);
            }
        }
        /// <summary>
        /// 移除一种事件
        /// </summary>
        /// <param name="eventType">Event type.</param>
        public void RemoveEvent(GameEvent eventType)
        {
            listeners.Remove(eventType);
        }
        /// <summary>
        /// 清理冗余的null事件
        /// </summary>
        public void RemoveRedundancies()
        {
            Dictionary<GameEvent, List<OnEvent>> tempListeners = new Dictionary<GameEvent, List<OnEvent>>();
            foreach (KeyValuePair<GameEvent,List<OnEvent>> item in listeners)
            {
                for(int i=0;i<item.Value.Count;i++)
                {
                    if (item.Value[i].Equals(null))
                        item.Value.RemoveAt(i);
                }
                if (item.Value.Count > 0)
                    tempListeners.Add(item.Key,item.Value);
            }
            listeners = tempListeners;
        }
    }
}
