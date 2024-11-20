using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using SaveSystemTutorial;

public class NormalPlayer : MonoBehaviour
{
    // 蛇头刚体组件
    public Rigidbody rb;
    // 分数
    public int score;
    public int MaxScore = 0; // 单局最高分
    public int bodyNum = 0; // 变化前分数
    float countTime; // 计时器
    float fastCount = 0; // 加速计时器
    float spawnTime = 3; // 每几秒减一分
    // 分数文本UI
    public TextMeshProUGUI textComponent;
    public GameObject End; // 结束界面
    public Text topText; // 本局最高分
    public Text historyText; // 历史最高分
    // 蛇身
    public GameObject body;
    public List<GameObject> bodyList = new List<GameObject>();
    public List<Vector3> positionHistory = new List<Vector3>();
    // 补充物体
    public MakeSpawns addy;
    // 音效
    public AudioSource CatAd;
    public AudioSource HitAd;
    public AudioSource EatAd;
    // 音效开关
    public Toggle SomeMusic;

    float moveSpeed = 20f; // 移动速度
    float steerSpeed = 90f; // 旋转速度
    float bodySpeed = 20f; // 身体速度
    int Gap = 30; // 身体距离

    [System.Serializable]
    class HistoryScoreData
    {
        public int score;
    }

    [System.Serializable]
    class OnTriggerMusic
    {
        public bool someMusicOn;
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

    OnTriggerMusic onTriggerMusic = new();

    void Start()
    {
        Time.timeScale = 1;
        // 将蛇头作为第一个身体
        bodyList.Add(gameObject);

        var SomeMuPath = Application.persistentDataPath + "\\SomeMusic.mly";
        if (File.Exists(SomeMuPath))
        {
            SomeMusic.isOn = SaveSystem.LoadFromJson<OnTriggerMusic>("SomeMusic.mly").someMusicOn;
        }
        else SomeMusic.isOn = true;

        var ModPath = Application.persistentDataPath + "\\GameMod.mly";
        var dataPath = Application.persistentDataPath + "\\PlayerData.mly";
        if (File.Exists(ModPath))
        {
            Game_mod gameMod = SaveSystem.LoadFromJson<Game_mod>("GameMod.mly");
            if (gameMod.IsContinue && File.Exists(dataPath))
            {
                PlayerData playerData = SaveSystem.LoadFromJson<PlayerData>("PlayerData.mly");

                score = playerData.Score;
                bodyNum = playerData.bodyNum_score;
                gameObject.transform.position = playerData.Body_po[0];
                gameObject.transform.rotation = playerData.Body_ro[0];
                for (int i = 1; i < playerData.Body_num; ++i)
                {
                    GameObject NewBody = Instantiate(body, playerData.Body_po[i], playerData.Body_ro[i]);
                    bodyList.Add(NewBody);
                }
                positionHistory = playerData.History_po;
            }
        }
    }

    void Update()
    {
        if (score < 0)
        {
            End.SetActive(true); // 0分结束

            var HistoryPath = Application.persistentDataPath + "\\HistoryScore.mly";
            int overscore;
            if (File.Exists(HistoryPath))
            {
                overscore = SaveSystem.LoadFromJson<HistoryScoreData>("HistoryScore.mly").score;
            }
            else overscore = 0;

            if (MaxScore > overscore)
            {
                HistoryScoreData max = new()
                {
                    score = MaxScore
                };
                SaveSystem.SaveByJason("HistoryScore.mly", max); // 存最高分
                historyText.text = "历史最高知识：" + MaxScore;
            }
            else
                historyText.text = "历史最高知识：" + overscore.ToString();

            topText.text = "本局最高知识：" + MaxScore;

            Time.timeScale = 0;
            return;
        }

        // 蛇头移动
        if (Time.timeScale == 1)
            Move();
        // 每n秒减1分
        countTime += Time.deltaTime;
        if (countTime >= spawnTime)
        {
            if (score > 0)
                score--;
            countTime = 0;
        }

        // 更新分数
        textComponent.text = "知识:" + score.ToString();
        MaxScore = score > MaxScore ? score : MaxScore;

        // 身体根据分数增减
        if (score / 10 > bodyNum)
        {
            for (int j = 0; j < (score / 10) - bodyNum; ++j)
                NormalCreateBody();
        }
        else if (score / 10 < bodyNum)
        {
            for (int j = 0; j < bodyNum - (score / 10); ++j)
            {
                Destroy(bodyList[bodyList.Count - 1]);
                bodyList.RemoveAt(bodyList.Count - 1);
            }
        }
        bodyNum = score / 10;
    }

    // 检测碰撞
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                score = -1; // 碰到边界，游戏结束
                break;
            case "Tree":
                score -= 50; // 撞树-50
                addy.CreateTree(new(), new(), false); // 随机生成新的树
                Destroy(other.gameObject); // 删除撞到的树
                addy.Trees.Remove(other.gameObject);
                if (SomeMusic.isOn) // 判断音效是否开启
                    HitAd.Play(); // 播放音效
                break;
            case "Rock":
                score -= 20;
                addy.CreateRock(new(), false);
                Destroy(other.gameObject);
                addy.Rocks.Remove(other.gameObject);
                if (SomeMusic.isOn)
                    HitAd.Play();
                break;
            case "Cat":
                score -= 10;
                addy.CreatCat(new(), new(), false);
                Destroy(other.gameObject);
                addy.Cats.Remove(other.gameObject);
                if (SomeMusic.isOn)
                    CatAd.Play();
                break;
            case "Food":
                score += 15;
                addy.CreateFood(new(), false);
                Destroy(other.gameObject);
                addy.Foods.Remove(other.gameObject);
                if (SomeMusic.isOn)
                    EatAd.Play();
                break;
            case "Body":
                if (score > 100) score -= 10 * (bodyList.Count - bodyList.IndexOf(other.gameObject));
                break;
        }
    }

    // 生成身体并连接到前一个身体
    void NormalCreateBody()
    {
        Vector3 po = bodyList[bodyList.Count - 1].transform.position;
        GameObject NewBody = Instantiate(body, new(po.x, po.y, po.z), Quaternion.identity);
        bodyList.Add(NewBody);
    }

    void Move()
    {
        // 向前移动
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        // 加速
        if (Input.GetKey(KeyCode.Space))
        {
            moveSpeed = 40;
            bodySpeed = 40;
            fastCount += Time.deltaTime;
            if (fastCount > 2)
            {
                if (score > 0) score--;
                fastCount = 0;
            }
        }
        else
        {
            moveSpeed = 20;
            bodySpeed = 20;
        }

        // 方向操控
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        // 保存位置移动史
        positionHistory.Insert(0, transform.position);
        while (positionHistory.Count > bodyList.Count * Gap)
            positionHistory.RemoveAt(positionHistory.Count - 1);

        // 移动身体
        int index = 0;
        foreach (var body in bodyList)
        {
            Vector3 point = positionHistory[Mathf.Clamp(index * Gap, 0, positionHistory.Count - 1)];

            // 让贪吃蛇的身体沿头部轨迹运动
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;

            // 让身体朝向头部移动的方向
            body.transform.LookAt(point);

            index++;
        }
    }

    // 音效Toggle
    public void SomeMusicSwitch()
    {
        if (SomeMusic.isOn == false)
        {
            onTriggerMusic.someMusicOn = false;
            SaveSystem.SaveByJason("SomeMusic.mly", onTriggerMusic);
        }
        else
        {
            onTriggerMusic.someMusicOn = true;
            SaveSystem.SaveByJason("SomeMusic.mly", onTriggerMusic);
        }
    }
}
