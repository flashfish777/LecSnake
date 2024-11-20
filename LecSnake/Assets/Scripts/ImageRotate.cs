using UnityEngine;

public class ImageRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 头像图片旋转
        transform.Rotate(Vector3.back * 0.1f);
    }
}
