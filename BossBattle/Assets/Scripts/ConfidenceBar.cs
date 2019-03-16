using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfidenceBar : MonoBehaviour
{
    Transform bar;
    private float confidence = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetConfidence(float conf) {
        confidence = conf;
        bar.localScale = new Vector3(confidence, 1f);
    }

    public float GetConfidence() {
        return confidence;
    }
}
