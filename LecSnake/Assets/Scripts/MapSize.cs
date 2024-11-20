using UnityEngine;

public class MapSize : MonoBehaviour
{
    public GameObject player; // 蛇头

    void Start()
    {

    }

    void Update()
    {
        // 保持在蛇头上方
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }

    // 将相机下移实现放大
    public void Big()
    {
        if (transform.position.y > 20)
            transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
    }

    // 将相机上移实现缩小
    public void Small()
    {
        if (transform.position.y < 400)
            transform.position = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
    }
}
