using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class howto_load : MonoBehaviour
{
    public AudioSource bgm;
    public Text hyoudai;
    public Text bio;
    public Button goyoutube;
    public Button gostaffroll;
    bool playing = true;
    int page = 1;
    int maxpage = 14;
    int cmdsuc = 0;

    void Start()
    {
        page = 1;
        updatetext();
        goyoutube.gameObject.SetActive(false);
        gostaffroll.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) /* 上キーが押された */
        {
            if (cmdsuc == 0 || cmdsuc == 1) /* 上上ならば加算 */
            {
                cmdsuc++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (cmdsuc == 2 || cmdsuc == 3)
            {
                cmdsuc++;
            }
        }else if (Input.GetKeyDown(KeyCode.B))
        {
            if(cmdsuc == 8)
            {
                cmdsuc++;
            }
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            if(cmdsuc == 9)
            {
                SceneManager.LoadScene("staffroll");
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) /* ミュート処理 */
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

        if (Input.GetKeyDown(KeyCode.Escape)) /* タイトルに戻る処理 */
        {
            SceneManager.LoadScene("title");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) /* 次のページへ */
        {
            if(cmdsuc == 4 || cmdsuc == 6)
            {
                cmdsuc++;
            }

            if (page > 1)
            {
                page--;
                updatetext();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) /* 前のページへ */
        {
            if (cmdsuc == 5 || cmdsuc == 7)
            {
                cmdsuc++;
            }
            if (page < maxpage)
            {
                page++;
                updatetext();
            }
        }
    }

    void updatetext()
    {
        switch (page)
        {
            case 1:
                hyoudai.text = "ゲームについて (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "このゲームは、バックギャモンの簡易版である\n「おさんぽギャモン」のデジタル版（非公式）です。\nこのゲームを通して、ギャモンの楽しさを\n知っていただけたら幸いです。";
                break;
            case 2:
                hyoudai.text = "操作方法 (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "キーボードのみの操作に対応しています。\n\n[D] ダイスを振る\n[1]～[4] ダイスを選択\n[←] [→] コマを選択\n[Space] 決定（移動）\n[Z] １手戻る\n[M] BGMミュート\n[Esc] タイトルに戻る（確認なし）";
                break;
            case 3:
                hyoudai.text = "プレイ方法 (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "[Esc] でタイトルに戻り、\n好きなプレイ方法を選択します。\n\n＊同一端末上でのマルチプレイ（PvP）\n＊敵がNPCのソロプレイ（PvE）\nに対応していますよ。";
                break;
            case 4:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "1.\n順番はダイスを振って決める。\n\nゲームを開始した直後にダイスを振ります。\nダイスの目が大きい方が先攻です。";
                break;
            case 5:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "2.\n相手のコマが２つ以上あるマスには移動できない。\n\n移動先に相手のペンギンが２体以上いる場合は、\nそこにコマを移動することができません。";
                break;
            case 6:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "3.\n相手に自分のコマを取られた場合、\nそのコマは漂流する。\n\n移動先に相手のペンギンが１体しかいない場合、\nそのペンギンを漂流させ、ペンギンがいた場所に\n自分のペンギンを進めることができます。";
                break;
            case 7:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "4.\n漂流しているコマがある場合は\nそれを最優先で動かす。\n\nペンギンたちは、仲間意識がとても高いです。\n漂流している仲間がいると、\nそのペンギンが氷に上がるまで待機します。\n漂流ペンギンは、最優先で\n動かさなければなりません。\n氷に上がれる場所がなければ漂流し、\n相手のターンになります。";
                break;
            case 8:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "5.\n自分のコマが全て同じ色の場所になければ\nゴールできない。\n\nペンギンはとても心が優しいです。\n遅れている仲間がいると、\nその仲間が追いつくまで家に帰りません。\n家に帰ることができる範囲は、\n氷の色で確認可能です。";
                break;
            case 9:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "6.\n移動不能な場合を除き、\n必ず移動しなければならない。\n\nペンギンは常に移動しなければなりません。\n……なんだかマグロみたいですね。";
                break;
            case 10:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "7.\nゴール時はダイスの値と一致するコマを\n優先で動かさなければならない。\n\n例えばダイスで「２」が出て、\n家に着くまで「あと２マス」のペンギンがいた場合、\nそのペンギンを先に帰宅させましょう。\nそういうルールです。";
                break;
            case 11:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "8.\n先に、フィールドにいる\n全ての同じ色のコマがゴールすれば勝利。\n\n青と桃の２色のペンギンがいます。\nプレイヤーは、どちらか好きな方を決めましょう。\n選んだ色と同じ色のペンギンを動かします。\n先に８体全てを帰宅させたプレイヤーの勝利です。";
                goyoutube.gameObject.SetActive(false);
                break;
            case 12:
                hyoudai.text = "ルール (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "動画を見ると分かりやすいかもしれません。";
                goyoutube.gameObject.SetActive(true);
                break;
            case 13:
                hyoudai.text = "免責 (" + page + "/" + (maxpage - 1) + ")";
                bio.text = "このゲームを遊んで発生した\n損害・損失及び友情の崩壊などについては、\n製作陣は一切の責任を負わないものとします。\n自己責任で、楽しくプレイしましょう。";
                goyoutube.gameObject.SetActive(false);
                gostaffroll.gameObject.SetActive(false);
                break;
            case 14:
                hyoudai.text = "？？？ (" + page + "/" + maxpage + ")";
                bio.text = "573";
                gostaffroll.gameObject.SetActive(true);
                break;
        }
        return;
    }
}
