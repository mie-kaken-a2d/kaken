#pragma once

#include "ofMain.h"
#include "ofxAssimpModelLoader.h"

class testApp : public ofBaseApp{
	private:

		/* �ϐ��錾 */
		const string appname;		//�A�v���P�[�V������
		int ground[15];				//�t�B�[���h�ɂ������̏��B�����Ȃ獶��(Left Player)�A�����Ȃ�E��(Right Player)�B
		/*
			�y�⑫�z
			���Ƃ��΁ALeft Player��ground[2]��3�̂���Ȃ�΁A"ground[2] = -3" �ɂȂ�A
			RightPlayer��ground[6]��1�̂Ȃ�΁A"ground[6] = 1" �ƂȂ�B

			�y�l�����\�z
			ground[0] = �E���S�[�� (Left Cassle)
			      [1] = �E�������� (Right���̃R�}���ċ�)
			      [2] = �t�B�[���h1 (Left���w�n@1)
				  [3] = �t�B�[���h2 (Left���w�n@2)
				  [4] = �t�B�[���h3 (Left���w�n@3)
				  [5] = �t�B�[���h4 (Mid�w�nLeft��@1)
				  [6] = �t�B�[���h5 (Mid�w�nLeft��@2)
				  [7] = �t�B�[���h6 (Mid�w�nLeft��@3)
							<--- ���� --->
				  [8] = �t�B�[���h7 (Mid�w�nRight��@3)
				  [9] = �t�B�[���h8 (Mid�w�nRight��@2)
				  [10] = �t�B�[���h9 (Mid�w�nRight��@1)
				  [11] = �t�B�[���h10 (Right���w�n@3)
				  [12] = �t�B�[���h11 (Right���w�n@2)
				  [13] = �t�B�[���h12 (Right���w�n@1)
				  [14] = ���������� (Left���̃R�}���ċ�)
				  [15] = �����S�[�� (Right Cassle)
		*/
		int lkoma[7], rkoma[7];		//��Ƃ�ground[]�̈ʒu
		int lgoal, rgoal;			//�e�w�c���Ƃ̃S�[�����B8�ɂȂ�����Q�[�������B
		int ldice, rdice;			//�e�w�c���Ƃ̃_�C�X�i�[�B�c�ړ����B
		int turncnt;				//�^�[�����J�E���^�B
		bool ongame;				//�Q�[�����s�����̔���Bfalse�Ȃ�Q�[���͎n�܂��Ă��Ȃ��B
		bool isplayer;				//�G���l�Ԃ��BPvP�Ȃ�true�B
		/*bool ismulti;				//�}���`�v���C���B�}���`�v���C�Ȃ�Aisplayer��true�ɂȂ�B*/
		bool turn;					//true�Ȃ�Left Player�̃^�[���Bfalse�Ȃ�Right Player�B


		/* ���f���錾 */
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
