using UnityEngine;
using UnityEngine.UI; //MusicValue1
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class EqualizerController : MonoBehaviour
{
    const int sliderCount = 16;
    public float[] spectrum = new float[sliderCount];
    List<Slider> sliders = new List<Slider>();

    void Start()
    {
        GameObject[] slidersGO = GameObject.FindGameObjectsWithTag("slider");

        foreach (GameObject go in slidersGO)
        {
            sliders.Add(go.GetComponent<Slider>());
        }

    }


    void Update()
    {


        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 0; i < sliderCount; i++)
        {
            sliders[i].value = spectrum[i];

        }
    }
}