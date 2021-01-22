using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pvp_game_load : MonoBehaviour
{
    bool user = false;  //プレイヤー(左側がtrue)
    bool gameready = false; //ready to game
    bool ongame = false;    //playing game
    bool canroll = false;   //ダイスロールの可否
    bool selectkoma = false;    //コマ選択モードか
    int turn = 0;   //経過ターン
    int remain = 0; //残りの移動回数
    int remainmax = 0;  //残り移動回数の最大値
    const int komakazu = 7; //コマの数
    int[] lkoma;    //左側コマの位置
    int[] rkoma;    //右側コマの位置
    int[] ground;   //フィールドのコマの数
    int roll1 = 0, roll2 = 0;   //ダイスの出た値
    int selected = 0;   //選択中のダイス
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
        lkoma[0] = new int();
        lkoma[0] = 2;
        lkoma0_obj.transform.localPosition = new Vector3(-275, -45, 0);
        lkoma[1] = new int();
        lkoma[1] = 2;
        lkoma1_obj.transform.localPosition = new Vector3(-275, 5, 0);
        rkoma[0] = new int();
        rkoma[0] = 4;
        rkoma0_obj.transform.localPosition = new Vector3(275, -45, 0);
        rkoma[1] = new int();
        rkoma[1] = 4;
        rkoma1_obj.transform.localPosition = new Vector3(275, 5, 0);
        rkoma[2] = new int();
        rkoma[2] = 4;
        rkoma2_obj.transform.localPosition = new Vector3(25, -20, 0);
        lkoma[2] = new int();
        lkoma[2] = 7;
        lkoma2_obj.transform.localPosition = new Vector3(-25, -20, 0);
        lkoma[3] = new int();
        lkoma[3] = 7;
        lkoma3_obj.transform.localPosition = new Vector3(-25, 30, 0);
        lkoma[4] = new int();
        lkoma[4] = 7;
        lkoma4_obj.transform.localPosition = new Vector3(-25, 80, 0);
        rkoma[3] = new int();
        rkoma[3] = 8;
        rkoma3_obj.transform.localPosition = new Vector3(25, 30, 0);
        rkoma[4] = new int();
        rkoma[4] = 8;
        rkoma4_obj.transform.localPosition = new Vector3(25, 80, 0);
        rkoma[5] = new int();
        rkoma[5] = 8;
        rkoma5_obj.transform.localPosition = new Vector3(-175, 55, 0);
        lkoma[5] = new int();
        lkoma[5] = 11;
        lkoma5_obj.transform.localPosition = new Vector3(175, 55, 0);
        lkoma[6] = new int();
        lkoma[6] = 11;
        lkoma6_obj.transform.localPosition = new Vector3(175, 105, 0);
        lkoma[7] = new int();
        lkoma[7] = 11;
        lkoma7_obj.transform.localPosition = new Vector3(175, 55, 0);
        rkoma[6] = new int();
        rkoma[6] = 13;
        rkoma6_obj.transform.localPosition = new Vector3(-175, 105, 0);
        rkoma[7] = new int();
        rkoma[7] = 13;
        rkoma7_obj.transform.localPosition = new Vector3(-175, 55, 0);

        /* Ready! */
        diceview1_obj.GetComponent<Image>().material = null;
        diceview2_obj.GetComponent<Image>().material = null;
        diceview3_obj.GetComponent<Image>().material = null;
        diceview4_obj.GetComponent<Image>().material = null;
        turn = 0;   //ターン数初期化
        remain = 0; //残りダイス数初期化
        roll1 = 0;  //ダイス初期化
        roll2 = 0;  //ダイス初期化
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
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            selected_dice_change(selected);
                            selectkoma = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            //コマの移動ここ//
                            //komamove(selectedkoma, selected)
                            activedice_change(selected);
                            selectkoma = false;
                            remain--;
                            selected_dice_change(selected);
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
                                selectkoma = true;
                            }
                        }
                    }
                }
                else
                {
                    //移動フェーズが終わった場合
                    if (ground[0] == 8 || ground[15] == 8) /* 勝利判定 */
                    {
                        if (ground[0] == 8)
                        {
                            //Left側の勝利
                            /* 勝利イベントここ */
                            todotext.text = "青色の勝利！おめでとう！";
                            return;
                        }
                        else
                        {
                            //Right側の勝利
                            /* 勝利イベントここ */
                            todotext.text = "赤色の勝利！おめでとう！";
                            return;
                        }
                    }
                    /* プレイヤー交代 */
                    user = !user;
                    if (user)
                    {
                        turnuser.transform.localPosition = new Vector3(350, 0, 0);
                    }
                    else
                    {
                        turnuser.transform.localPosition = new Vector3(-345, 0, 0);
                    }
                    todotext.text = "ターンチェンジ。\n[D]キーを押してダイスを振りましょう。";
                    canroll = true;
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
                    turnuser.transform.localPosition = new Vector3(-345, 0, 0);
                    todotext.text = "←　先攻が決まりました！";
                }
                else
                {
                    user = false;
                    turnuser.transform.localPosition = new Vector3(350, 0, 0);
                    todotext.text = "先攻が決まりました！　→";
                }
                todotext.text += "\n[D]キーを押してダイスを振りましょう。";
                canroll = true;
                ongame = true;
                gameready = false;
            }
        }
    }

    int diceroll()  /* do dice rolling */
    {
        int diceval;
        diceval = Random.Range(0, 3);
        return diceval;
    }

    void diceapply(int dice1, int dice2, bool zoro = false) /* dice image update */
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

    void selected_dice_change(int value, bool select = false)
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

    bool can_select_dice(int value)
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

    void activedice_change(int value)
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
}
