using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pvp_game_load : MonoBehaviour
{
    /*
     [TO DO]
        * 動けるコマがない場合の処理（ターンエンド）
        * 音を入れる
        * クリアイベントの作成
     [BUG]
        * 赤(Right)側の漂流数の数字が表示されないバグ
        * Fキースキップ時にコマが選択色のままになる問題
     [改善点]
        * 先攻ターンがどっちか直感的にわからない
        * Xコン対応、操作方法
        * 戻れると良い
        * ダイス選択とコマ選択が同時に行えるようにする
        * 漂流したコマがある場合はそっちのコマを優先し、ほかのコマを選択できないようにする。
        * 操作説明の位置
        * todoが見にくい
        * ゴールしたコマを
    */

    bool user = false;  //プレイヤー(左側がtrue)
    bool gameready = false; //ready to game
    bool ongame = false;    //playing game
    bool canroll = false;   //ダイスロールの可否
    bool selectkoma = false;    //コマ選択モードか
    int turn = 0;   //経過ターン
    int remain = 0; //残りの移動回数
    int remainmax = 0;  //残り移動回数の最大値
    int[] lkoma;    //左側コマの位置
    int[] rkoma;    //右側コマの位置
    int[] ground;   //フィールドのコマの数
    int roll1 = 0, roll2 = 0;   //ダイスの出た値
    int selected = 0;   //選択中のダイス
    int selected_koma = 0;  //選択中のコマ
    bool activedice1, activedice2, activedice3, activedice4;    //ダイス使用済みか

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
        rkoma0_obj.transform.localPosition = new Vector3(-215, -45, 0);
        rkoma[1] = new int();
        rkoma[1] = 2;
        rkoma1_obj.transform.localPosition = new Vector3(-215, 5, 0);
        lkoma[0] = new int();
        lkoma[0] = 13;
        lkoma0_obj.transform.localPosition = new Vector3(220, -45, 0);
        lkoma[1] = new int();
        lkoma[1] = 13;
        lkoma1_obj.transform.localPosition = new Vector3(220, 5, 0);
        lkoma[2] = new int();
        lkoma[2] = 8;
        lkoma2_obj.transform.localPosition = new Vector3(20, -20, 0);
        rkoma[2] = new int();
        rkoma[2] = 7;
        rkoma2_obj.transform.localPosition = new Vector3(-20, -20, 0);
        rkoma[3] = new int();
        rkoma[3] = 7;
        rkoma3_obj.transform.localPosition = new Vector3(-20, 30, 0);
        rkoma[4] = new int();
        rkoma[4] = 7;
        rkoma4_obj.transform.localPosition = new Vector3(-20, 80, 0);
        lkoma[3] = new int();
        lkoma[3] = 8;
        lkoma3_obj.transform.localPosition = new Vector3(20, 30, 0);
        lkoma[4] = new int();
        lkoma[4] = 8;
        lkoma4_obj.transform.localPosition = new Vector3(20, 80, 0);
        lkoma[5] = new int();
        lkoma[5] = 4;
        lkoma5_obj.transform.localPosition = new Vector3(-140, 55, 0);
        rkoma[5] = new int();
        rkoma[5] = 11;
        rkoma5_obj.transform.localPosition = new Vector3(140, 55, 0);
        rkoma[6] = new int();
        rkoma[6] = 11;
        rkoma6_obj.transform.localPosition = new Vector3(140, 105, 0);
        rkoma[7] = new int();
        rkoma[7] = 11;
        rkoma7_obj.transform.localPosition = new Vector3(140, 150, 0);
        lkoma[6] = new int();
        lkoma[6] = 4;
        lkoma6_obj.transform.localPosition = new Vector3(-140, 105, 0);
        lkoma[7] = new int();
        lkoma[7] = 4;
        lkoma7_obj.transform.localPosition = new Vector3(-140, 150, 0);

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
        todotext.text = "先攻を決めます。\n[D]キーを押してね。";
        turntext.text = "Turn: " + turn.ToString();
        l_goal_label.text = ground[0].ToString() + "/8";
        r_goal_label.text = ground[15].ToString() + "/8";
        l_kick_label.text = "漂流：" + ground[1].ToString();
        r_kick_label.text = "漂流：" + ground[14].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (ongame)
        {
            if (canroll)
            {
                if (Input.GetKeyDown(KeyCode.D))    /* ダイスロール */
                {
                    canroll = false;

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
                    //移動可能コマがある
                    if (selectkoma)
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
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            //コマ選択キャンセル
                            selected_dice_change(selected);
                            selectkoma = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            //コマ移動確定（検証）
                            if (canmovekoma(user, selected_koma, selected))
                            {
                                //移動完了
                                selectkoma = false;
                                remain--;
                                selected_koma = 0;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.F))
                        {
                            //移動不能パス（仮）
                            selected_dice_change(selected);
                            selected_koma = 0;
                            remain = 0;
                            selectkoma = false;
                        }
                    }
                    else
                    {
                        //コマの指定変更ができる
                        if (Input.GetKeyDown(KeyCode.Alpha1))
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
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            if (can_select_dice(selected))  /* 選択中のダイスが選択可能か */
                            {
                                selected_dice_change(selected, true);
                                activekoma_change(user, 0);
                                selectkoma = true;
                            }
                        }
                    }
                }
                else
                {
                    /* プレイヤー交代 */
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
                }
                remaintext.text = "残ダイス数:" + remain.ToString();

                //勝利しましたか？
                if (ground[0] >= 8 || ground[15] >= 8) /* 勝利判定 */
                {
                    if (ground[0] >= 8)
                    {
                        //Left側の勝利
                        /* 勝利イベントここ */
                        gamewin(true);
                        return;
                    }
                    else
                    {
                        //Right側の勝利
                        /* 勝利イベントここ */
                        gamewin(false);
                        return;
                    }
                }
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
                todotext.text += "\n[D]キーを押してダイスを振りましょう。";
                canroll = true;
                ongame = true;
                gameready = false;
            }
        }
    }

    int diceroll()  /* ダイスを振る */
    {
        int diceval;
        diceval = Random.Range(0, 3);
        return diceval;
    }

    void diceapply(int dice1, int dice2, bool zoro = false) /* ダイスの画像を反映 */
    {
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
        if (isblue)  //青ターン
        {
            if (komaid != -1)   //-1だと全部のコマの色を初期化する
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
                    lkoma0_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma1_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma2_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma3_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma4_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma5_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma6_obj.GetComponent<Image>().material = lkoma_notselected;
                    lkoma7_obj.GetComponent<Image>().material = lkoma_notselected;
                    break;
            }
        }
        else
        {
            if (komaid != -1)
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
                    rkoma0_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma1_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma2_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma3_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma4_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma5_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma6_obj.GetComponent<Image>().material = rkoma_notselected;
                    rkoma7_obj.GetComponent<Image>().material = rkoma_notselected;
                    break;
            }
        }
        return;
    }

    bool canmovekoma(bool isblue, int komaid, int diceselected) /* コマの移動可否検証及び実行 */
    {
        bool canmove = false;
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
                    return canmove;
                }
            }
            Debug.Log("右コマ(LKoma)ID:" + komaid + " は、現在ground[" + komapos + "]に位置しています。\nまた、監獄がなければ、ground[" + (komapos - move) + "]へ移動します。");
            if (komapos - move <= 1)    //移動先が監獄だった場合、ゴールへ
            {
                move = (komapos - 0);
                Debug.Log("移動先がゴールです。");
                if (cangoal(isblue, komaid))
                {
                    canmove = true;
                    goal(isblue, komaid);
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    return canmove;
                }
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
                    Debug.Log("味方のコマと合流します。");
                    canmove = true;
                }
            }
            else    //移動先にコマがない場合
            {
                Debug.Log("新規領域を占領します。");
                canmove = true;
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
                    return canmove;
                }
            }
            Debug.Log("右側 コマID:" + komaid + " は、現在 " + komapos + "に位置しています。\nまた、監獄がなければ、ground[" + (komapos + move) + "]へ移動します。");
            if (komapos + move >= 14)    //移動先が監獄だった場合、ゴールへ
            {
                move = Mathf.Abs(komapos - 15);
                Debug.Log("移動先がゴールです。");
                if (cangoal(isblue, komaid))
                {
                    canmove = true;
                    goal(isblue, komaid);
                }
                else
                {
                    Debug.Log("コマが揃っていないためゴールできません。");
                    return canmove;
                }
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
                    Debug.Log("味方と合流します。");
                    canmove = true;
                }
            }
            else    //移動先にコマがない場合
            {
                Debug.Log("新規領域を占領します。");
                canmove = true;
            }
        }
        if (canmove)
        {
            movekoma(isblue, komaid, komapos, move, hasenemy);
        }
        else
        {
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
        }
        return canmove;
    }

    void movekoma(bool isblue, int komaid, int komapos, int move, bool hasenemy) /* コマを移動 */
    {
        //ダイス移動処理ここから
        Debug.Log("[Function Join] movekoma\n左側？:" + isblue + " / コマID:" + komaid + " / コマの座標:" + komapos + " / 移動コマ数:" + move + " / 敵がいるか？:" + hasenemy);
        if (isblue)
        {
            //左側プレイヤー
            if (hasenemy)
            {
                //移動先マスに敵が1体いる場合
                ground[1]++;    //監獄に加算
                int enemy = getenemykomaid(komapos - move); //飛ばされる敵のコマを取得
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
                //移動先マスに敵が1コマいる場合
                ground[14]--;    //監獄に加算
                int enemy = getenemykomaid(komapos + move); //飛ばされる敵のコマを取得
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
        activekoma_change(user, -1);
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
            Debug.Log("[Error] (getenemykomaid) That komaID Not Found!!!");
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
        int AddposX = 20 * komaid;

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
                        lkoma0_obj.transform.localPosition = new Vector3(newposX, -45, 0);
                    break;
                case 1:
                    if (goprison)
                        lkoma1_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma1_obj.transform.localPosition = new Vector3(newposX, 5, 0);
                    break;
                case 2:
                    if (goprison)
                        lkoma2_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma2_obj.transform.localPosition = new Vector3(newposX, -20, 0);
                    break;
                case 3:
                    if (goprison)
                        lkoma3_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma3_obj.transform.localPosition = new Vector3(newposX, 30, 0);
                    break;
                case 4:
                    if (goprison)
                        lkoma4_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma4_obj.transform.localPosition = new Vector3(newposX, 80, 0);
                    break;
                case 5:
                    if (goprison)
                        lkoma5_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma5_obj.transform.localPosition = new Vector3(newposX, 55, 0);
                    break;
                case 6:
                    if (goprison)
                        lkoma6_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma6_obj.transform.localPosition = new Vector3(newposX, 105, 0);
                    break;
                case 7:
                    if (goprison)
                        lkoma7_obj.transform.localPosition = new Vector3(newposX - AddposX, -155, 0);
                    else
                        lkoma7_obj.transform.localPosition = new Vector3(newposX, 150, 0);
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
                        rkoma0_obj.transform.localPosition = new Vector3(newposX, -45, 0);
                    break;
                case 1:
                    if (goprison)
                        rkoma1_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma1_obj.transform.localPosition = new Vector3(newposX, 5, 0);
                    break;
                case 2:
                    if (goprison)
                        rkoma2_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma2_obj.transform.localPosition = new Vector3(newposX, -20, 0);
                    break;
                case 3:
                    if (goprison)
                        rkoma3_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma3_obj.transform.localPosition = new Vector3(newposX, 30, 0);
                    break;
                case 4:
                    if (goprison)
                        rkoma4_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma4_obj.transform.localPosition = new Vector3(newposX, 80, 0);
                    break;
                case 5:
                    if (goprison)
                        rkoma5_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma5_obj.transform.localPosition = new Vector3(newposX, 55, 0);
                    break;
                case 6:
                    if (goprison)
                        rkoma6_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma6_obj.transform.localPosition = new Vector3(newposX, 105, 0);
                    break;
                case 7:
                    if (goprison)
                        rkoma7_obj.transform.localPosition = new Vector3(newposX + AddposX, -155, 0);
                    else
                        rkoma7_obj.transform.localPosition = new Vector3(newposX, 150, 0);
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
        Debug.Log("[Function Join] goal\nisblue:" + isblue + " / KomaID:" + komaid);
        if (isblue)
        {
            switch (komaid)
            {
                case 0:
                    lkoma0_obj.SetActive(false);
                    break;
                case 1:
                    lkoma1_obj.SetActive(false);
                    break;
                case 2:
                    lkoma2_obj.SetActive(false);
                    break;
                case 3:
                    lkoma3_obj.SetActive(false);
                    break;
                case 4:
                    lkoma4_obj.SetActive(false);
                    break;
                case 5:
                    lkoma5_obj.SetActive(false);
                    break;
                case 6:
                    lkoma6_obj.SetActive(false);
                    break;
                case 7:
                    lkoma7_obj.SetActive(false);
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
                    rkoma0_obj.SetActive(false);
                    break;
                case 1:
                    rkoma1_obj.SetActive(false);
                    break;
                case 2:
                    rkoma2_obj.SetActive(false);
                    break;
                case 3:
                    rkoma3_obj.SetActive(false);
                    break;
                case 4:
                    rkoma4_obj.SetActive(false);
                    break;
                case 5:
                    rkoma5_obj.SetActive(false);
                    break;
                case 6:
                    rkoma6_obj.SetActive(false);
                    break;
                case 7:
                    rkoma7_obj.SetActive(false);
                    break;
                default:
                    Debug.Log("[Error] (goal) switch overflow!!! [" + isblue + " / " + komaid + "]");
                    break;
            }
        }
    }

    void textupdate()
    {
        l_goal_label.text = Mathf.Abs(ground[0]).ToString() + "/8";
        r_goal_label.text = ground[15].ToString() + "/8";
        l_kick_label.text = "漂流: " + ground[1].ToString();
        r_kick_label.text = "漂流: " + Mathf.Abs(ground[14]).ToString();
        turntext.text = "Turn: " + turn.ToString();
        return;
    }

    bool cangoal(bool isblue, int komaid)
    {
        bool ans = false;
        if (isblue)
        {
            //青
            for (int i = 0; i < 8; i++)
            {
                if(i == komaid)
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
                Debug.Log("[Function] (cangoal) lkoma[" + i + "]は座標" + lkoma[i] + "に位置し、判定結果は" + ans + "です。");
            }
        }
        else
        {
            //赤
            for (int i = 0; i < 8; i++)
            {
                if(i == komaid)
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
                Debug.Log("[Function] (cangoal) rkoma[" + i + "]は座標" + rkoma[i] + "に位置し、判定結果は" + ans + "です。");
            }
        }
        return ans;
    }

    void gamewin(bool isblue)
    {
        if (isblue)
        {
            todotext.text = "青色ペンギンさんチームの勝利！\nおめでとう！！";
            gameready = false;
            ongame = false;
        }
        else
        {
            todotext.text = "赤色ペンギンさんチームの勝利！\nおめでとう！！";
            gameready = false;
            ongame = false;
        }
        return;
    }

}
