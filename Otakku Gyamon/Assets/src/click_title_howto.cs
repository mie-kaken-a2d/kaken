using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class click_title_howto : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("howto");
    }
}
