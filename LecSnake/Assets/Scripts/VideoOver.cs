using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoOver : MonoBehaviour
{
    float countTime = 0; // 计时器

    void Update()
    {
        // 播放完毕自动跳转场景
        CountTime();
    }

    void CountTime()
    {
        countTime += Time.deltaTime;
        if (countTime > 63f) // 视频有多长就等多长时间
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    // 点击跳过
    public void Skip()
    {
        countTime = 100;
    }
}
