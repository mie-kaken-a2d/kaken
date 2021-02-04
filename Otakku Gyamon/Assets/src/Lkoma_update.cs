using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lkoma_update : MonoBehaviour
{
    float seconds;
    public GameObject lkoma;
    public GameObject rkoma;
    public Material lkoma1;
    public Material lkoma2;
    public Material lkoma3;
    public Material rkoma1;
    public Material rkoma2;
    public Material rkoma3;

    // Update is called once per frame
    void Update()
    {
        switch (seconds)
        {
            case 1:
                lkoma.GetComponent<Image>().material = lkoma1;
                break;
            case 2:
                lkoma.GetComponent<Image>().material = lkoma2;
                break;
            case 3:
                lkoma.GetComponent<Image>().material = lkoma3;
                seconds = 0;
                break;
        }
        seconds += Time.deltaTime;
    }
}
