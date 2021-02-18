using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class click_title_mute : MonoBehaviour
{
    public Button mutebtn;
    public AudioSource bgm;
    bool playing = true;

    public void mutebtn_click()
    {
        if (playing)
        {
            bgm.Pause();
            playing = !playing;
        }
        else
        {
            bgm.UnPause();
            playing = !playing;
        }
    }
}
