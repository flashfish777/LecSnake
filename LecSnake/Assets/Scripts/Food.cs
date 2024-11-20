using UnityEngine;

public class Food : MonoBehaviour
{
    public bool WitchScene; // 判断在哪个场景，true为Main，flase为Menu
    void Start()
    {

    }

    void Update()
    {
        if (WitchScene)
        {
            if (Time.timeScale == 1) // 判断是否暂停
                transform.Rotate(Vector3.up * 0.2f, Space.World);
        }
        else 
        {
            // 在Menu场景中该脚本控制相机旋转
            transform.Rotate(Vector3.up * 0.01f, Space.World);
        }
    }
}
