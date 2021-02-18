using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class click_btn_staffroll : MonoBehaviour
{
    // Start is called before the first frame update
    public void BtnClicked()
    {
        Application.OpenURL("https://ja.wikipedia.org/wiki/%E3%82%B3%E3%83%8A%E3%83%9F%E3%82%B3%E3%83%9E%E3%83%B3%E3%83%89");
        return;
    }
}
