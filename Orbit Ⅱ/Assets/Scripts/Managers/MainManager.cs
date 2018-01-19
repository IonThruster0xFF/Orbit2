/// <summary>
/// Main manager.
/// 加载和卸载Manager
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class MainManager : MonoSingleton<MainManager>
    {
        List<GameObject> managerList = new List<GameObject>();//不包括自己

    	void Awake () 
        {
            DontDestroyOnLoad(gameObject);// Dont Destroy 只对根对象有效
            var mainManager = Instance;

            var eventManager = EventManager.Instance;//事件管理
            var audioManager = AudioManager.Instance;//音频管理
            var saveManager = SaveManager.Instance;//储存管理
            var gameManager = GameManager.Instance;
           
            managerList.Add(GameManager.Instance.gameObject);//游戏管理
            managerList.Add(EventManager.Instance.gameObject);//事件管理
            managerList.Add(SaveManager.Instance.gameObject);//储存管理
            managerList.Add(AudioManager.Instance.gameObject);//音频管理


            foreach (GameObject manager in managerList)
                manager.transform.SetParent(this.gameObject.transform);
       }

        void OnApplicationQuit()
        {
            foreach (GameObject manager in managerList)
                Destroy(manager);  
        }
    }
}