using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    static readonly int WINDOW_SIZE = 10;
    float totalDelta = 0f;
    int i = 0;

    public TMP_Text fpsText;

    void Update() {
        if(i == WINDOW_SIZE) {
            int fps = (int)(1.0f / (totalDelta / WINDOW_SIZE));
            fpsText.text = "FPS: " + fps;
            totalDelta = i = 0;
        }
        totalDelta += Time.deltaTime;
        i++;
    }
}
