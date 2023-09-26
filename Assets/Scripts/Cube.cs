using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public void GenerateColor()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.color = Random.ColorHSV();
    }

    public void ResetColor()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.color=Color.white;
    }
}
