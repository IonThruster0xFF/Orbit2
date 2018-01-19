/// <summary>
/// Game manager.
/// 每个游戏有不同的GameManager
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager> 
    {   
        //public List<Record> records;

        void Start()
        {
            //records = Record.GetRecords();

            GameObject example = new GameObject("Example");
            example.AddComponent<Example>();
        }
    }
}
