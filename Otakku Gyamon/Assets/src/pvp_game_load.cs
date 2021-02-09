﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pvp_game_load : MonoBehaviour
{
    /*
     [改善点]
        * 先攻ターンがどっちか直感的にわからない
        * ゴール可能状態では、引いたコマの値と同値でゴールできるコマを優先する（）
        * （デバッグデータの取得待ち）監獄ロールバックのコマバグ
    */

    bool user = false;  //プレイヤー(左側がtrue)
    bool gameready = false; //ready to game
    bool ongame = false;    //playing game
    bool canroll = false;   //ダイスロールの可否
    bool selectkoma = false;    //コマ選択モードか
    bool playsound; //サウンドの再生が許可されているか
    bool visiblefieldid;    //フィールド番号の表示
    int turn = 0;   //経過ターン
    int remain = 0; //残りの移動回数
    int[] lkoma;    //左側コマの位置
    int[] rkoma;    //右側コマの位置
    int[] ground;   //フィールドのコマの数
    int roll1 = 0, roll2 = 0;   //ダイスの出た値
    int selected = 0;   //選択中のダイス
    int selected_koma = 0;  //選択中のコマ
    bool activedice1, activedice2, activedice3, activedice4;    //ダイス使用済みか
    bool rb_canback;
    int rb_lp, rb_rp;           //【Return Buffer】blueコマの前の位置、redコマの前の位置
    int rb_lid, rb_rid;         //【Return Buffer】blueコマのID、redコマのID
    int rb_bg, rb_mg, rb_ag;    //【Return Buffer】左右の移動時のground値
    int rb_diceid, rb_diceti;   //【Return Buffer】ダイスID
    bool rb_wasblue;            //【Return Buffer】青色のターンだったか
    bool rb_prison;             //【Return Buffer】監獄に連れて行かれたか
    int rb_prisonid;            //【Return Buffer】監獄に連れてかれたコマのID
    float timecounter;

    public GameObject lkoma0_obj;
    public GameObject lkoma1_obj;
    public GameObject lkoma2_obj;
    public GameObject lkoma3_obj;
    public GameObject lkoma4_obj;
    public GameObject lkoma5_obj;
    public GameObject lkoma6_obj;
    public GameObject lkoma7_obj;
    public GameObject rkoma0_obj;
    public GameObject rkoma1_obj;
    public GameObject rkoma2_obj;
    public GameObject rkoma3_obj;
    public GameObject rkoma4_obj;
    public GameObject rkoma5_obj;
    public GameObject rkoma6_obj;
    public GameObject rkoma7_obj;
    public GameObject diceview1_obj;
    public GameObject diceview2_obj;
    public GameObject diceview3_obj;
    public GameObject diceview4_obj;
    public Material di1;
    public Material di2;
    public Material di3;
    public Material lkoma_notselected;
    public Material rkoma_notselected;
    public Material lkoma_selected;
    public Material rkoma_selected;
    public Material lkoma_goal;
    public Material rkoma_goal;
    public Text turntext;
    public Text todotext;
    public Text turnuser;
    public Text l_goal_label;
    public Text r_goal_label;
    public Text l_kick_label;
    public Text r_kick_label;
    public Text selected_dice_1;
    public Text selected_dice_2;
    public Text selected_dice_3;
    public Text selected_dice_4;
    public Text remaintext;
    public Text debugtext;
    public Text debugtext1;
    public Text groundidtext;
    public AudioSource bgm;
    public AudioSource se;
    public AudioClip clearbgm;
    public AudioClip dicese;
    public AudioClip movekomase;
    public AudioClip goalse;
    public AudioClip prisonse;
    public AudioClip cantse;
    public AudioClip gyamonwin;

    // Start is called before the first frame update
    void Start()
    {
        /* フィールド初期化 */
        ground = new int[16];
        for (int i = 0; i < 16; i++)
        {
            ground[i] = new int();
            switch (i)
            {
                case 2:
                    ground[i] = 2;
                    break;
                case 4:
                    ground[i] = -3;
                    break;
                case 7:
                    ground[i] = 3;
                    break;
                case 8:
                    ground[i] = -3;
                    break;
                case 11:
                    ground[i] = 3;
                    break;
                case 13:
                    ground[i] = -2;
                    break;
                default:
                    ground[i] = 0;
                    break;
            }
        }

        /*
        【補足】
        たとえば、Left Playerがground[2]に3体いるならば、"ground[2] = -3" になり、
        RightPlayerがground[6]に1体ならば、"ground[6] = 1" となる。

        【値早見表】
        ground[0] = 右側ゴール (Left Cassle)
              [1] = 右側島流し (Right側のコマが監禁)
              [2] = フィールド1 (Left側陣地@1)
              [3] = フィールド2 (Left側陣地@2)
              [4] = フィールド3 (Left側陣地@3)
              [5] = フィールド4 (Mid陣地Left側@1)
              [6] = フィールド5 (Mid陣地Left側@2)
              [7] = フィールド6 (Mid陣地Left側@3)
              [8] = フィールド7 (Mid陣地Right側@3)
              [9] = フィールド8 (Mid陣地Right側@2)
              [10] = フィールド9 (Mid陣地Right側@1)
              [11] = フィールド10 (Right側陣地@3)
              [12] = フィールド11 (Right側陣地@2)
              [13] = フィールド12 (Right側陣地@1)
              [14] = 左側島流し (Left側のコマが監禁)
              [15] = 左側ゴール (Right Cassle)
    */

        /* コマ初期化 */
        lkoma = new int[8];
        rkoma = new int[8];

        rkoma[0] = new int();
        rkoma[0] = 2;
        rkoma0_obj.transform.localPosition = new Vector3(-215, -60, 0);
        rkoma[1] = new int();
        rkoma[1] = 2;
        rkoma1_obj.transform.localPosition = new Vector3(-215, -10, 0);
        lkoma[0] = new int();
        lkoma[0] = 13;
        lkoma0_obj.transform.localPosition = new Vector3(220, -60, 0);
        lkoma[1] = new int();
        lkoma[1] = 13;
        lkoma1_obj.transform.localPosition = new Vector3(220, -10, 0);
        lkoma[2] = new int();
        lkoma[2] = 8;
        lkoma2_obj.transform.localPosition = new Vector3(20, -35, 0);
        rkoma[2] = new int();
        rkoma[2] = 7;
        rkoma2_obj.transform.localPosition = new Vector3(-20, -35, 0);
        rkoma[3] = new int();
        rkoma[3] = 7;
        rkoma3_obj.transform.localPosition = new Vector3(-20, 15, 0);
        rkoma[4] = new int();
        rkoma[4] = 7;
        rkoma4_obj.transform.localPosition = new Vector3(-20, 65, 0);
        lkoma[3] = new int();
        lkoma[3] = 8;
        lkoma3_obj.transform.localPosition = new Vector3(20, 15, 0);
        lkoma[4] = new int();
        lkoma[4] = 8;
        lkoma4_obj.transform.localPosition = new Vector3(20, 65, 0);
        lkoma[5] = new int();
        lkoma[5] = 4;
        lkoma5_obj.transform.localPosition = new Vector3(-140, 40, 0);
        rkoma[5] = new int();
        rkoma[5] = 11;
        rkoma5_obj.transform.localPosition = new Vector3(140, 40, 0);
        rkoma[6] = new int();
        rkoma[6] = 11;
        rkoma6_obj.transform.localPosition = new Vector3(140, 90, 0);
        rkoma[7] = new int();
        rkoma[7] = 11;
        rkoma7_obj.transform.localPosition = new Vector3(140, 135, 0);
        lkoma[6] = new int();
        lkoma[6] = 4;
        lkoma6_obj.transform.localPosition = new Vector3(-140, 90, 0);
        lkoma[7] = new int();
        lkoma[7] = 4;
        lkoma7_obj.transform.localPosition = new Vector3(-140, 135, 0);

        /* Ready! */
        diceview1_obj.GetComponent<Image>().material = null;
        diceview2_obj.GetComponent<Image>().material = null;
        diceview3_obj.GetComponent<Image>().material = null;
        diceview4_obj.GetComponent<Image>().material = null;
        turn = 0;   //ターン数初期化
        remain = 0; //残りダイス数初期化
        roll1 = 0;  //ダイス初期化
        roll2 = 0;  //ダイス初期化
        selected_koma = 0;
        selected = 1;
        activedice1 = false;
        activedice2 = false;
        activedice3 = false;
        activedice4 = false;
        user = false;
        ongame = false;
        canroll = false;
        gameready = true;
        selectkoma = false;
        rb_canback = false;
        rb_lp = -1;
        rb_rp = -1;
        rb_lid = -1;
        rb_rid = -1;
        rb_bg = -1;
        rb_mg = -1;
        rb_ag = -1;
        rb_diceid = -1;
        rb_diceti = -1;
        rb_prison = false;
        rb_prisonid = -1;
        todotext.text = "先攻を決めます。\n[D]キーを押してね。";
        turntext.text = "Turn: " + turn.ToString();
        l_goal_label.text = ground[0].ToString() + "/8";
        r_goal_label.text = ground[15].ToString() + "/8";
        l_kick_label.text = "漂流：" + ground[1].ToString();
        r_kick_label.text = "漂流：" + ground[14].ToString();
        playsound = true;
        visiblefieldid = true;
        timecounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /* turntext.text = "groundデバッグ\n";
        for (int i = 0; i < 16; i++)
        {
            //デバッグ用Ground出力
            turntext.text += "ground[" + i + "]@" + ground[i] + "\n";
        } */

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (playsound)
            {
                bgm.Pause();
                playsound = !playsound;
            }
            else
            {
                bgm.UnPause();
                playsound = !playsound;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("title");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (visiblefieldid)
            {
                //visible
                groundidtext.gameObject.SetActive(false);
                visiblefieldid = !visiblefieldid;
            }
            else
            {
                //hide
                groundidtext.gameObject.SetActive(true);
                visiblefieldid = !visiblefieldid;
            }
        }

        if (ongame)
        {
            //勝利しましたか？
            if (ground[0] <= -8)
            {
                //Left側の勝利
                /* 勝利イベントここ */
                gamewin(true);
                return;
            }
            else if (ground[15] >= 8)
            {
                //Right側の勝利
                /* 勝利イベントここ */
                gamewin(false);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                returnmove();
            }

            if (canroll)
            {
                if (Input.GetKeyDown(KeyCode.D))    /* ダイスロール */
                {
                    canroll = false;
                    rb_canback = false;

                    turn++;
                    turntext.text = "Turn: " + turn.ToString();

                    todotext.text = "ダイスを振っています…";
                    roll1 = diceroll();
                    roll2 = diceroll();
                    todotext.text = "ダイスを振りました。\n数字／スペースキーでコマを移動しましょう。";


                    if (roll1 == roll2)
                    {
                        remain = 4;
                        diceapply(roll1, roll2, true);
                    }
                    else
                    {
                        remain = 2;
                        diceapply(roll1, roll2);
                    }
                }
            }
            else
            {
                //コマ移動フェーズ
                if (remain > 0)
                {
                    if ((activedice1 && (isstuck(user, roll1) == false)) || (activedice4 && (isstuck(user, roll2) == false)) || (activedice2 && (isstuck(user, roll1)) == false) || (activedice3 && (isstuck(user, roll1) == false)))
                    {
                        if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            //←キー
                            if (selected_koma <= 0)
                            {
                                selected_koma = 7;
                            }
                            else
                            {
                                selected_koma--;
                            }
                            activekoma_change(user, -1);
                            activekoma_change(user, selected_koma, true);
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            //→キー
                            if (selected_koma >= 7)
                            {
                                selected_koma = 0;
                            }
                            else
                            {
                                selected_koma++;
                            }
                            activekoma_change(user, -1);
                            activekoma_change(user, selected_koma);
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            if (can_select_dice(selected))  /* 選択中のダイスが選択可能か */
                            {
                                selected_dice_change(selected, true);
                                //activekoma_change(user, 0);
                                //コマ移動確定（検証）
                                if (canmovekoma(user, selected_koma, selected))
                                {
                                    //移動完了
                                    remain--;
                                    selected_koma = 0;
                                    activekoma_change(user, -1);
                                    activekoma_change(user, selected_koma);
                                }
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            selected_dice_change(1);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            selected_dice_change(2);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha3))
                        {
                            selected_dice_change(3);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha4))
                        {
                            selected_dice_change(4);
                        }
                    }
                    else
                    {
                        Debug.Log("動かせるコマがありません！！(stucked!!)");
                        todotext.text = "スタックしました！\n[Space]キーを押して交代です。";
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            activekoma_change(user, -1);
                            selected_dice_change(selected);
                            selected_koma = 0;
                            remain = 0;
                        }
                    }
                }
                else
                {
                    /* プレイヤー交代 */
                    activekoma_change(user, -1);
                    user = !user;
                    if (user)
                    {
                        turnuser.transform.localPosition = new Vector3(-315, -75, 0);
                    }
                    else
                    {
                        turnuser.transform.localPosition = new Vector3(315, -75, 0);
                    }
                    todotext.text = "ターンチェンジ。\n[D]キーを押してダイスを振りましょう。";
                    canroll = true;
                    selected_koma = 0;
                    activekoma_change(user, -1);
                    activekoma_change(user, selected_koma);
                }
                remaintext.text = "残ダイス数:" + remain.ToString();

            }
        }
        else if (gameready)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                //順番決め
                todotext.text = "先攻を決めています…";
                do
                {
                    roll1 = diceroll();
                    roll2 = diceroll();
                    diceapply(roll1, roll2);
                } while (roll1 == roll2);


                if (roll1 > roll2)
                {
                    user = true;
                    turnuser.transform.localPosition = new Vector3(-315, -75, 0);
                    todotext.text = "←　先攻が決まりました！";
                }
                else
                {
                    user = false;
                    turnuser.transform.localPosition = new Vector3(315, -75, 0);
                    todotext.text = "先攻が決まりました！　→";
                }
                todotext.text += "\nダイスとコマを選んで動かしましょう。";
                activekoma_change(user, 0);
                remain = 2;
                canroll = false;
                ongame = true;
                gameready = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("go_multiplay");
            }
        }
    }

    int diceroll()  /* ダイスを振る */
    {
        int diceval;
        if (playsound)
        {
            se.clip = dicese;
            se.Play();
        }
        diceval = Random.Range(0, 3);
        return diceval;
    }

    void diceapply(int dice1, int dice2, bool zoro = false) /* ダイスの画像を反映 */
    {
        /*
         * ダイスエフェクト（墓地）
        timecounter = 0;
        int counter = 0;
        int tempdice = 0;
        while (counter != 10)
        {
            if (timecounter >= 2)
            {
                tempdice = Random.Range(0, 3);
                switch (tempdice)
                {
                    case 0:
                        diceview1_obj.GetComponent<Image>().material = di1;
                        diceview2_obj.GetComponent<Image>().material = di1;
                        diceview3_obj.GetComponent<Image>().material = di1;
                        diceview4_obj.GetComponent<Image>().material = di1;
                        break;
                    case 1:
                        diceview1_obj.GetComponent<Image>().material = di2;
                        diceview2_obj.GetComponent<Image>().material = di2;
                        diceview3_obj.GetComponent<Image>().material = di2;
                        diceview4_obj.GetComponent<Image>().material = di2;
                        break;
                    case 2:
                        diceview1_obj.GetComponent<Image>().material = di3;
                        diceview2_obj.GetComponent<Image>().material = di3;
                        diceview3_obj.GetComponent<Image>().material = di3;
                        diceview4_obj.GetComponent<Image>().material = di3;
                        break;
                }
                counter++;
                timecounter = 0;
            }
            timecounter += Time.deltaTime;
        }

        diceview1_obj.GetComponent<Image>().material = null;
        diceview2_obj.GetComponent<Image>().material = null;
        diceview3_obj.GetComponent<Image>().material = null;
        diceview4_obj.GetComponent<Image>().material = null;
        */

        if (zoro)
        {
            switch (dice1)
            {
                case 0:
                    diceview1_obj.GetComponent<Image>().material = di1;
                    diceview2_obj.GetComponent<Image>().material = di1;
                    diceview3_obj.GetComponent<Image>().material = di1;
                    diceview4_obj.GetComponent<Image>().material = di1;
                    break;
                case 1:
                    diceview1_obj.GetComponent<Image>().material = di2;
                    diceview2_obj.GetComponent<Image>().material = di2;
                    diceview3_obj.GetComponent<Image>().material = di2;
                    diceview4_obj.GetComponent<Image>().material = di2;
                    break;
                case 2:
                    diceview1_obj.GetComponent<Image>().material = di3;
                    diceview2_obj.GetComponent<Image>().material = di3;
                    diceview3_obj.GetComponent<Image>().material = di3;
                    diceview4_obj.GetComponent<Image>().material = di3;
                    break;
                default:
                    Debug.Log("[Error] (diceapply:true) switch overflow!!! [" + dice1 + " / " + zoro + "]");
                    break;
            }
            activedice1 = true;
            activedice2 = true;
            activedice3 = true;
            activedice4 = true;
        }
        else
        {
            switch (dice1)
            {
                case 0:
                    diceview1_obj.GetComponent<Image>().material = di1;
                    break;
                case 1:
                    diceview1_obj.GetComponent<Image>().material = di2;
                    break;
                case 2:
                    diceview1_obj.GetComponent<Image>().material = di3;
                    break;
                default:
                    Debug.Log("[Error] (diceapply:false:dice1) switch overflow!!! [" + dice1 + " / " + dice2 + " / " + zoro + "]");
                    break;
            }
            switch (dice2)
            {
                case 0:
                    diceview4_obj.GetComponent<Image>().material = di1;
                    break;
                case 1:
                    diceview4_obj.GetComponent<Image>().material = di2;
                    break;
                case 2:
                    diceview4_obj.GetComponent<Image>().material = di3;
                    break;
                default:
                    Debug.Log("[Error] (diceapply:false:dice2) switch overflow!!! [" + dice1 + " / " + dice2 + " / " + zoro + "]");
                    break;

            }
            diceview2_obj.GetComponent<Image>().material = null;
            diceview3_obj.GetComponent<Image>().material = null;
            activedice1 = true;
            activedice2 = false;
            activedice3 = false;
            activedice4 = true;
        }
        return;
    }

    void selected_dice_change(int value, bool select = false) /* コマ選択時にコマ確定にテキスト変更 */
    {
        if (select)
        {
            switch (value)
            {
                case 1:
                    selected_dice_1.text = "(1)";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "4";
                    selected = 1;
                    break;
                case 2:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "(2)";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "4";
                    selected = 2;
                    break;
                case 3:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "(3)";
                    selected_dice_4.text = "4";
                    selected = 3;
                    break;
                case 4:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "(4)";
                    selected = 4;
                    break;
                default:
                    Debug.Log("[Error] (selected_dice_change:true) switch overflow!!! [" + value + " / " + select + "]");
                    break;
            }
        }
        else
        {
            switch (value)
            {
                case 1:
                    selected_dice_1.text = "[1]";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "4";
                    selected = 1;
                    break;
                case 2:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "[2]";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "4";
                    selected = 2;
                    break;
                case 3:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "[3]";
                    selected_dice_4.text = "4";
                    selected = 3;
                    break;
                case 4:
                    selected_dice_1.text = "1";
                    selected_dice_2.text = "2";
                    selected_dice_3.text = "3";
                    selected_dice_4.text = "[4]";
                    selected = 4;
                    break;
                default:
                    Debug.Log("[Error] (activedice_change:false) switch overflow!!! [" + value + " / " + select + "]");
                    break;
            }
        }
        return;
    }

    bool can_select_dice(int value) /* 要求されたダイスが選択可能か */
    {
        bool canselect = false;
        switch (value)
        {
            case 1:
                if (activedice1)
                    canselect = true;
                break;
            case 2:
                if (activedice2)
                    canselect = true;
                break;
            case 3:
                if (activedice3)
                    canselect = true;
                break;
            case 4:
                if (activedice4)
                    canselect = true;
                break;
            default:
                Debug.Log("[Error] (can_select_dice) switch overflow!!! [" + value + "]");
                break;
        }
        return canselect;
    }

    void activedice_change(int value) /* 使用済みのダイスの画像を消す */
    {
        switch (value)
        {
            case 1:
                activedice1 = false;
                diceview1_obj.GetComponent<Image>().material = null;
                break;
            case 2:
                activedice2 = false;
                diceview2_obj.GetComponent<Image>().material = null;
                break;
            case 3:
                activedice3 = false;
                diceview3_obj.GetComponent<Image>().material = null;
                break;
            case 4:
                activedice4 = false;
                diceview4_obj.GetComponent<Image>().material = null;
                break;
            default:
                Debug.Log("[Error] (activedice_change) switch overflow!!! [" + value + "]");
                break;
        }
        return;
    }

    void activekoma_change(bool isblue, int komaid, bool keydownleft = false) /* 選択中のコマの色を変える */
    {
        if (!(ground[0] <= -8 || ground[15] >= 8))
        {
            if (isblue)  //青ターン
            {
                if (komaid != -1)   //-1だと全部のコマの色を初期化する
                {
                    if (ground[14] != 0)
                    {
                        //漂流コマがいる場合はそれを優先する
                        int koma = getenemykomaid(14);
                        komaid = koma;
                        selected_koma = koma;
                    }
                    else
                    {
                        if (keydownleft)    //押されたキーが←の場合
                        {
                            while (lkoma[komaid] <= 0)
                            {
                                komaid--;
                                selected_koma--;
                                if (komaid <= -1)
                                {
                                    komaid = 7;
                                    selected_koma = 7;
                                }
                            }
                        }
                        else    //押されたキーが→の場合
                        {
                            while (lkoma[komaid] <= 0)
                            {
                                komaid++;
                                selected_koma++;
                                if (komaid >= 8)
                                {
                                    komaid = 0;
                                    selected_koma = 0;
                                }
                            }
                        }
                    }
                    Debug.Log("isblue:" + isblue + " / KomaID:" + komaid + "  を選択します。");
                }

                switch (komaid)
                {
                    case 0:
                        lkoma0_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 1:
                        lkoma1_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 2:
                        lkoma2_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 3:
                        lkoma3_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 4:
                        lkoma4_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 5:
                        lkoma5_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 6:
                        lkoma6_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    case 7:
                        lkoma7_obj.GetComponent<Image>().material = lkoma_selected;
                        break;
                    default:
                        if (lkoma[0] != 0)
                        {
                            lkoma0_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[1] != 0)
                        {
                            lkoma1_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[2] != 0)
                        {
                            lkoma2_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[3] != 0)
                        {
                            lkoma3_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[4] != 0)
                        {
                            lkoma4_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[5] != 0)
                        {
                            lkoma5_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[6] != 0)
                        {
                            lkoma6_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        if (lkoma[7] != 0)
                        {
                            lkoma7_obj.GetComponent<Image>().material = lkoma_notselected;
                        }
                        break;
                }
            }
            else
            {
                if (komaid != -1)
                {
                    if (ground[1] != 0)
                    {
                        //漂流コマがいる場合はそれを優先する
                        int koma = getenemykomaid(1);
                        komaid = koma;
                        selected_koma = koma;
                    }
                    else
                    {
                        if (keydownleft)
                        {
                            while (rkoma[komaid] >= 15)
                            {
                                komaid--;
                                selected_koma--;
                                if (komaid <= -1)
                                {
                                    komaid = 7;
                                    selected_koma = 7;
                                }
                            }
                        }
                        else
                        {
                            while (rkoma[komaid] >= 15)
                            {
                                komaid++;
                                selected_koma++;
                                if (komaid >= 8)
                                {
                                    komaid = 0;
                                    selected_koma = 0;
                                }
                            }
                        }
                    }
                    Debug.Log(komaid + "を選択します。");
                }

                switch (komaid)
                {
                    case 0:
                        rkoma0_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 1:
                        rkoma1_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 2:
                        rkoma2_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 3:
                        rkoma3_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 4:
                        rkoma4_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 5:
                        rkoma5_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 6:
                        rkoma6_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    case 7:
                        rkoma7_obj.GetComponent<Image>().material = rkoma_selected;
                        break;
                    default:
                        if (rkoma[0] != 15)
                        {
                            rkoma0_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[1] != 15)
                        {
                            rkoma1_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[2] != 15)
                        {
                            rkoma2_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[3] != 15)
                        {
                            rkoma3_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[4] != 15)
                        {
                            rkoma4_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[5] != 15)
                        {
                            rkoma5_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[6] != 15)
                        {
                            rkoma6_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        if (rkoma[7] != 15)
                        {
                            rkoma7_obj.GetComponent<Image>().material = rkoma_notselected;
                        }
                        break;
                }
            }
        }
        return;
    }

    bool canmovekoma(bool isblue, int komaid, int diceselected) /* コマの移動可否検証及び実行 */
    {
        bool canmove = false;
        bool cantbyanother = false;
        /* 移動可否判定ここから */
        bool hasenemy = false;
        //ダイスの移動数取得
        int move = 0;
        switch (diceselected)
        {
            case 1:
                move = roll1 + 1;
                break;
            case 4:
                move = roll2 + 1;
                break;
            default:
                move = roll1 + 1;
                break;
        }

        Debug.Log("[Function Join] canmovekoma\nisblue?:" + isblue + " / KomaID:" + komaid + " / SelectedDiceID:" + diceselected + " / Move:" + move);
        //コマ移動ここから
        int komapos = -1;
        if (isblue) //左のターンの場合（足す）
        {
            komapos = lkoma[komaid];    //コマの現在地取得
            if (ground[14] != 0)    //漂流兵がいる場合に選択したコマがそれか
            {
                if (komapos != 14)
                {
                    Debug.Log("先に漂流されているコマを動かしてね！\nground[14]: " + ground[14]);
                    todotext.text = "先に漂流中の味方を動かしましょう！";
                    debugtext.text = "cmc:blue/1";
                    if (playsound)
                    {
                        se.clip = cantse;
                        se.Play();
                    }
                    return canmove;
                }
            }
            Debug.Log("右コマ(LKoma)ID:" + komaid + " は、現在ground[" + komapos + "]に位置しています。\nまた、監獄がなければ、ground[" + (komapos - move) + "]へ移動します。");
            if ((komapos - move) <= 1)    //移動先が監獄だった場合、ゴールへ
            {
                Debug.Log("移動先がゴールです。");
                debugtext.text = "cmc:blue/2";
                if (cangoal(isblue, komaid))
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            debugtext.text = "cmc:blue/3";
                            canmove = true;
                            goal(isblue, komaid);
                        }
                        else
                        {
                            cantbyanother = true;
                            debugtext.text = "cmc:blue/4";
                        }
                    }
                    else
                    {
                        canmove = true;
                        debugtext.text = "cmc:blue/0";
                    }
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    todotext.text = "仲間を置いて帰宅はできません！";
                    debugtext.text = "cmc:blue/5";
                    if (playsound)
                    {
                        se.clip = cantse;
                        se.Play();
                    }
                    return canmove;
                }
                move = komapos - 0;
            }
            else if (ground[komapos - move] != 0)    //移動先に何かしらのコマがある場合
            {
                Debug.Log("移動先に何かしらのコマがあります。");
                debugtext.text = "cmc:blue/6";
                if (ground[komapos - move] > 0)    //移動先が敵のコマの場合
                {
                    Debug.Log("移動先に敵兵発見！");
                    debugtext.text = "cmc:blue/7";
                    if (ground[komapos - move] == 1)    //敵のコマ1つしかないよ
                    {
                        Debug.Log("敵コマを制圧します。");
                        debugtext.text = "cmc:blue/8";
                        hasenemy = true;
                        canmove = true;
                    }
                }
                else    //味方のコマの場合
                {
                    debugtext.text = "cmc:blue/9";
                    if (cangoal(isblue, komaid, false))
                    {
                        debugtext.text = "cmc:blue/10";
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                            debugtext.text = "cmc:blue/11";
                        }
                        else
                        {
                            cantbyanother = true;
                            debugtext.text = "cmc:blue/12";
                        }
                    }
                    else
                    {
                        Debug.Log("味方のコマと合流します。");
                        debugtext.text = "cmc:blue/13";
                        canmove = true;
                    }
                }
            }
            else    //移動先にコマがない場合
            {
                if (cangoal(isblue, komaid, false))
                {
                    if (noanothergoal(isblue, komaid, move))
                    {
                        canmove = true;
                        debugtext.text = "cmc:blue/15";
                    }
                    else
                    {
                        cantbyanother = true;
                        debugtext.text = "cmc:blue/16";
                    }

                }
                else
                {
                    debugtext.text = "cmc:blue/14";
                    Debug.Log("新規領域を占領します。");
                    canmove = true;
                }
            }
        }
        else    //右のターンの場合（引く）
        {
            debugtext.text = "cmc:blue/19";
            komapos = rkoma[komaid];    //コマの現在地取得
            if (ground[1] != 0)    //漂流兵がいる場合に選択したコマがそれか
            {
                debugtext.text = "cmc:blue/20";
                if (komapos != 1)
                {
                    Debug.Log("先に漂流されているコマを動かしてね！\nground[1]: " + ground[1]);
                    todotext.text = "漂流中の仲間を先に動かしましょう！";
                    debugtext.text = "cmc:blue/21";
                    if (playsound)
                    {
                        se.clip = cantse;
                        se.Play();
                    }
                    return canmove;
                }
            }
            Debug.Log("右側 コマID:" + komaid + " は、現在 " + komapos + "に位置しています。\nまた、監獄がなければ、ground[" + (komapos + move) + "]へ移動します。");
            if ((komapos + move) >= 14)    //移動先が監獄だった場合、ゴールへ
            {
                Debug.Log("移動先がゴールです。");
                debugtext.text = "cmc:blue/22";
                if (cangoal(isblue, komaid))
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        debugtext.text = "cmc:blue/23";
                        if (noanothergoal(isblue, komaid, move))
                        {
                            debugtext.text = "cmc:blue/25";
                            goal(isblue, komaid);
                            canmove = true;
                        }
                        else
                        {
                            debugtext.text = "cmc:blue/24";
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        debugtext.text = "cmc:blue/33";
                        canmove = true;
                    }
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    todotext.text = "全ての仲間が陣地内に収まっていません！";
                    debugtext.text = "cmc:blue/26";
                    if (playsound)
                    {
                        se.clip = cantse;
                        se.Play();
                    }
                    return canmove;
                }
                move = Mathf.Abs(komapos - 15);
            }
            else if (ground[komapos + move] != 0)    //移動先に何かしらのコマがある場合
            {
                debugtext.text = "cmc:blue/27";
                Debug.Log("移動先に何かしらのコマを検知しました。");
                if (ground[komapos + move] < 0)    //移動先が敵のコマの場合
                {
                    Debug.Log("移動先に敵のコマがあります。偵察を開始。");
                    debugtext.text = "cmc:blue/28";
                    if (ground[komapos + move] == -1)    //敵のコマ1つしかないよ
                    {
                        Debug.Log("敵コマを制圧します。");
                        debugtext.text = "cmc:blue/29";
                        hasenemy = true;
                        canmove = true;
                    }
                }
                else    //味方のコマの場合
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                            debugtext.text = "cmc:blue/31";
                        }
                        else
                        {
                            cantbyanother = true;
                            debugtext.text = "cmc:blue/32";
                        }
                    }
                    else
                    {
                        debugtext.text = "cmc:blue/30";
                        Debug.Log("味方と合流します。");
                        canmove = true;
                    }
                }
            }
            else    //移動先にコマがない場合
            {
                debugtext.text = "cmc:blue/35";
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                            debugtext.text = "cmc:blue/36";
                        }
                        else
                        {
                            cantbyanother = true;
                            debugtext.text = "cmc:blue/37";
                        }
                    }
                    else
                    {
                        Debug.Log("新規領域を占領します。");
                        debugtext.text = "cmc:blue/38";
                        canmove = true;
                    }
                }
            }
        }
        if (canmove)
        {
            movekoma(isblue, komaid, komapos, move, hasenemy, diceselected);
        }
        else if (canmove == false && cantbyanother == false)
        {
            if (playsound)
            {
                se.clip = cantse;
                se.Play();
            }
            int grounds = -1, ti = -1;
            if (isblue)
            {
                grounds = komapos - move;
                ti = ground[komapos - move];
            }
            else
            {
                grounds = komapos + move;
                ti = ground[komapos + move];
            }
            Debug.Log("敵が多すぎて動けません！\nground[" + grounds + "]@" + ti);
            todotext.text = "移動先の敵が多すぎます！";
        }
        else
        {
            if (playsound)
            {
                se.clip = cantse;
                se.Play();
            }
            Debug.Log("ほかに移動できるコマがあります。");
            todotext.text = "移動優先度が高い別のコマが\nあります。";
        }
        return canmove;
    }

    void movekoma(bool isblue, int komaid, int komapos, int move, bool hasenemy, int diceid) /* コマを移動 */
    {
        //ダイス移動処理ここから
        Debug.Log("[Function Join] movekoma\n左側？:" + isblue + " / コマID:" + komaid + " / コマの座標:" + komapos + " / 移動コマ数:" + move + " / 敵がいるか？:" + hasenemy);
        if (isblue)
        {
            //左側プレイヤー
            if (hasenemy)
            {

                //移動先マスに敵が1体いる場合
                int enemy = getenemykomaid(komapos - move); //飛ばされる敵のコマを取得

                //ロールバック処理のための記録
                rb_prison = true;
                rb_prisonid = enemy;
                rb_lp = lkoma[komaid];
                rb_rp = rkoma[enemy];
                rb_lid = komaid;
                rb_rid = enemy;
                rb_bg = ground[komapos];
                rb_mg = ground[komapos - move];
                rb_ag = ground[1];
                rb_diceid = diceid;
                rb_wasblue = isblue;
                rb_canback = true;

                ground[1]++;    //監獄に加算
                if (playsound)
                {
                    se.clip = prisonse;
                    se.Play();
                }

                rkoma[enemy] = 1;   //飛ばされたコマの座標を監獄に設定。
                movekoma_apply(isblue, enemy, 1, true, enemy);   //飛ばされたコマの描画処理。
                ground[komapos - move] = -1;    //敵がいたコマに自分の値を確保
                lkoma[komaid] = komapos - move; //自分のコマの座標を設定。
                ground[komapos]++;  //自分の元いた場所の処理
                movekoma_apply(isblue, komaid, lkoma[komaid]);   //移動した自分のコマの描画処理。
                Debug.Log("左側、移動先(ground[" + (komapos - move) + "])に敵アリ\n lkoma[" + komaid + "] (" + komapos + ") => (" + (komapos - move) + ")／rkoma[" + enemy + "](" + rkoma[enemy] + ") => (1)／ground[" + (komapos - move) + "]@" + ground[komapos - move]);
            }
            else
            {
                Debug.Log("左側、移動先(ground[" + (komapos - move) + "])に敵ナシ");
                //敵がいない場合

                rb_prison = false;
                rb_prisonid = -1;
                rb_lp = lkoma[komaid];
                rb_rp = -1;
                rb_lid = komaid;
                rb_rid = -1;
                rb_bg = ground[komapos];
                rb_mg = ground[komapos - move];
                rb_ag = -1;
                rb_diceid = diceid;
                rb_wasblue = isblue;
                rb_canback = true;

                if (playsound)
                {
                    se.clip = movekomase;
                    se.Play();
                }

                lkoma[komaid] = komapos - move;
                ground[komapos]++;
                ground[komapos - move]--;
                movekoma_apply(isblue, komaid, lkoma[komaid]);
            }
        }
        else
        {
            //右側プレイヤー
            if (hasenemy)
            {
                int enemy = getenemykomaid(komapos + move); //飛ばされる敵のコマを取得

                //ロールバック処理のための記録
                rb_prison = true;
                rb_prisonid = enemy;
                rb_lp = lkoma[enemy];
                rb_rp = rkoma[komaid];
                rb_lid = enemy;
                rb_rid = komaid;
                rb_bg = ground[komapos];
                rb_mg = ground[komapos + move];
                rb_ag = ground[14];
                rb_diceid = diceid;
                rb_wasblue = isblue;
                rb_canback = true;

                //移動先マスに敵が1コマいる場合
                ground[14]--;    //監獄に加算

                if (playsound)
                {
                    se.clip = prisonse;
                    se.Play();
                }

                lkoma[enemy] = 14;   //飛ばされたコマの座標を監獄に設定。
                movekoma_apply(isblue, enemy, 14, true, enemy);   //飛ばされたコマの描画処理。
                ground[komapos + move] = 1;    //敵がいたコマに自分の値を確保
                rkoma[komaid] = komapos + move; //自分のコマの座標を設定。
                ground[komapos]--;  //自分の元いた場所の処理。
                movekoma_apply(isblue, komaid, rkoma[komaid]);   //移動した自分のコマの描画処理。
                Debug.Log("ground[" + (komapos + move) + "]@" + ground[komapos + move]);
                Debug.Log("右側、移動先(ground[" + (komapos + move) + "])に敵アリ\n rkoma[" + komaid + "] (" + komapos + ") => (" + (komapos + move) + ")／lkoma[" + enemy + "](" + lkoma[enemy] + ") => (14)");
            }
            else
            {
                Debug.Log("右側、移動先(ground[" + (komapos - move) + "])に敵ナシ");
                //敵がいない場合

                rb_prison = false;
                rb_prisonid = -1;
                rb_lp = -1;
                rb_rp = rkoma[komaid];
                rb_lid = -1;
                rb_rid = komaid;
                rb_bg = ground[komapos];
                rb_mg = ground[komapos + move];
                rb_ag = -1;
                rb_diceid = diceid;
                rb_wasblue = isblue;
                rb_canback = true;

                if (playsound)
                {
                    se.clip = movekomase;
                    se.Play();
                }

                rkoma[komaid] = komapos + move;
                ground[komapos]--;
                ground[komapos + move]++;
                movekoma_apply(isblue, komaid, rkoma[komaid]);
            }
        }

        //反映処理
        int grounds = -1, ti = -1;
        if (isblue)
        {
            grounds = komapos - move;
            ti = ground[komapos - move];
        }
        else
        {
            grounds = komapos + move;
            ti = ground[komapos + move];
        }
        Debug.Log("コマを移動しました。\nground[" + grounds + "]の新しい値は" + ti);
        activedice_change(selected);
        selected_dice_change(selected);
        textupdate();
        return;
    }

    int getenemykomaid(int groundid)
    {
        Debug.Log("[Function Join] getenemykomaid");
        bool notfound = true;
        int cnt = 0;
        bool loop_l = true;
        //指定されたフィールドにいるコマIDを返却
        while (notfound)
        {
            if (loop_l)
            {
                if (lkoma[cnt] == groundid)
                {
                    notfound = false;
                    break;
                }
                Debug.Log("[Info] (getenemykomaid)\nlkoma[" + cnt + "] != " + groundid + " / Check:" + notfound);
                cnt++;
                if (cnt >= 8)
                {
                    cnt = 0;
                    loop_l = false;
                }
            }
            else
            {
                if (rkoma[cnt] == groundid)
                {
                    notfound = false;
                    break;
                }
                Debug.Log("[Info] (getenemykomaid)\nrkoma[" + cnt + "] != " + groundid + " / Check:" + notfound);
                cnt++;
                if (cnt > 7)
                {
                    cnt = -1;
                    notfound = false;
                }
            }
        }
        if (cnt == -1)
        {
            Debug.Log("[Error] (getenemykomaid) That komaID Not Found!!!(Enemy Not Found!)");
        }
        return cnt;
    }

    void movekoma_apply(bool isblue, int komaid, int fieldid, bool goprison = false, int goprison_komaid = -1)
    {
        Debug.Log("[Function Join] movekoma_apply\nisblue:" + isblue + " / KomaID:" + komaid + " / FieldID:ground[" + fieldid + "] / 監獄転送モード:" + goprison);
        if (goprison)
        {
            isblue = !isblue;
            komaid = goprison_komaid;
        }

        int posX = -255;
        int idousu = ((fieldid - 1) * 39);
        int newposX = posX + idousu;
        int AddposX = 7 * komaid;

        //コマをフィールドに描画反映。
        if (isblue)
        {
            Debug.Log("左側コマの描画適用");
            //左側のコマ

            switch (komaid)
            {
                case 0:
                    if (goprison)
                        lkoma0_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma0_obj.transform.localPosition = new Vector3(newposX, -60, 0);
                    break;
                case 1:
                    if (goprison)
                        lkoma1_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma1_obj.transform.localPosition = new Vector3(newposX, -10, 0);
                    break;
                case 2:
                    if (goprison)
                        lkoma2_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma2_obj.transform.localPosition = new Vector3(newposX, -35, 0);
                    break;
                case 3:
                    if (goprison)
                        lkoma3_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma3_obj.transform.localPosition = new Vector3(newposX, 15, 0);
                    break;
                case 4:
                    if (goprison)
                        lkoma4_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma4_obj.transform.localPosition = new Vector3(newposX, 65, 0);
                    break;
                case 5:
                    if (goprison)
                        lkoma5_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma5_obj.transform.localPosition = new Vector3(newposX, 40, 0);
                    break;
                case 6:
                    if (goprison)
                        lkoma6_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma6_obj.transform.localPosition = new Vector3(newposX, 90, 0);
                    break;
                case 7:
                    if (goprison)
                        lkoma7_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma7_obj.transform.localPosition = new Vector3(newposX, 135, 0);
                    break;
                default:
                    Debug.Log("[Error] (movekoma_apply) switch overflow!!! [" + komaid + "]");
                    break;
            }
        }
        else
        {
            Debug.Log("右側コマの描画適用");
            switch (komaid)
            {
                case 0:
                    if (goprison)
                        rkoma0_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma0_obj.transform.localPosition = new Vector3(newposX, -60, 0);
                    break;
                case 1:
                    if (goprison)
                        rkoma1_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma1_obj.transform.localPosition = new Vector3(newposX, -10, 0);
                    break;
                case 2:
                    if (goprison)
                        rkoma2_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma2_obj.transform.localPosition = new Vector3(newposX, -35, 0);
                    break;
                case 3:
                    if (goprison)
                        rkoma3_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma3_obj.transform.localPosition = new Vector3(newposX, 15, 0);
                    break;
                case 4:
                    if (goprison)
                        rkoma4_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma4_obj.transform.localPosition = new Vector3(newposX, 65, 0);
                    break;
                case 5:
                    if (goprison)
                        rkoma5_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma5_obj.transform.localPosition = new Vector3(newposX, 40, 0);
                    break;
                case 6:
                    if (goprison)
                        rkoma6_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma6_obj.transform.localPosition = new Vector3(newposX, 90, 0);
                    break;
                case 7:
                    if (goprison)
                        rkoma7_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma7_obj.transform.localPosition = new Vector3(newposX, 135, 0);
                    break;
                default:
                    Debug.Log("[Error] (movekoma_apply) switch overflow!!! [" + komaid + "]");
                    break;
            }
        }
        return;
    }

    void goal(bool isblue, int komaid)
    {
        if (playsound)
        {
            se.clip = goalse;
            se.Play();
        }
        Debug.Log("[Function Join] goal\nisblue:" + isblue + " / KomaID:" + komaid);
        if (isblue)
        {
            switch (komaid)
            {
                case 0:
                    lkoma0_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma0_obj.transform.localPosition = new Vector3(-259, -43, 0);
                    break;
                case 1:
                    lkoma1_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma1_obj.transform.localPosition = new Vector3(-312, -74, 0);
                    break;
                case 2:
                    lkoma2_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma2_obj.transform.localPosition = new Vector3(-294, -40, 0);
                    break;
                case 3:
                    lkoma3_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma3_obj.transform.localPosition = new Vector3(-367, -70, 0);
                    break;
                case 4:
                    lkoma4_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma4_obj.transform.localPosition = new Vector3(-291, 152, 0);
                    break;
                case 5:
                    lkoma5_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma5_obj.transform.localPosition = new Vector3(-317, 193, 0);
                    break;
                case 6:
                    lkoma6_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma6_obj.transform.localPosition = new Vector3(-265, 180, 0);
                    break;
                case 7:
                    lkoma7_obj.GetComponent<Image>().material = lkoma_goal;
                    lkoma7_obj.transform.localPosition = new Vector3(-363, 180, 0);
                    break;
                default:
                    Debug.Log("[Error] (goal) switch overflow!!! [" + isblue + " / " + komaid + "]");
                    break;
            }
        }
        else
        {
            //b
            switch (komaid)
            {
                case 0:
                    rkoma0_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma0_obj.transform.localPosition = new Vector3(262, -45, 0);
                    break;
                case 1:
                    rkoma1_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma1_obj.transform.localPosition = new Vector3(314, -76, 0);
                    break;
                case 2:
                    rkoma2_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma2_obj.transform.localPosition = new Vector3(348, -42, 0);
                    break;
                case 3:
                    rkoma3_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma3_obj.transform.localPosition = new Vector3(307, -38, 0);
                    break;
                case 4:
                    rkoma4_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma4_obj.transform.localPosition = new Vector3(287, 182, 0);
                    break;
                case 5:
                    rkoma5_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma5_obj.transform.localPosition = new Vector3(356, 179, 0);
                    break;
                case 6:
                    rkoma6_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma6_obj.transform.localPosition = new Vector3(249, 153, 0);
                    break;
                case 7:
                    rkoma7_obj.GetComponent<Image>().material = rkoma_goal;
                    rkoma7_obj.transform.localPosition = new Vector3(319, 170, 0);
                    break;
                default:
                    Debug.Log("[Error] (goal) switch overflow!!! [" + isblue + " / " + komaid + "]");
                    break;
            }
        }
        return;
    }

    void textupdate()
    {
        l_goal_label.text = Mathf.Abs(ground[0]).ToString() + "/8";
        r_goal_label.text = ground[15].ToString() + "/8";
        l_kick_label.text = "漂流: " + ground[1].ToString();
        r_kick_label.text = "漂流: " + Mathf.Abs(ground[14]).ToString();
        turntext.text = "Turn: " + (turn / 2 + 1).ToString();
        return;
    }

    bool cangoal(bool isblue, int komaid, bool includeit = true)
    {
        Debug.Log("[Function Join] (cangoal) Function joined!");
        bool ans = false;
        if (isblue)
        {
            //青
            for (int i = 0; i < 8; i++)
            {
                if (includeit)
                {
                    if (i == komaid)
                    {
                        ans = true;
                    }
                    else if (lkoma[i] <= 4)
                    {
                        ans = true;
                    }
                    else
                    {
                        ans = false;
                        Debug.Log("[Function] (cangoal) lkoma[" + i + "]は座標" + lkoma[i] + "に位置しているため、ゴール侵入は拒否されました。");
                        break;
                    }
                }
                else
                {
                    if (lkoma[i] <= 4)
                    {
                        ans = true;
                    }
                    else
                    {
                        ans = false;
                        Debug.Log("[Function] (cangoal) lkoma[" + i + "]は座標" + lkoma[i] + "に位置しているため、ゴール侵入は拒否されました。");
                        break;
                    }
                }
                Debug.Log("[Function] (cangoal) lkoma[" + i + "]は座標" + lkoma[i] + "に位置し、判定結果は" + ans + "です。");
            }
        }
        else
        {
            //赤
            for (int i = 0; i < 8; i++)
            {
                if (includeit)
                {
                    if (i == komaid)
                    {
                        ans = true;
                    }
                    else if (rkoma[i] >= 11)
                    {
                        ans = true;
                    }
                    else
                    {
                        ans = false;
                        Debug.Log("[Function] (cangoal) rkoma[" + i + "]は座標" + rkoma[i] + "に位置しているため、ゴール侵入は拒否されました。");
                        break;
                    }
                }
                else
                {
                    if (rkoma[i] >= 11)
                    {
                        ans = true;
                    }
                    else
                    {
                        ans = false;
                        Debug.Log("[Function] (cangoal) rkoma[" + i + "]は座標" + rkoma[i] + "に位置しているため、ゴール侵入は拒否されました。");
                        break;
                    }
                }
                Debug.Log("[Function] (cangoal) rkoma[" + i + "]は座標" + rkoma[i] + "に位置し、判定結果は" + ans + "です。");
            }
        }
        return ans;
    }

    void gamewin(bool isblue)
    {
        Debug.Log("[Function Join] (gamewin)");
        if (isblue)
        {
            todotext.text = "青色ペンギンさんチームの勝利！\nおめでとう！\n[R]を押してもう一度プレイできるぞ！";
            if (ground[15] == 0)
            {
                todotext.text = "青色ペンギンさんチームのギャモン勝ち！\n戦略ゲーは得意なのかな？\n[R]を押してもう一度プレイできるぞ！";
                if (playsound)
                {
                    se.clip = gyamonwin;
                    se.Play();
                }
            }
        }
        else
        {
            todotext.text = "赤色ペンギンさんチームの勝利！\nおめでとう！！\n[R]を押してもう一度プレイできるぞ！";
            if (ground[0] == 0)
            {
                todotext.text = "赤色ペンギンさんチームのギャモン勝ち！\nオリジナルのバックギャモンにも挑戦してみて！\n[R]を押してもう一度プレイできるぞ！";
                if (playsound)
                {
                    se.clip = gyamonwin;
                    se.Play();
                }
            }
        }
        if (playsound)
        {
            bgm.clip = clearbgm;
            bgm.Play();
        }
        gameready = false;
        ongame = false;
        remain = -1;
        activekoma_change(user, -1);
        activekoma_change(!user, -1);
        rb_canback = false;
        turntext.text = "経過ターンは" + (turn / 2 + 1).ToString() + "ターンでした。";
        return;
    }

    bool isstuck(bool isblue, int roll)   /* 全てのアクティブなコマが移動可能か */
    {
        Debug.Log("[Function Join] (isstuck) スタック判定を開始。");
        bool ans = true;
        int dice = roll + 1;
        if (isblue)
        {
            //青
            if (ground[14] == 0)    //監獄に誰もいない場合
            {
                for (int i = 0; i < 8; i++)
                {
                    if (lkoma[i] >= 2) //ゴールしていないコマを選択
                    {
                        if ((lkoma[i] - dice) >= 2)   //移動先がゴールより奥に行かない場合
                        {
                            //判定
                            if (ground[(lkoma[i] - dice)] <= 1) //移動先のフィールドに位置する敵コマが取得可能な場合
                            {
                                ans = false;
                                Debug.Log("(isstuck) 移動先のフィールドを取得可能です。");
                            }
                        }
                        else
                        {
                            if (cangoal(isblue, i))
                            {
                                //ゴールに行く場合
                                ans = false;
                                Debug.Log("(isstuck) 移動先のゴールを取得可能です。");
                            }
                        }
                    }
                }
            }
            else   //監獄に囚われがいる場合
            {
                if (ground[(14 - dice)] <= 1)
                {
                    //移動可能
                    ans = false;
                    Debug.Log("監獄に囚われがいますが移動可能です。");
                }
            }
        }
        else
        {
            //赤
            if (ground[1] == 0)    //監獄に誰もいない場合
            {
                for (int i = 0; i < 8; i++)
                {
                    if (rkoma[i] <= 13) //ゴールしていないコマを選択
                    {
                        if ((rkoma[i] + dice) <= 13)   //移動先がゴールより奥に行かない場合
                        {
                            //判定
                            if (ground[(rkoma[i] + dice)] >= -1) //移動先のフィールドに位置する敵コマが取得可能な場合
                            {
                                ans = false;
                                Debug.Log("(isstuck) 移動先のフィールドを取得可能です。");
                            }
                        }
                        else
                        {
                            if (cangoal(isblue, i))
                            {
                                //ゴールに行く場合
                                ans = false;
                                Debug.Log("(isstuck) 移動先のゴールを取得可能です。");
                            }
                        }
                    }
                }
            }
            else   //監獄に囚われがいる場合
            {
                if (ground[(1 + dice)] >= -1)
                {
                    //移動可能
                    ans = false;
                    Debug.Log("監獄に囚われがいますが移動可能です。");
                }
            }
        }
        if (ans)
        {
            Debug.Log("判定結果：スタックしています。移動できません。");
        }
        else
        {
            Debug.Log("判定結果：スタックしていません。移動可能です。");
        }
        return ans;
    }

    void returnmove()   /* ロールバック処理 */
    {
        if (rb_canback) //ロールバックが許可されているか
        {
            if (rb_wasblue)     //青ターン
            {
                if (rb_prison)  //敵を監獄に飛ばした
                {
                    ground[lkoma[rb_lid]] = rb_mg;
                    ground[1] = rb_ag;
                    lkoma[rb_lid] = rb_lp;
                    rkoma[rb_rid] = rb_rp;
                    ground[rb_lp] = rb_bg;
                    if (rb_lp == 14) //自分が監獄から出たときに敵を監獄に飛ばした
                    {
                        debugtext.text = "1";
                        movekoma_apply(true, rb_lid, rb_lp, true, rb_lid);
                        movekoma_apply(false, rb_rid, rb_rp);
                    }
                    else    //通常移動で敵を監獄に飛ばした
                    {
                        debugtext.text = "2";
                        movekoma_apply(true, rb_lid, rb_lp);
                        movekoma_apply(false, rb_rid, rb_rp);
                    }
                }
                else    //通常移動のみ
                {
                    ground[lkoma[rb_lid]] = rb_mg;
                    lkoma[rb_lid] = rb_lp;
                    ground[rb_lp] = rb_bg;
                    if (rb_lp == 14)    //監獄から出た
                    {
                        debugtext.text = "3";
                        movekoma_apply(true, rb_lid, rb_lp, true, rb_lid);
                    }
                    else    //通常状態から通常移動
                    {
                        debugtext.text = "4";
                        movekoma_apply(true, rb_lid, rb_lp);
                    }
                }
            }
            else    //赤ターン
            {
                if (rb_prison)  //敵を監獄に飛ばした
                {
                    ground[rkoma[rb_rid]] = rb_mg;
                    ground[14] = rb_ag;
                    lkoma[rb_lid] = rb_lp;
                    rkoma[rb_rid] = rb_rp;
                    ground[rb_rp] = rb_bg;
                    if (rb_rp == 1) //自分が監獄から出たときに敵を監獄に飛ばした
                    {
                        debugtext.text = "5";
                        movekoma_apply(false, rb_rid, rb_rp, true, rb_rid);
                        movekoma_apply(true, rb_lid, rb_lp);
                    }
                    else    //通常移動で敵を監獄に飛ばした
                    {
                        debugtext.text = "6";
                        movekoma_apply(false, rb_rid, rb_rp);
                        movekoma_apply(true, rb_lid, rb_lp);
                    }
                }
                else    //通常移動のみ
                {
                    ground[rkoma[rb_rid]] = rb_mg;
                    rkoma[rb_rid] = rb_rp;
                    ground[rb_rp] = rb_bg;
                    if (rb_rp == 1)
                    {
                        debugtext.text = "7";
                        movekoma_apply(false, rb_rid, rb_rp, true, rb_rid);
                    }
                    else
                    {
                        debugtext.text = "8";
                        movekoma_apply(false, rb_rid, rb_rp);
                    }
                }
            }
            remain++;

            if (rb_wasblue != user)
            {
                user = !user;
            }
            if (canroll)
            {
                canroll = false;
            }

            switch (rb_diceid)  //diceselectedと同値
            {
                case 1:
                    activedice1 = true;
                    switch (roll1)
                    {
                        case 0:
                            diceview1_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview1_obj.GetComponent<Image>().material = di2;
                            break;
                        case 2:
                            diceview1_obj.GetComponent<Image>().material = di3;
                            break;
                        default:
                            Debug.Log("[Error] (returnmove) Stack Overflow!!");
                            break;
                    }
                    break;
                case 2:
                    activedice2 = true;
                    switch (roll1)
                    {
                        case 0:
                            diceview2_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview2_obj.GetComponent<Image>().material = di2;
                            break;
                        case 2:
                            diceview2_obj.GetComponent<Image>().material = di3;
                            break;
                        default:
                            Debug.Log("[Error] (returnmove) Stack Overflow!!");
                            break;
                    }
                    break;
                case 3:
                    activedice3 = true;
                    switch (roll1)
                    {
                        case 0:
                            diceview3_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview3_obj.GetComponent<Image>().material = di2;
                            break;
                        case 2:
                            diceview3_obj.GetComponent<Image>().material = di3;
                            break;
                        default:
                            Debug.Log("[Error] (returnmove) Stack Overflow!!");
                            break;
                    }
                    break;
                case 4:
                    activedice4 = true;
                    switch (roll2)
                    {
                        case 0:
                            diceview4_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview4_obj.GetComponent<Image>().material = di2;
                            break;
                        case 2:
                            diceview4_obj.GetComponent<Image>().material = di3;
                            break;
                        default:
                            Debug.Log("[Error] (returnmove) Stack Overflow!!");
                            break;
                    }
                    break;
                default:
                    Debug.Log("[Error] (returnmove) Switch Overflow!!");
                    break;
            }
            activekoma_change(user, -1);
            activekoma_change(!user, -1);
            activekoma_change(user, 0);
            textupdate();
            Debug.Log("(returnmove) 戻しました。");
            todotext.text = "直近の1手を戻しました。\nコマを再度動かしてください。";
            rb_canback = false;
        }
        Debug.Log("(returnmove) 呼び出されました。");
        return;
    }

    bool noanothergoal(bool isblue, int komaid, int diceti)
    {
        bool ans = false;

        if (isblue)
        {
            //青ターン
            if (ground[(diceti + 1)] <= -1)
            {
                debugtext1.text = "移動に適切なコマがあります。";
                //コマがある場合は、選択したコマがジャストで動けるコマかチェックする
                if (lkoma[komaid] == (diceti + 1))
                {
                    debugtext1.text = "移動に適切なコマを選択しています。";
                    //ダイスの値とジャストで移動できるコマ
                    ans = true;
                }
                else
                {
                    //選択したコマ以外にジャストで動かせるコマがある

                    if (remain >= 2 && lkoma[komaid] == 4)
                    {
                        //lkomaがground[4]に位置し、3の目ダイスでゴール出来る場合
                        if (roll1 == 0 && roll2 == 1)
                        {
                            //1と2の目のとき
                            ans = true;
                        }
                        else if (roll1 == 1 && roll2 == 0)
                        {
                            //2と1の目のとき
                            ans = true;
                        }
                        else if (roll1 == 0 && roll2 == 0 && remain >= 3)
                        {
                            //1が３つあるとき
                            ans = true;
                        }
                    }
                    else if (remain >= 2 && lkoma[komaid] == 3)
                    {
                        //lkomaがground[3]に位置し、2の目ダイスでゴールできる場合
                        if (roll1 == 0 && roll2 == 0)
                        {
                            //1と1の目のとき
                            ans = true;
                        }
                    }
                    else
                    {
                        todotext.text = "他に移動可能なコマがあります。";
                        debugtext1.text = "ほかに移動可能。";
                    }
                }
            }
            else
            {
                //フィールドにコマがない場合はほかのコマ移動を許可する
                debugtext1.text = "移動に適切なコマがありません。";
                if ((lkoma[komaid] - 1) < diceti)
                {
                    switch (diceti)
                    {
                        case 1:
                            //後方（２，３マス目）にコマがあるか見る
                            if ((ground[3] != 0 && lkoma[komaid] == 3) || (ground[4] != 0 && lkoma[komaid] == 4))
                            {
                                ans = true;
                            }
                            else
                            {
                                todotext.text = "最後尾のコマを優先的に動かしましょう。";
                            }
                            break;
                        case 2:
                            //後方（３マス目）にコマがあるか見る
                            if (ground[4] != 0 && lkoma[komaid] == 4)
                            {
                                ans = true;
                            }
                            else
                            {
                                todotext.text = "一番後ろのコマを優先で動かしましょう。";
                            }
                            break;
                        default:
                            //do nothing
                            break;
                    }
                }
                else
                {
                    ans = true;
                }
            }
            debugtext1.text = "bg[" + (diceti + 1) + "] / " + ground[(diceti + 1)];
        }
        else
        {
            //赤ターン
            if (ground[(15 - diceti - 1)] >= 1)
            {
                debugtext1.text = "移動に適切なコマがあります。";
                //コマがある場合は、選択したコマがジャストで動けるコマかチェックする
                if (rkoma[komaid] == (15 - diceti - 1))
                {
                    debugtext1.text = "移動に適切なコマを選択。";
                    //ダイスの値とジャストで移動できるコマ
                    ans = true;
                }
                else
                {
                    //選択したコマ以外にジャストで動かせるコマがある

                    if (remain >= 2 && rkoma[komaid] == 11)
                    {
                        //lkomaがground[4]に位置し、3の目ダイスでゴール出来る場合
                        if (roll1 == 0 && roll2 == 1)
                        {
                            //1と2の目のとき、ダイスを２個使ってゴールへ
                            ans = true;
                        }
                        else if (roll1 == 1 && roll2 == 0)
                        {
                            //2と1の目のとき、ダイスを２個使ってゴールへ
                            ans = true;
                        }
                        else if (roll1 == 0 && roll2 == 0 && remain >= 3)
                        {
                            //ダイスを３つ使ってコマをゴールに導く
                            ans = true;
                        }
                    }
                    else if (remain >= 2 && rkoma[komaid] == 12)
                    {
                        //lkomaがground[3]に位置し、2の目ダイスでゴールできる場合
                        if (roll1 == 0 && roll2 == 0)
                        {
                            //1と1の目のとき
                            ans = true;
                        }
                    }
                    else
                    {
                        todotext.text = "他に移動可能なコマがあります。";
                        debugtext1.text = "他に移動可能なコマあり。";
                    }
                }
            }
            else
            {
                //フィールドにコマがない場合はほかのコマ移動を許可する
                debugtext1.text = "移動に適切なコマがありません。";
                if ((15 - rkoma[komaid] - 1) > diceti)
                {
                    switch (diceti)
                    {
                        case 1:
                            //後方（２，３マス目）にコマがあるか見る
                            if ((ground[12] != 0 && rkoma[komaid] == 12) || (ground[11] != 0 && rkoma[komaid] == 11))
                            {
                                ans = true;
                            }
                            else
                            {
                                ans = false;
                                todotext.text = "最後尾のコマを優先的に動かしましょう。";
                            }
                            break;

                        case 2:
                            //後方（３マス目）にコマがあるか見る
                            if (ground[11] != 0 && rkoma[komaid] == 11)
                            {
                                ans = true;
                            }
                            else
                            {
                                ans = false;
                                todotext.text = "一番後ろのコマを優先で動かしましょう。";
                            }
                            break;

                        default:
                            //do nothing
                            break;
                    }
                }
                else
                {
                    ans = true;
                }
            }
            debugtext1.text = "rg[" + (15 - diceti - 1) + "] / " + ground[(15 - diceti - 1)];
        }

        if (ans == false)
        {
            if (playsound)
            {
                se.clip = cantse;
                se.Play();
            }
        }

        return ans;
    }

}
