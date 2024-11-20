using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SaveSystemTutorial;

public class MakeSpawns : MonoBehaviour
{
    public GameObject[] Treelist;
    public GameObject[] Foodlist;
    public GameObject[] Rocklist;
    public GameObject Cat;
    public GameObject Spider;
    // bool[,] map = new bool[33, 33]; // 格随机

    public bool IsContinue = false;

    public List<GameObject> Trees = new();
    public List<GameObject> Foods = new();
    public List<GameObject> Rocks = new();
    public List<GameObject> Cats = new();

    [System.Serializable]
    class Game_mod
    {
        public bool IsContinue;
    }

    [System.Serializable]
    class TreeData
    {
        public List<Vector3> Tree_po = new();
        public List<Quaternion> Tree_ro = new();
    }

    [System.Serializable]
    class FoodData
    {
        public List<Vector3> Food_po = new();
    }

    [System.Serializable]
    class RockData
    {
        public List<Vector3> Rock_po = new();
    }
    [System.Serializable]
    class CatData
    {
        public List<Vector3> Cat_po = new();
        public List<Quaternion> Cat_ro = new();
    }

    TreeData treeData = new();
    FoodData foodData = new();
    RockData rockData = new();
    CatData catData = new();

    void Start()
    {
        var ModPath = Application.persistentDataPath + "\\GameMod.mly";
        var TreePath = Application.persistentDataPath + "\\TreeData.mly";
        if (File.Exists(ModPath) && File.Exists(TreePath))
        {
            Game_mod gameMod = SaveSystem.LoadFromJson<Game_mod>("GameMod.mly");
            if (gameMod.IsContinue)
            {
                IsContinue = true;

                treeData = SaveSystem.LoadFromJson<TreeData>("TreeData.mly");
                foodData = SaveSystem.LoadFromJson<FoodData>("FoodData.mly");
                rockData = SaveSystem.LoadFromJson<RockData>("RockData.mly");
                catData = SaveSystem.LoadFromJson<CatData>("CatData.mly");
            }
        }

        if (IsContinue)
        {
            // 树
            for (int i = 0; i < 500; ++i)
                CreateTree(treeData.Tree_po[i], treeData.Tree_ro[i], IsContinue);

            // 食物
            for (int i = 0; i < 300; ++i)
                CreateFood(foodData.Food_po[i], IsContinue);

            // 石头
            for (int i = 0; i < 500; ++i)
                CreateRock(rockData.Rock_po[i], IsContinue);

            // 猫
            for (int i = 0; i < 100; ++i)
                CreatCat(catData.Cat_po[i], catData.Cat_ro[i], IsContinue);

            // 蜘蛛
            for (int i = 0; i < 200; ++i)
                CreateSpider();
        }
        else
        {
            // 树
            for (int i = 0; i < 500; ++i)
                CreateTree(new(), new(), IsContinue);

            // 食物
            for (int i = 0; i < 300; ++i)
                CreateFood(new(), IsContinue);

            // 石头
            for (int i = 0; i < 500; ++i)
                CreateRock(new(), IsContinue);

            // 猫
            for (int i = 0; i < 100; ++i)
                CreatCat(new(), new(), IsContinue);

            // 蜘蛛
            for (int i = 0; i < 200; ++i)
                CreateSpider();
        }

    }

    public void CreateTree(Vector3 treePo, Quaternion treeRo, bool IsContinue)
    {
        if (IsContinue)
            Trees.Add(Instantiate(Treelist[0], treePo, treeRo));
        else
        {
            Vector3 spawnTreePosition;
            spawnTreePosition.x = Random.Range(-800, 800);
            spawnTreePosition.y = 0.5f;
            spawnTreePosition.z = Random.Range(-800, 800);

            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            Trees.Add(Instantiate(Treelist[0], spawnTreePosition, spawnRotation));
        }
    }

    public void CreateFood(Vector3 foodPo, bool IsContinue)
    {
        int x = Random.Range(0, 3);
        Quaternion spawnRotation = Quaternion.Euler(45, Random.Range(0, 360), 0);

        if (IsContinue)
            Foods.Add(Instantiate(Foodlist[x], foodPo, spawnRotation));
        else
        {
            Vector3 spawnFoodPosition;
            spawnFoodPosition.x = Random.Range(-790, 790);
            spawnFoodPosition.y = 1.5f;
            spawnFoodPosition.z = Random.Range(-790, 790);

            Foods.Add(Instantiate(Foodlist[x], spawnFoodPosition, spawnRotation));
        }
    }

    public void CreateRock(Vector3 rockPo, bool IsContinue)
    {
        int x = Random.Range(0, 2);
        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        if (IsContinue)
            Rocks.Add(Instantiate(Rocklist[x], rockPo, spawnRotation));
        else
        {
            Vector3 spawnRockPosition;
            spawnRockPosition.x = Random.Range(-800, 800);
            spawnRockPosition.y = -0.3f;
            spawnRockPosition.z = Random.Range(-800, 800);

            Rocks.Add(Instantiate(Rocklist[x], spawnRockPosition, spawnRotation));
        }
    }

    public void CreatCat(Vector3 catPo, Quaternion catRo, bool IsContinue)
    {
        if (IsContinue)
            Cats.Add(Instantiate(Cat, catPo, catRo));
        else
        {
            Vector3 spawnRockPosition;
            spawnRockPosition.x = Random.Range(-800, 800);
            spawnRockPosition.y = 0;
            spawnRockPosition.z = Random.Range(-800, 800);

            Quaternion spawnRotation = Quaternion.Euler(-90, 0, 0);

            Cats.Add(Instantiate(Cat, spawnRockPosition, spawnRotation));
        }
    }

    public void CreateSpider()
    {
        Vector3 spawnRockPosition;
        spawnRockPosition.x = Random.Range(-780, 780);
        spawnRockPosition.y = 0;
        spawnRockPosition.z = Random.Range(-780, 780);

        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Instantiate(Spider, spawnRockPosition, spawnRotation);
    }
}

#region 格随机
// int num = 0;
// while (num < 1000)
// {
//     int x = Random.Range(1, 32); //19 3
//     int z = Random.Range(1, 32); // 7 10
//     Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);


//     if (!map[z, x])
//     {
//         map[z, x] = true;

//         x = x <= 16 ? ((x - 17) * 2 + 1) : ((x - 16) * 2 - 1);
//         z = z <= 16 ? ((17 - z) * 2 - 1) : ((16 - z) * 2 + 1);

//         Vector3 spawnPosition = new Vector3(x * 25, 0.5f, z * 25);
//         Instantiate(Treelist[0], spawnPosition, spawnRotation);

//         num++;
//     }
// }
#endregion