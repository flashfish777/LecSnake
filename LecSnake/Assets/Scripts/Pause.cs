using UnityEngine;
using UnityEngine.UI;
using SaveSystemTutorial;
using System.IO;

public class Pause : MonoBehaviour
{
    public GameObject Settings; // 设置界面

    public Toggle MenuBackMusic;
    public Toggle MainBackMusic;
    public AudioSource music;

    [System.Serializable]
    class BackMusic
    {
        public bool BackMusicOn; // 存档记录变量
    }

    BackMusic backMusic = new();

    void Start()
    {
        var MenuMuPath = Application.persistentDataPath + "\\MenuBackMusic.mly";
        var MainMuPath = Application.persistentDataPath + "\\MainBackMusic.mly";

        if (MenuBackMusic)
        {
            if (File.Exists(MenuMuPath))
            {
                MenuBackMusic.isOn = SaveSystem.LoadFromJson<BackMusic>("MenuBackMusic.mly").BackMusicOn;
            }
            else MenuBackMusic.isOn = true;

            music.enabled = MenuBackMusic.isOn;
        }

        if (MainBackMusic)
        {
            if (File.Exists(MainMuPath))
            {
                MainBackMusic.isOn = SaveSystem.LoadFromJson<BackMusic>("MainBackMusic.mly").BackMusicOn;
            }
            else MainBackMusic.isOn = true;

            music.enabled = MainBackMusic.isOn;
        }

    }

    void Update()
    {

    }

    // 打开设置同时暂停游戏
    public void Open()
    {
        gameObject.SetActive(false);
        Settings.SetActive(true);
        Time.timeScale = 0;
    }

    // 继续
    public void Close()
    {
        gameObject.SetActive(true);
        Settings.SetActive(false);
        Time.timeScale = 1;
    }

    // 主菜单背景音乐Toggle
    public void MenuMusicSwitch()
    {
        if (MenuBackMusic.isOn == false)
        {
            backMusic.BackMusicOn = false;
            SaveSystem.SaveByJason("MenuBackMusic.mly", backMusic);

            music.enabled = false;
        }
        else
        {
            backMusic.BackMusicOn = true;
            SaveSystem.SaveByJason("MenuBackMusic.mly", backMusic);

            music.enabled = true;
        }
    }

    // 游戏内背景音乐Toggle
    public void MainMusicSwitch()
    {
        if (MainBackMusic.isOn == false)
        {
            backMusic.BackMusicOn = false;
            SaveSystem.SaveByJason("MainBackMusic.mly", backMusic);

            music.enabled = false;
        }
        else
        {
            backMusic.BackMusicOn = true;
            SaveSystem.SaveByJason("MainBackMusic.mly", backMusic);

            music.enabled = true;
        }
    }
}
