﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pvp_game_load : MonoBehaviour
{
    bool user = false;  //プレイヤー(左側がtrue)
    bool gameready = false; //ready to game
    bool ongame = false;    //playing game
    bool canroll = false;   //ダイスロールの可否
    bool dup = false;       //ゾロ目か
    int turn = 0;   //経過ターン
    int remain = 0; //残りの移動回数
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

        lkoma[1] = new int();
        lkoma[1] = 2;
        rkoma[0] = new int();
        rkoma[0] = 4;
        rkoma[1] = new int();
        rkoma[1] = 4;
        rkoma[2] = new int();
        rkoma[2] = 4;
        lkoma[2] = new int();
        lkoma[2] = 7;
        lkoma[3] = new int();
        lkoma[3] = 7;
        lkoma[4] = new int();
        lkoma[4] = 7;
        rkoma[3] = new int();
        rkoma[3] = 8;
        rkoma[4] = new int();
        rkoma[4] = 8;
        rkoma[5] = new int();
        rkoma[5] = 8;
        lkoma[5] = new int();
        lkoma[5] = 11;
        lkoma[6] = new int();
        lkoma[6] = 11;
        lkoma[7] = new int();
        lkoma[7] = 11;
        rkoma[6] = new int();
        rkoma[6] = 13;
        rkoma[7] = new int();
        rkoma[7] = 13;

        /* Ready! */
        diceview1_obj.GetComponent<Image>().material = null;
        diceview2_obj.GetComponent<Image>().material = null;
        diceview3_obj.GetComponent<Image>().material = null;
        diceview4_obj.GetComponent<Image>().material = null;
        turn = 0;   //ターン数初期化
        remain = 0; //残りダイス数初期化
        roll1 = 0;  //ダイス初期化
        roll2 = 0;  //ダイス初期化
        activedice1 = false;
        activedice2 = false;
        activedice3 = false;
        activedice4 = false;
        user = false;
        ongame = false;
        canroll = false;
        gameready = true;
        todotext.text = "先攻を決めます。\n[D]キーを押してね。";
    }

    // Update is called once per frame
    void Update()
    {
        if (ongame)
        {
            if (canroll)
            {
                //ダイスロールフェーズ
                if (Input.GetKeyDown(KeyCode.D))
                {
                    canroll = false;

                    turn++;
                    turntext.text = "Turn: " + turn.ToString();

                    todotext.text = "ダイスを振っています…";
                    roll1 = diceroll();
                    roll2 = diceroll();
                    todotext.text = "ダイスを振りました。";

                    if (user)
                    {
                        turnuser.transform.localPosition = new Vector3(330, 160, 0);
                    }
                    else
                    {
                        turnuser.transform.localPosition = new Vector3(-330, 160, 0);
                    }

                    if (roll1 == roll2)
                    {
                        todotext.text += "[ゾロ目]";
                        remain = 4;
                        dup = true;
                        diceapply(roll1, roll2, true);
                    }
                    else
                    {
                        remain = 2;
                        dup = false;
                        diceapply(roll1, roll2);
                    }
                }
            }
            else
            {
                //コマ移動フェーズ
                if(remain > 0)
                {
                    //移動可能なコマがある場合

                }
                else
                {
                    //移動フェーズが終わった場合
                    user = !user;
                    canroll = true;
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
                    switch (roll1)
                    {
                        case 0:
                            diceview1_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview1_obj.GetComponent<Image>().material = di2;
                            break;
                        default:
                            diceview1_obj.GetComponent<Image>().material = di3;
                            break;
                    }

                    roll2 = diceroll();
                    switch (roll2)
                    {
                        case 0:
                            diceview2_obj.GetComponent<Image>().material = di1;
                            break;
                        case 1:
                            diceview2_obj.GetComponent<Image>().material = di2;
                            break;
                        default:
                            diceview2_obj.GetComponent<Image>().material = di3;
                            break;
                    }
                } while (roll1 == roll2);

                if (roll1 > roll2)
                {
                    user = true;
                    turnuser.transform.localPosition = new Vector3(-330, 160, 0);
                    todotext.text = "←　先攻が決まりました！";
                }
                else
                {
                    user = false;
                    turnuser.transform.localPosition = new Vector3(330, 160, 0);
                    todotext.text = "先攻が決まりました！　→";
                }
                todotext.text += "\n[D]キーを押してダイスを振りましょう。[" + roll1.ToString() + "/" + roll2.ToString() + "]";
                canroll = true;
                ongame = true;
                gameready = false;
            }
        }
    }

    int diceroll()
    {
        int diceval;
        diceval = Random.Range(0, 3);
        return diceval;
    }

    void diceapply(int dice1, int dice2, bool zoro = false)
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
                default:
                    diceview1_obj.GetComponent<Image>().material = di3;
                    diceview2_obj.GetComponent<Image>().material = di3;
                    diceview3_obj.GetComponent<Image>().material = di3;
                    diceview4_obj.GetComponent<Image>().material = di3;
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
                default:
                    diceview1_obj.GetComponent<Image>().material = di3;
                    break;
            }
            switch (dice2)
            {
                case 0:
                    diceview2_obj.GetComponent<Image>().material = di1;
                    break;
                case 1:
                    diceview2_obj.GetComponent<Image>().material = di2;
                    break;
                default:
                    diceview2_obj.GetComponent<Image>().material = di3;
                    break;
            }
            diceview3_obj.GetComponent<Image>().material = null;
            diceview4_obj.GetComponent<Image>().material = null;
            activedice1 = true;
            activedice2 = true;
            activedice3 = false;
            activedice4 = false;
        }
        return;
    }

}
