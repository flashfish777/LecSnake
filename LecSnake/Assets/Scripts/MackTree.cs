using UnityEngine;

public class MackTree : MonoBehaviour
{
    public GameObject Tree; // 在Menu场景中的随机树木

    void Start()
    {
        for (int i = 0; i < 300; ++i)
            CreateTree();
    }

    void Update()
    {
        
    }

    public void CreateTree()
    {
        Vector3 spawnTreePosition;
        spawnTreePosition.x = Random.Range(-600, 600);
        spawnTreePosition.y = 0.5f;
        spawnTreePosition.z = Random.Range(-600, 600);

        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Instantiate(Tree, spawnTreePosition, spawnRotation);
    }
}