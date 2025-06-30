using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSuisei : MonoBehaviour
{
    public float minScale = 1f;
    public float maxScale = 1.2f;
    public float duration = 1.5f;
    public AnimationCurve easingCurve;

    private float timer = 0f;
    private Vector3 initialScale;
    private bool scalingUp = true;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime / duration;
        float t = easingCurve.Evaluate(timer);

        float scale = Mathf.Lerp(minScale, maxScale, scalingUp ? t : 1f - t);
        transform.localScale = initialScale * scale;

        if (timer >= 1f)
        {
            timer = 0f;
            scalingUp = !scalingUp;
        }
    }

}
