using UnityEngine;

public class Cat : MonoBehaviour
{
    // 两种颜色
    public Texture[] textures;

    // 八向速度
    float[] Mx = { 0, 1, 1.44f, 1, 0, -1, -1.44f, -1 };
    float[] Mz = { 1.44f, 1, 0, -1, -1.44f, -1, 0, 1 };
    // 朝向
    int foward;

    float countTime; // 计时器
    float Turntime = 25; // 移动+静止
    float Movetime = 20; // 移动

    void Start()
    {
        // 随机颜色
        Material mats = GetComponent<MeshRenderer>().material;
        mats.mainTexture = textures[Random.Range(0, 2)];
        // 初始朝向随机
        foward = Random.Range(0, 8);
        transform.rotation = Quaternion.Euler(-90, foward * 45, 0);
    }

    void Update()
    {
        countTime += Time.deltaTime; // 按秒计时

        if (isStay() == 0)
        {
            //移动
            if (Time.timeScale == 1) // 判断是否暂停
                transform.position = new Vector3(transform.position.x + Mx[foward] * 0.03f, 0, transform.position.z + Mz[foward] * 0.03f);

        }
        else if (isStay() == 1)
        {
            //静止
        }
        else
        {
            //转向
            foward = turnfoward();
        }
    }

    // 检测与刚体的碰撞
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
            case "Tree":
            case "Rock":
                // 原地掉头
                foward = (foward + 4) % 8; // 180度旋转
                transform.rotation = Quaternion.Euler(-90, foward * 45, 0);
                break;
        }
    }

    // 随机转向
    int turnfoward()
    {
        int x = Random.Range(0, 8);
        transform.rotation = Quaternion.Euler(-90, x * 45, 0);
        return x;
    }

    // 返回当前状态
    int isStay()
    {
        if (countTime < Movetime)
        {
            // 移动
            return 0;
        }
        else if (countTime > Movetime && countTime < Turntime)
        {
            // 静止
            return 1;
        }
        else
        {
            // 转向
            countTime = 0;
            return 2;
        }
    }
}