using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            // 让物体朝向相机
            transform.forward = cam.transform.forward;
        }
    }
}
