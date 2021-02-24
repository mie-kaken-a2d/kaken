using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title : MonoBehaviour
{
    public AudioSource bgmplayer;
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;
    public AudioClip bgm4;
    public AudioClip bgm5;
    public AudioClip bgm6;
    public AudioSource seplayer;
    public AudioClip se1;

    bool isplayermode = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isplayermode)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                seplayer.clip = se1;
                seplayer.Play();
                isplayermode = true;
            }
        }

        if (isplayermode)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                bgmplayer.clip = bgm1;
                bgmplayer.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bgmplayer.clip = bgm2;
                bgmplayer.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                bgmplayer.clip = bgm3;
                bgmplayer.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                bgmplayer.clip = bgm4;
                bgmplayer.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                bgmplayer.clip = bgm5;
                bgmplayer.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                bgmplayer.clip = bgm6;
                bgmplayer.Play();
            }
        }
    }
}
