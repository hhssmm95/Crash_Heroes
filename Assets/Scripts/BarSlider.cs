using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    // Start is called before the first frame update
    Slider slBar;
    float fSliderBarTime;

    void Start()
    {
        slBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slBar.value <= 0)
            transform.Find("Fill Area").gameObject.SetActive(false);
        else
            transform.Find("Fill Area").gameObject.SetActive(true);
    }
}
