using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfiguration : MonoBehaviour {
    
    public MeshRenderer bodyRenderer;
    public MeshRenderer footRenderer;

    float lastColorChageTime = 0;
    Color color, targetColor;

    // Update is called once per frame
    void Update()
    {

        if (bodyRenderer)
        {
            color = Color.Lerp(bodyRenderer.material.color, targetColor, Time.deltaTime * 1);
            bodyRenderer.material.color = color;
            footRenderer.material.color = color;
            if (lastColorChageTime + 1 < Time.realtimeSinceStartup)
            {
                lastColorChageTime = Time.realtimeSinceStartup;
                targetColor = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}
