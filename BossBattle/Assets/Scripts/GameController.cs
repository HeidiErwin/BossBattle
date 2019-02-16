using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ConfidenceBar confidenceBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // placeholder to test confidence bar

        if (Input.GetKey(KeyCode.Space)) {
            confidenceBar.SetConfidence(confidenceBar.GetConfidence() - .01f);
        }
    }
}
