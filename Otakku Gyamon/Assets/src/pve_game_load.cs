using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pve_game_load : MonoBehaviour
{
    /*
     [改善点]
        * 先攻ターンがどっちか直感的にわからない
        * 異なる値のダイスがでた場合、大きいコマから優先的に使う。
    */

    bool user = false;  //プレイヤー(左側がtrue)
    bool gameready = false; //ready to game
    bool ongame = false;    //playing game
    bool canroll = false;   //ダイスロールの可否
    bool selectkoma = false;    //コマ選択モードか
    bool playsound; //サウンドの再生が許可されているか
    bool visiblefieldid;    //フィールド番号の表示
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
    bool playedse;

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
    public GameObject komapreview_obj;
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
    public Material hlkoma;
    public Material hrkoma;
    public Material cantgoal;
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
    public AudioClip stackse;
    public AudioClip rollbackse;
    public Text debug;

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
        l_goal_label.text = ground[0].ToString() + "/8";
        r_goal_label.text = ground[15].ToString() + "/8";
        l_kick_label.text = "漂流：" + ground[1].ToString();
        r_kick_label.text = "漂流：" + ground[14].ToString();
        playsound = true;
        visiblefieldid = true;
        playedse = false;


        komapreview(user, 0, 0, true);
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
            if (user)
            {    /* プレイヤーの動き */
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
                    if (playsound)
                    {
                        se.clip = rollbackse;
                        se.Play();
                    }
                }

                if (canroll)
                {
                    if (Input.GetKeyDown(KeyCode.D))    /* ダイスロール */
                    {
                        canroll = false;
                        rb_canback = false;


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

                        switch (selected)
                        {
                            case 1:
                                if (activedice1)
                                {
                                    komapreview(user, selected_koma, (roll1 + 1));
                                }
                                else
                                {
                                    komapreview(user, 0, 0, true);
                                }
                                break;
                            case 2:
                                if (activedice2)
                                {
                                    komapreview(user, selected_koma, (roll1 + 1));
                                }
                                else
                                {
                                    komapreview(user, 0, 0, true);
                                }
                                break;
                            case 3:
                                if (activedice3)
                                {
                                    komapreview(user, selected_koma, (roll1 + 1));
                                }
                                else
                                {
                                    komapreview(user, 0, 0, true);
                                }
                                break;
                            case 4:
                                if (activedice4)
                                {
                                    komapreview(user, selected_koma, (roll2 + 1));
                                }
                                else
                                {
                                    komapreview(user, 0, 0, true);
                                }
                                break;
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
                        playedse = false;
                        activekoma_change(user, -1);
                        activekoma_change(user, selected_koma);
                    }
                    remaintext.text = "残ダイス数:" + remain.ToString();

                }
            }
            else   /* NPCの動き */
            {
                /* ----------勝利判定---------- */
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
                /* ---------------------------- */

                /* ----------さいころ---------- */
                canroll = false;
                rb_canback = false;


                todotext.text = "ダイスを振っています…";
                roll1 = diceroll();
                roll2 = diceroll();
                todotext.text = "ダイスを振りました。\n数字／スペースキーでコマを移動しましょう。";


                if (roll1 == roll2)
                {
                    remain = 4;
                    //diceapply(roll1, roll2, true);
                }
                else
                {
                    remain = 2;
                    //diceapply(roll1, roll2);
                }
                /* ---------------------------- */

                /* -----------NPC移動---------- */
                int[] Dice = new int[4] { -10, -10, -10, -10 };/* ダイスの数を格納 */
                int[] Gstatus = new int[16];/* フィールドの状態格納 */
                int[] Nkoma = new int[8];/* NPC駒の位置格納 */
                int[,] Score = new int[4, 8];/* 移動確定のためのスコア格納 */
                int[] Move = new int[4] { 0, 0, 0, 0 };/* 移動確定比較変数 */
                int[] KomaNo = new int[4];/* 移動が確定した駒の値格納 */
                int[] DiceNo = new int[4] { -1, -1, -1, -1};/* 移動が確定したダイスの格納 */
                int Temp = 0;/* 入れ替え変数 */
                bool Gool = true;/* ゴールできるかの判定 */

                /* --ダイスの値格納-- */
                if (remain == 4)/* ダイスの値が同じ時の処理 */
                {
                    for (int i = 0; i < 4; i++)/* ダイス4つに数値を代入 */
                    {
                        Dice[i] = roll1 + 1;
                    }
                }
                else/* ダイスの値が違う場合の処理 */
                {
                    Dice[0] = roll1 + 1;
                    Dice[1] = roll2 + 1;
                }
                /* ------------------ */

                /* フィールド状態格納 */
                for (int i = 0; i < 16; i++)
                {
                    Gstatus[i] = ground[i];
                }
                /* ------------------ */

                /* ---NPC駒位置格納-- */
                for (int i = 0; i < 8; i++)
                {
                    Nkoma[i] = rkoma[i];
                }
                /* ------------------ */
                for (int Brain = 0; Brain < remain; Brain++)//移動させるコマの決定
                {
                    /* -------------------- スコア計算 -------------------- */
                    for (int x = 0; x < remain; x++)//Score配列・行
                    {
                        for (int y = 0; y < 8; y++)//Scoire配列・列
                        {
                            Score[x, y] = 0;//Score配列の初期化
                            if (Nkoma[y] + Dice[x] >= 16)//ゴール越え対策
                            {
                                for (int i = 1; i < 11; i++)//ゴールできるか判定
                                {
                                    if (Gstatus[i] > 0)
                                    {
                                        Score[x, y] = 0;
                                        break;
                                    }
                                    else
                                    {
                                        Score[x, y] = 100;
                                    }
                                }
                            }
                            else if (Nkoma[y] + Dice[x] == 14)//プレイヤー側島流し対策
                            {
                                Score[x, y] = 0;
                            }
                            else if (Gstatus[(Nkoma[y] + Dice[x])] >= -1)//動ける場合 
                            {
                                if (Gstatus[Nkoma[y] + Dice[x]] == -1)//移動先の敵の数が1
                                {
                                    Score[x, y] = 3 * (15 - Nkoma[y]);
                                }
                                else if (Gstatus[Nkoma[y] + Dice[x]] == 0)//移動先に誰もいない
                                {
                                    Score[x, y] = 2 * (15 - Nkoma[y]);
                                }
                                else//移動先に味方がいる
                                {
                                    Score[x, y] = 1 * (15 - Nkoma[y]);
                                }
                            }
                            else//移動ができない
                            {
                                Score[x, y] = 0;
                            }

                        }//Score配列・列終点
                    }//Score配列・行終点
                    /* -------------------- スコア計算 -------------------- */

                    /* -------------------- コマの決定 -------------------- */
                    for (int x = 0; x < remain; x++)//Score配列・行
                    {
                        for (int Dicecheak = 0; Dicecheak < remain; Dicecheak++)//同じサイコロが使われているかチェックする
                        {
                            if (DiceNo[Dicecheak] == Dicecheak)
                                x++;
                        }
                        if (x >= remain)//サイコロチェックでサイコロを最後まで使われていた場合処理を抜ける
                            break;
                        for (int y = 0; y < 8; y++)//Score配列・列
                        {
                            if (Move[Brain] <= Score[x, y])
                            {
                                Move[Brain] = Score[x, y];
                                KomaNo[Brain] = y;
                                DiceNo[Brain] = x;
                            }
                        }//Score配列・列終点
                    }//Score配列・行終点
                    /* -------------------- コマの決定 -------------------- */
                    Gstatus[Nkoma[KomaNo[Brain]] + Dice[DiceNo[Brain]]] += 1;//フィールドの状況更新
                }//Brain終点

                for(int i = 0; i < 15; i++)
                {
                    Debug.Log("Gsutatus[" + i + "]:" + Gstatus[i]);
                }

                for (int i = 0; i < 8; i++)
                {
                    Debug.Log("Score[" + i + "]:" + Score[0, i] + ":" + Score[1, i] + ":" + Score[2, i] + ":" + Score[3, i]);
                }
                Debug.Log("move[0]:" + Move[0] + "komano[0]:" + KomaNo[0] + "diceno[0]:" + DiceNo[0]);
                Debug.Log("move[1]:" + Move[1] + "komano[1]:" + KomaNo[1] + "diceno[1]:" + DiceNo[1]);
                Debug.Log("move[2]:" + Move[2] + "komano[2]:" + KomaNo[2] + "diceno[2]:" + DiceNo[2]);
                Debug.Log("move[3]:" + Move[3] + "komano[3]:" + KomaNo[3] + "diceno[3]:" + DiceNo[3]);
                Debug.Log("Dice[0]" + Dice[0]);
                Debug.Log("Dice[1]" + Dice[1]);
                Debug.Log("Dice[2]" + Dice[2]);
                Debug.Log("Dice[3]" + Dice[3]);

                /* -------移動------- */
                for (int i = 0; i < remain; i++)
                {
                    if (ground[Nkoma[KomaNo[i]] + Dice[DiceNo[i]]] <= -1) {
                        movekoma(user, KomaNo[i], Nkoma[KomaNo[i]], Dice[DiceNo[i]], true, DiceNo[i] + 1);
                    }
                    else
                    {
                        movekoma(user, KomaNo[i], Nkoma[KomaNo[i]], Dice[DiceNo[i]], false, DiceNo[i] + 1);
                    }

                }
                for (int i = 0; i < 8; i++)
                {
                    Debug.Log("Score[" + i + "]:" + Score[0, i] + ":" + Score[1, i] + ":" + Score[2, i] + ":" + Score[3, i]);
                }
                Debug.Log("move[0]:" + Move[0] + "komano[0]:" + KomaNo[0] + "diceno[0]:" + DiceNo[0]);
                Debug.Log("move[1]:" + Move[1] + "komano[1]:" + KomaNo[1] + "diceno[1]:" + DiceNo[1]);
                Debug.Log("move[2]:" + Move[2] + "komano[2]:" + KomaNo[2] + "diceno[2]:" + DiceNo[2]);
                Debug.Log("move[3]:" + Move[3] + "komano[3]:" + KomaNo[3] + "diceno[3]:" + DiceNo[3]);
                Debug.Log("Dice[0]" + Dice[0]);
                Debug.Log("Dice[1]" + Dice[1]);
                Debug.Log("Dice[2]" + Dice[2]);
                Debug.Log("Dice[3]" + Dice[3]);
                /* ------------------ */
                remain = 0;
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
                playedse = false;
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
                SceneManager.LoadScene("go_soloplay");
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

        switch (selected)
        {
            case 1:
                if (activedice1)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 2:
                if (activedice2)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 3:
                if (activedice3)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 4:
                if (activedice4)
                {
                    komapreview(user, selected_koma, (roll2 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
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
                        komapreview(true, 0, 0, true);
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
                        komapreview(false, 0, 0, true);
                        break;
                }
            }
        }

        switch (selected)
        {
            case 1:
                if (activedice1)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 2:
                if (activedice2)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 3:
                if (activedice3)
                {
                    komapreview(user, selected_koma, (roll1 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
            case 4:
                if (activedice4)
                {
                    komapreview(user, selected_koma, (roll2 + 1));
                }
                else
                {
                    komapreview(user, 0, 0, true);
                }
                break;
        }

        return;
    }

    bool canmovekoma(bool isblue, int komaid, int diceselected) /* コマの移動可否検証及び実行 */
    {
        bool canmove = false;
        bool cantbyanother = false;
        /* 移動可否判定ここから */
        bool hasenemy = false;
        bool goals = false;
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
                if (cangoal(isblue, komaid))
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                            goals = true;
                        }
                        else
                        {
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        canmove = true;
                    }
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    todotext.text = "仲間を置いて帰宅はできません！";
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
                if (ground[komapos - move] > 0)    //移動先が敵のコマの場合
                {
                    Debug.Log("移動先に敵兵発見！");
                    if (ground[komapos - move] == 1)    //敵のコマ1つしかないよ
                    {
                        Debug.Log("敵コマを制圧します。");
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
                        }
                        else
                        {
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        Debug.Log("味方のコマと合流します。");
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
                    }
                    else
                    {
                        cantbyanother = true;
                    }

                }
                else
                {
                    Debug.Log("新規領域を占領します。");
                    canmove = true;
                }
            }
        }
        else    //右のターンの場合（引く）
        {
            komapos = rkoma[komaid];    //コマの現在地取得
            if (ground[1] != 0)    //漂流兵がいる場合に選択したコマがそれか
            {
                if (komapos != 1)
                {
                    Debug.Log("先に漂流されているコマを動かしてね！\nground[1]: " + ground[1]);
                    todotext.text = "漂流中の仲間を先に動かしましょう！";
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
                if (cangoal(isblue, komaid))
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                            goals = true;
                        }
                        else
                        {
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        canmove = true;
                    }
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    todotext.text = "仲間を置いて帰れません！";
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
                Debug.Log("移動先に何かしらのコマを検知しました。");
                if (ground[komapos + move] < 0)    //移動先が敵のコマの場合
                {
                    Debug.Log("移動先に敵のコマがあります。偵察を開始。");
                    if (ground[komapos + move] == -1)    //敵のコマ1つしかないよ
                    {
                        Debug.Log("敵コマを制圧します。");
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
                        }
                        else
                        {
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        Debug.Log("味方と合流します。");
                        canmove = true;
                    }
                }
            }
            else    //移動先にコマがない場合
            {
                {
                    if (cangoal(isblue, komaid, false))
                    {
                        if (noanothergoal(isblue, komaid, move))
                        {
                            canmove = true;
                        }
                        else
                        {
                            cantbyanother = true;
                        }
                    }
                    else
                    {
                        Debug.Log("新規領域を占領します。");
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

        if (goals)
        {
            goal(isblue, komaid);
        }
        return canmove;
    }

    void movekoma(bool isblue, int komaid, int komapos, int move, bool hasenemy, int diceid) /* コマを移動 */
    {
        //ダイス移動処理ここから
        Debug.Log("[Function Join] movekoma\nisBlue?:" + isblue + " / コマID:" + komaid + " / コマの座標:" + komapos + " / 移動コマ数:" + move + " / 敵がいるか？:" + hasenemy);
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

            if (komapos == 14) //監獄出身者はロールバック不能に
            {
                rb_canback = false;
            }
        }
        else
        {
            //右側プレイヤー
            if (hasenemy)
            {
                int enemy = getenemykomaid(komapos + move); //飛ばされる敵のコマを取得
                Debug.Log("[Debug] enemy:" + enemy);
                Debug.Log("[Debug] lkoma[enemy]" + lkoma[enemy]);
                //ロールバック処理のための記録
               /* rb_prison = true;
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
                rb_canback = true;*/

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

            if (komapos == 1)
            {
                rb_canback = false;
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
                Debug.Log("[Info] (getenemykomaid)\nlkoma[" + cnt + "] != " + groundid + " (in " + lkoma[cnt] + ") / Check:" + notfound);
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
                Debug.Log("[Info] (getenemykomaid)\nrkoma[" + cnt + "] != " + groundid + " (in " + rkoma[cnt] + ") / Check:" + notfound);
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
            todotext.text = "青色ペンギンチームの勝利！おめでとう！\n[R]を押してもう一度プレイできるぞ！";
            if (ground[15] == 0)
            {
                //ギャモン勝ち判定ここから
                if (ground[2] >= 1 || ground[3] >= 1 || ground[4] >= 1)
                {
                    //バックギャモン勝ち
                    todotext.text = "青色ペンギンチームのバックギャモン勝ち！圧倒的戦略に感服です\n[R]を押すともう一度プレイできます。";
                    if (playsound)
                    {
                        se.clip = gyamonwin;
                        se.Play();
                    }
                }
                else
                {
                    todotext.text = "青色ペンギンチームのギャモン勝ち！戦略ゲーは得意なのかな？\n[R]を押してもう一度プレイできるぞ！";
                    if (playsound)
                    {
                        se.clip = gyamonwin;
                        se.Play();
                    }
                }
            }
        }
        else
        {
            todotext.text = "赤色ペンギンチームの勝利！おめでとう！！\n[R]を押してもう一度プレイできるぞ！";
            if (ground[0] == 0)
            {
                //ギャモン勝ち判定ここから
                if (ground[13] <= -1 || ground[12] <= -1 || ground[11] <= -1)
                {
                    todotext.text = "赤色ペンギンチームのバックギャモン勝ち！圧倒的ですね・・・\n[R]を押すともう一度プレイできますよ。";
                    if (playsound)
                    {
                        se.clip = gyamonwin;
                        se.Play();
                    }
                }
                else
                {
                    todotext.text = "赤色ペンギンチームのギャモン勝ち！\n[R]を押してもう一度プレイできるぞ！";
                    if (playsound)
                    {
                        se.clip = gyamonwin;
                        se.Play();
                    }
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
            if (!playedse)
            {
                se.clip = stackse;
                se.Play();
                playedse = true;
            }
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
                        movekoma_apply(true, rb_lid, rb_lp, true, rb_lid);
                        movekoma_apply(false, rb_rid, rb_rp);
                    }
                    else    //通常移動で敵を監獄に飛ばした
                    {
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
                        movekoma_apply(true, rb_lid, rb_lp, true, rb_lid);
                    }
                    else    //通常状態から通常移動
                    {
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
                        movekoma_apply(false, rb_rid, rb_rp, true, rb_rid);
                        movekoma_apply(true, rb_lid, rb_lp);
                    }
                    else    //通常移動で敵を監獄に飛ばした
                    {
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
                        movekoma_apply(false, rb_rid, rb_rp, true, rb_rid);
                    }
                    else
                    {
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
            if ((lkoma[komaid] - 1) < diceti)
            {
                switch (diceti)
                {
                    case 2:
                        if (lkoma[komaid] == 2)
                        {
                            //後方（３マス目）にコマがあるか見る
                            if (ground[3] <= -1 || ground[4] <= -1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        else
                        {
                            ans = true;
                        }
                        break;

                    case 3:
                        if (lkoma[komaid] == 2)
                        {
                            if (ground[3] <= -1 || ground[4] <= -1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        else if (lkoma[komaid] == 3)
                        {
                            if (ground[4] <= -1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        else
                        {
                            ans = true;
                        }
                        break;

                    default:
                        //do nothing
                        ans = true;
                        break;
                }
            }
            else
            {
                ans = true;
            }
        }
        else
        {
            //赤ターン
            if ((15 - rkoma[komaid] - 1) < diceti)
            {
                switch (diceti)
                {
                    case 2:
                        if (rkoma[komaid] == 13)
                        {
                            //後方（３マス目）にコマがあるか見る
                            if (ground[12] >= 1 || ground[11] >= 1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        else
                        {
                            ans = true;
                        }
                        break;

                    case 3:
                        if (rkoma[komaid] == 13)
                        {
                            if (ground[11] >= 1 || ground[12] >= 1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        else if (rkoma[komaid] == 12)
                        {
                            if (ground[11] >= 1)
                            {
                                ans = false;
                            }
                            else
                            {
                                ans = true;
                            }
                        }
                        break;

                    default:
                        //do nothing
                        ans = true;
                        break;
                }
            }
            else
            {
                ans = true;
            }
        }

        if (ans == false)
        {
            todotext.text = "一番後ろのコマを優先で動かしましょう。";
            if (playsound)
            {
                se.clip = cantse;
                se.Play();
            }
        }

        return ans;
    }

    void komapreview(bool isblue, int komaid, int diceti, bool reset = false)
    {
        int posX = -255;
        int idousu = -1;
        int newposX = -1;
        int fieldid = -1;

        if (reset)
        {
            komapreview_obj.GetComponent<Image>().material = null;
            komapreview_obj.GetComponent<Image>().material = null;
        }
        else
        {
            if (isblue)
            {
                //青ターン
                komapreview_obj.GetComponent<Image>().material = hlkoma;
                if (lkoma[komaid] - diceti <= 1)
                {
                    fieldid = 0;
                    if (!(cangoal(isblue, komaid, true)))
                    {
                        komapreview_obj.GetComponent<Image>().material = cantgoal;
                    }
                }
                else
                {
                    fieldid = lkoma[komaid] - diceti;
                    if (ground[fieldid] >= 2)
                    {
                        komapreview_obj.GetComponent<Image>().material = cantgoal;
                    }
                }
            }
            else
            {
                //赤ターン
                komapreview_obj.GetComponent<Image>().material = hrkoma;
                if (rkoma[komaid] + diceti >= 14)
                {
                    fieldid = 15;
                    if (!(cangoal(isblue, komaid, true)))
                    {
                        komapreview_obj.GetComponent<Image>().material = cantgoal;
                    }
                }
                else
                {
                    fieldid = rkoma[komaid] + diceti;
                    if (ground[fieldid] <= -2)
                    {
                        komapreview_obj.GetComponent<Image>().material = cantgoal;
                    }
                }
            }

            idousu = ((fieldid - 1) * 39);
            newposX = posX + idousu;

            switch (komaid)
            {
                case 0:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, -60, 0);
                    break;
                case 1:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, -10, 0);
                    break;
                case 2:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, -35, 0);
                    break;
                case 3:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, 15, 0);
                    break;
                case 4:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, 65, 0);
                    break;
                case 5:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, 40, 0);
                    break;
                case 6:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, 90, 0);
                    break;
                case 7:
                    komapreview_obj.transform.localPosition = new Vector3(newposX, 135, 0);
                    break;
                default:
                    Debug.Log("[Error] (goal) switch overflow!!! [" + isblue + " / " + komaid + "]");
                    break;
            }
        }
        return;
    }

}
