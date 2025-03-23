using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float backgroundHeight = 10f;       // 背景图块的高度
    public float parallaxFactor = 0.8f;        // 背景移动速率，相对于摄像机。越小越慢（0.5 = 半速）
    public GameObject backgroundPrefab;        // 背景 prefab，用于生成新的图块

    private Camera mainCam;
    private float lastCamY;
    private Transform camTransform;

    void Start()
    {
        mainCam = Camera.main;
        camTransform = mainCam.transform;
        lastCamY = camTransform.position.y;
    }

    void Update()
    {
        float camY = camTransform.position.y;
        float deltaY = camY - lastCamY;

        // 视差移动：跟随摄像机但略慢
        transform.position += new Vector3(0, deltaY * parallaxFactor, 0);
        lastCamY = camY;

        // 如果摄像机已经上升到超过该背景顶端，就生成新的背景图块
        float backgroundTopY = transform.position.y + backgroundHeight / 2f;
        float cameraTopY = camY + mainCam.orthographicSize;

        if (cameraTopY > backgroundTopY)
        {
            SpawnNextBackground();
        }
    }

    void SpawnNextBackground()
    {
        // 防止重复生成
        if (GameObject.Find("Background_" + (transform.position.y + backgroundHeight)) != null)
            return;

        // 创建新的背景图块
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + backgroundHeight, transform.position.z);
        GameObject newBackground = Instantiate(backgroundPrefab, newPos, Quaternion.identity);
        newBackground.name = "Background_" + newPos.y;
    }
}
