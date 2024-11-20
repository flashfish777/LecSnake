using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using SaveSystemTutorial;
using System.Collections.Generic;

public class ScenesChange : MonoBehaviour
{
    public NormalPlayer player_data;
    public MakeSpawns spawns_data;

    public GameObject New; // 新游戏Panel
    public GameObject Pattern; // 模式选择Panel

    public GameObject loadScreen; // 加载界面Panel
    public Slider slider; // 加载界面进度条
    public TextMeshProUGUI text; // 进度文本

    [System.Serializable]
    class HistoryScoreData
    {
        public int score;
    }

    [System.Serializable]
    class Game_mod
    {
        public bool IsContinue;
    }

    [System.Serializable]
    class PlayerData
    {
        public int Score;
        public int bodyNum_score;
        // public Vector3 Head_po;
        // public Quaternion Head_ro;
        public int Body_num;
        public List<Vector3> Body_po = new();
        public List<Quaternion> Body_ro = new();
        public List<Vector3> History_po = new();
    }

    [System.Serializable]
    class TreeData
    {
        public List<Vector3> Tree_po = new();
        public List<Quaternion> Tree_ro = new();
    }

    [System.Serializable]
    class FoodData
    {
        public List<Vector3> Food_po = new();
    }

    [System.Serializable]
    class RockData
    {
        public List<Vector3> Rock_po = new();
    }
    [System.Serializable]
    class CatData
    {
        public List<Vector3> Cat_po = new();
        public List<Quaternion> Cat_ro = new();
    }

    Game_mod game_Mod = new();

    // 切换至模式选择
    public void JumpPattern()
    {
        Pattern.SetActive(true);
        New.SetActive(false);
    }

    // 切换至主菜单
    public void JumpMain()
    {
        Pattern.SetActive(false);
        New.SetActive(true);
    }

    // 新游戏
    public void JumpNew()
    {
        game_Mod.IsContinue = false;

        SaveSystem.SaveByJason("GameMod.mly", game_Mod);
        SaveSystem.DeleteSaveFile("PlayerData.mly");
        SaveSystem.DeleteSaveFile("TreeData.mly");
        SaveSystem.DeleteSaveFile("FoodData.mly");
        SaveSystem.DeleteSaveFile("RockData.mly");
        SaveSystem.DeleteSaveFile("CatData.mly");

        StartCoroutine(Loadlevel());
    }
    // 继续游戏
    public void JumpContinue()
    {
        game_Mod.IsContinue = true;

        SaveSystem.SaveByJason("GameMod.mly", game_Mod);

        StartCoroutine(Loadlevel());
    }
    // 联网对战
    public void JumpMore()
    {

    }

    // 保存并返回主界面
    public void JumpMenu()
    {
        var HistoryPath = Application.persistentDataPath + "\\HistoryScore.mly";
        int overscore;
        if (File.Exists(HistoryPath))
        {
            overscore = SaveSystem.LoadFromJson<HistoryScoreData>("HistoryScore.mly").score;
        }
        else overscore = 0;

        HistoryScoreData NowMaxScore = new()
        {
            score = player_data.MaxScore
        };
        if (NowMaxScore.score > overscore) SaveSystem.SaveByJason("HistoryScore.mly", NowMaxScore); // 存最高分

        PlayerData playerData = new()
        {
            Score = player_data.score,
            bodyNum_score = player_data.bodyNum,
            Body_num = player_data.bodyList.Count,
            History_po = player_data.positionHistory
        };
        for (int i = 0; i < playerData.Body_num; ++i)
        {
            playerData.Body_po.Add(player_data.bodyList[i].transform.position);
            playerData.Body_ro.Add(player_data.bodyList[i].transform.rotation);
        }

        SaveSystem.SaveByJason("PlayerData.mly", playerData);

        TreeData treeData = new();
        FoodData foodData = new();
        RockData rockData = new();
        CatData catData = new();
        for (int i = 0; i < 500; ++i)
        {
            if (i < 100)
            {
                catData.Cat_po.Add(spawns_data.Cats[i].transform.position);
                catData.Cat_ro.Add(spawns_data.Cats[i].transform.rotation);
            }
            if (i < 300)
                foodData.Food_po.Add(spawns_data.Foods[i].transform.position);

            treeData.Tree_po.Add(spawns_data.Trees[i].transform.position);
            treeData.Tree_ro.Add(spawns_data.Trees[i].transform.rotation);
            rockData.Rock_po.Add(spawns_data.Rocks[i].transform.position);
        }

        SaveSystem.SaveByJason("TreeData.mly", treeData);
        SaveSystem.SaveByJason("FoodData.mly", foodData);
        SaveSystem.SaveByJason("RockData.mly", rockData);
        SaveSystem.SaveByJason("CatData.mly", catData);

        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    // 死亡返回主菜单
    public void JumpMenuDead()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    // 异步加载场景，控制进度条
    IEnumerator Loadlevel()
    {
        loadScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            slider.value = operation.progress;

            text.text = (int)(operation.progress * 100) + "%";

            if (operation.progress >= 0.9f)
            {
                slider.value = 1;
                text.text = "点击任意位置开始";
                if (Input.GetMouseButtonDown(0))
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
