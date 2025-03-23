using UnityEngine;

public class ParallexScroll : MonoBehaviour
{
    public float backgroundHeight = 10f; // 你的背景图的实际高度
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        float cameraBottomY = mainCam.transform.position.y - mainCam.orthographicSize;

        // 如果背景已经完全落在摄像机下方
        if (transform.position.y + backgroundHeight < cameraBottomY)
        {
            // 找到所有背景图块
            ParallexScroll[] allBackgrounds = FindObjectsOfType<ParallexScroll>();

            float highestY = transform.position.y;

            foreach (var bg in allBackgrounds)
            {
                if (bg.transform.position.y > highestY)
                    highestY = bg.transform.position.y;
            }

            // 把当前背景移到最上面那个背景的正上方
            transform.position = new Vector3(transform.position.x, highestY + backgroundHeight, transform.position.z);
        }
    }
}
