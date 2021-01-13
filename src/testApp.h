#pragma once

#include "ofMain.h"
#include "ofxAssimpModelLoader.h"

class testApp : public ofBaseApp{
	private:

		/* 変数宣言 */
		const string appname;		//アプリケーション名
		int ground[15];				//フィールドにおける駒の情報。負数なら左側(Left Player)、正数なら右側(Right Player)。
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
							<--- 中央 --->
				  [8] = フィールド7 (Mid陣地Right側@3)
				  [9] = フィールド8 (Mid陣地Right側@2)
				  [10] = フィールド9 (Mid陣地Right側@1)
				  [11] = フィールド10 (Right側陣地@3)
				  [12] = フィールド11 (Right側陣地@2)
				  [13] = フィールド12 (Right側陣地@1)
				  [14] = 左側島流し (Left側のコマが監禁)
				  [15] = 左側ゴール (Right Cassle)
		*/
		int lkoma[7], rkoma[7];		//駒ごとのground[]の位置
		int lgoal, rgoal;			//各陣営ごとのゴール数。8になったらゲーム勝利。
		int ldice, rdice;			//各陣営ごとのダイス格納。残移動数。
		int turncnt;				//ターン数カウンタ。
		bool ongame;				//ゲーム実行中かの判定。falseならゲームは始まっていない。
		bool isplayer;				//敵が人間か。PvPならtrue。
		/*bool ismulti;				//マルチプレイか。マルチプレイなら、isplayerはtrueになる。*/
		bool turn;					//trueならLeft Playerのターン。falseならRight Player。


		/* モデル宣言 */
		ofxAssimpModelLoader m_rcassle, m_lcassle;
		ofxAssimpModelLoader m_rightgr[2], m_midgr[6], m_leftgr[2];
		ofxAssimpModelLoader m_rkoma[7], m_lkoma[7];

	public:
		void setup();
		void update();
		void draw();

		void keyPressed(int key);
		void keyReleased(int key);
		void mouseMoved(int x, int y );
		void mouseDragged(int x, int y, int button);
		void mousePressed(int x, int y, int button);
		void mouseReleased(int x, int y, int button);
		void windowResized(int w, int h);
		void dragEvent(ofDragInfo dragInfo);
		void gotMessage(ofMessage msg);
		
};
