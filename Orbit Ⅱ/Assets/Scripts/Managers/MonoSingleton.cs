/// <summary>
/// Mono singleton. 
/// 继承自MonoBehaviour的单例模板
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T instance = null;
    	
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.Log("More than 1 Manager of type " + typeof(T).Name);
                        return instance;
                    }

                    if (instance == null)
                    {
                        string instanceName = typeof(T).Name;
                        GameObject instanceGO = GameObject.Find(instanceName);// Empty GO
                        if (instanceGO == null)
                            instanceGO = new GameObject(instanceName);
                        instanceGO.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
     
    }
}