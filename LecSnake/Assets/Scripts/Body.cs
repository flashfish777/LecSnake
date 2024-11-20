using UnityEngine;

public class Body : MonoBehaviour
{
    public Texture[] textures;

    void Start()
    {
        // 身体图案随机
        Material mats = GetComponent<MeshRenderer>().material;
        mats.mainTexture = textures[Random.Range(0, 12)];
    }

    void Update()
    {
        // transform.position = new(transform.position.x, 1.5f, transform.position.z);
    }
}
