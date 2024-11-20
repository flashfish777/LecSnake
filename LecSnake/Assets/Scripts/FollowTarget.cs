using UnityEngine;
using UnityEngine.UI;
using SaveSystemTutorial;
using System.IO;

public class FollowTarget : MonoBehaviour
{
    public GameObject player;
    public Camera Mcamera;
    public Toggle ISOne; // 是否是第一人称
    public Toggle ISRotate; // 是否跟随旋转  

    Quaternion targetRotation;
    float smoothTime = 10;

    [System.Serializable]
    class OneAndRotate
    {
        public bool IsOne;
        public bool IsRotate;
    }

    OneAndRotate oneAndRotate = new();

    void Start()
    {
        var OneARotPath = Application.persistentDataPath + "\\OneAndRotate.mly";
        if (File.Exists(OneARotPath))
        {
            ISOne.isOn = SaveSystem.LoadFromJson<OneAndRotate>("OneAndRotate.mly").IsOne;
            ISRotate.isOn = SaveSystem.LoadFromJson<OneAndRotate>("OneAndRotate.mly").IsRotate;
        }
        else ISOne.isOn = ISRotate.isOn = true;
    }

    void Update()
    {
        // 使距离保持不变达到跟随效果
        transform.position = player.transform.position;
        if (ISOne.isOn)
        {
            Mcamera.transform.localPosition = new(0, 8, -25);
            Mcamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Mcamera.transform.localPosition = new(0, 35, -80);
            Mcamera.transform.localRotation = Quaternion.Euler(20, 0, 0);
        }

        if (ISRotate.isOn && transform.rotation.eulerAngles.y != player.transform.rotation.eulerAngles.y)
        {
            Quaternion currentRotation = transform.rotation;
            targetRotation = player.transform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, smoothTime * Time.deltaTime);
        }
    }

    // 人称Toggle
    public void IsOneSwitch()
    {
        if (ISOne.isOn == false)
        {
            oneAndRotate.IsOne = false;
            SaveSystem.SaveByJason("OneAndRotate.mly", oneAndRotate);
        }
        else
        {
            oneAndRotate.IsOne = true;
            SaveSystem.SaveByJason("OneAndRotate.mly", oneAndRotate);
        }
    }

    // 旋转Toggle
    public void IsRotateSwitch()
    {
        if (ISRotate.isOn == false)
        {
            oneAndRotate.IsRotate = false;
            SaveSystem.SaveByJason("OneAndRotate.mly", oneAndRotate);
        }
        else
        {
            oneAndRotate.IsRotate = true;
            SaveSystem.SaveByJason("OneAndRotate.mly", oneAndRotate);
        }
    }
}
