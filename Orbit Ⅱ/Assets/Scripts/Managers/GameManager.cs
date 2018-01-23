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
        Transform myCamera;
        ChunkManager chunkManager;

        //public List<Record> records;
        void Start()
        {
            myCamera = GameObject.Find("Main Camera").transform;
            chunkManager = new ChunkManager();

            Record a = new Record();
            if (a.SetName("Mum") != null)
            {
                SaveManager.Instance.EnterRecord(a);
            }
            else
            {
                SaveManager.Instance.EnterRecord("Mum");
                chunkManager.Load();
            }
        }
        public bool showAll = false;
        public bool exit = false;
        void Update()
        {
            chunkManager.UpdateArea((Vector2)myCamera.position);


            if (exit)
            {
                exit = false;
                chunkManager.Save();
                SaveManager.Instance.ExitRecord(true);
                Application.Quit();
            }
            if (showAll)
            {
                showAll = false;
                chunkManager.TestShowAll();
            }
        }
    }
}
