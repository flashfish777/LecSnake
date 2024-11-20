using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.IO;

public class Seccess : MonoBehaviour
{
    // 不同模式最高分数
    public TextMeshProUGUI NormalScore;

    [System.Serializable]
    class HistoryScoreData
    {
        public int score;
    }

    void Start()
    {
        // 从存储的数据中调出
        var path = Application.persistentDataPath + "\\HistoryScore.mly";
        if (File.Exists(path))
        {
            var saveData = SaveSystemTutorial.SaveSystem.LoadFromJson<HistoryScoreData>("HistoryScore.mly");
            NormalScore.text = "经典模式：" + saveData.score.ToString();
        }
        else NormalScore.text = "经典模式：" + 0;
    }

    void Update()
    {

    }
    // 成就界面开启关闭
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
