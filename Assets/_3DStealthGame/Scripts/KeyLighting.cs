using System.Drawing;
using UnityEngine;

public class KeyLighting : MonoBehaviour
{
    void Start()
    {
        
    }

    // Here we set the cycle of the light that rests above each individual key.
    void Update()
    {
        GetComponent<Light>().intensity = Mathf.Lerp(0.5f, 2, Mathf.Abs (Mathf.Sin(Time.time)));
    }
}
