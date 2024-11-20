using UnityEngine;

public class ClickMap : MonoBehaviour
{
    public GameObject Mapp; // 地图
    bool isBig = false; // 大小状态

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Click()
    {
        if (isBig) smaller(); // 变小
        else Bigger(); // 变大
    }

    void Bigger()
    {
        isBig = true;
        Mapp.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);
        Mapp.GetComponent<RectTransform>().anchoredPosition = new Vector2(773.25f, -435);
        GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(773.25f, -435);
    }

    void smaller()
    {
        isBig = false;
        Mapp.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 280);
        Mapp.GetComponent<RectTransform>().anchoredPosition = new Vector2(140, -140);
        GetComponent<RectTransform>().sizeDelta = new Vector2(280, 280);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(140, -140);
    }
}
