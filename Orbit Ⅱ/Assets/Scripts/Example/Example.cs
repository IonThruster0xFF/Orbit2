using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Manager;

public class Example : MonoBehaviour 
{
    // 接下来是事件管理器示例

    void Start()
    {
        音频示例();
        OnEvent pause = OnPauseGame;
        EventManager.Instance.AddListener(GameEvent.GAME_PAUSE,pause);// 监听此事件!
        StartCoroutine(五秒后发出游戏暂停消息());
    }
    IEnumerator 五秒后发出游戏暂停消息()
    {
        yield return new WaitForSeconds(5);
        EventManager.Instance.PostEvent(GameEvent.GAME_PAUSE,this);
    }
    public void OnPauseGame(GameEvent type, Component comp, object p = null)
    {
        Debug.Log("游戏暂停了!");

    }

    // 管理存档示例
    public string 存档名 = "新的存档";
    public bool 新建存档 = false;
    public bool 进入存档 = false;
    public bool 退出并保存存档 = false;
    void Update()
    {
        if (新建存档)
        {
            新建存档 = false;
            管理存档示例();
        }
        if (退出并保存存档)
        {
            退出并保存存档 = false;
            退出存档示例();
        }
        if (进入存档)
        {
            进入存档 = false;
            进入存档示例();
        }
    }
    void 管理存档示例()
    {
        Record 这是一个存档 = new Record();
        if (这是一个存档.SetName(存档名) != null)
        {   
            Debug.Log("成功设置了存档:" + 这是一个存档.name);

            SaveManager.Instance.EnterRecord(这是一个存档);

            // 玩游戏

            SaveManager.Instance.SaveCurrentRecord();
        }
        else
            Debug.Log("已经有这个存档了!!!");
	}
    public 大嘴怪物 monster;
    void 进入存档示例()
    {
        SaveManager.Instance.EnterRecord(存档名);

        GameObject bigMouse = new GameObject("大嘴怪");
        monster = bigMouse.AddComponent<大嘴怪物>();
    }
    void 退出存档示例()
    {
        //先保存数据
        monster.SaveMonster();
        Destroy(monster.gameObject);
        //再退出存档
        SaveManager.Instance.ExitRecord(true);// true表示退出存档时要保存存档
    }
	
    // 音频示例
    public AudioClip music;
	void 音频示例 () 
    {
        music = Resources.Load("music") as AudioClip;
        //AudioManager.Instance.PlayEffectSound(XXX);

        //AudioManager.Instance.PlayBackgroundMusic(music); // 游戏暂停时这个不会暂停
        AudioManager.Instance.PlayAmbientSound(music); // 游戏暂停时这个会暂停
	}
       
}

