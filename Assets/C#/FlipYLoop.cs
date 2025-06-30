using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipYLoop : MonoBehaviour
{
    public float duration = 1f;       // 1‰ñ‚Ì”½“]‚É‚©‚©‚éŠÔ
    private RectTransform rectTransform;
    private float timer = 0f;
    private int direction = 1;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        timer += Time.deltaTime / duration;
        float t = Mathf.SmoothStep(1f, -1f, timer);  // ŠŠ‚ç‚©‚ÉƒXƒP[ƒ‹”½“]

        rectTransform.localScale = new Vector3(2f, t, 1f);

        if (timer >= 1f)
        {
            timer = 0f;
            // ”½“]•ûŒü‚ğ‹t‚É‚µ‚Ä‰•œƒ‹[ƒv
            float currentY = rectTransform.localScale.y;
            rectTransform.localScale = new Vector3(2f, Mathf.Sign(currentY), 1f);
        }
    }

}
