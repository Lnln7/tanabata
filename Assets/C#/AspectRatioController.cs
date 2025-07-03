using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    [SerializeField] private Vector2 targetAspect = new Vector2(9f, 16f); // 目標アスペクト比
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        AdjustAspect();
    }

    void AdjustAspect()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float targetRatio = targetAspect.x / targetAspect.y;
        float scale = screenAspect / targetRatio;

        Rect rect = cam.rect;

        if (scale < 1f)
        {
            rect.width = 1f;
            rect.height = scale;
            rect.x = 0f;
            rect.y = (1f - scale) / 2f;
        }
        else
        {
            float scaleWidth = 1f / scale;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
        }

        cam.rect = rect;
    }

}
