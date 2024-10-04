using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance; // �V���O���g����
    [SerializeField] private FruitManager fruitManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameOverLine gameOverLine;
    [SerializeField] private Score score;
    [SerializeField] private Ranking ranking;
    [SerializeField] private GetDataBaseScore getDataBaseScore;
    [SerializeField] private UploadScore uploadScore;
    [SerializeField] private Text userGuideText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        KeyConfig kc_ins = KeyConfig.instance;

        userGuideText.text = kc_ins.leftMoveKeyText     + ":���ړ��@" + 
                             kc_ins.rightMoveKeyText    + ":�E�ړ��@" + 
                             kc_ins.pauseKeyText        + ":�|�[�Y�@" + 
                             kc_ins.fruitPutKeyText     + ":���Ƃ��@" + 
                             kc_ins.rankingResetKeyText + ":�����L���O���Z�b�g";

        // �f�[�^�x�[�X�̃X�R�A��ǂݍ���Ń����L���O�ɃZ�b�g����
        StartCoroutine(getDataBaseScore.init(ranking));

        // �ŏ��ɐ�������t���[�c�����߂�
        fruitManager.CurrentFruit(fruitManager.RandomGenerateFruitIndex());

        // �l�N�X�g�̃t���[�c��ݒ肷��
        fruitManager.NextFruit(fruitManager.RandomGenerateFruitIndex());
    }

    private void Update()
    {
        // �Q�[���̏�Ԃ̔���
        if (!gameOverLine.GetGameOverFlg())
        {
            PlayGame();
        }
        else
        {
            GameOver();
        }
    }

    private void PlayGame()
    {
        // �|�[�Y��ʁA�����L���O���Z�b�g��ʂ��J����Ă��Ȃ��ꍇ
        if (!uiManager.GetPauseActiveSelf() && !uiManager.GetRankingResetPanelActiveSelf())
        {
            // �|�[�Y��ʂ��J��
            if (Input.GetKeyDown(KeyConfig.instance.pauseKey))
            {
                uiManager.Pause(true);
            }

            // �G�X�P�[�v�L�[�Ń����L���O�����Z�b�g
            if (Input.GetKeyDown(KeyConfig.instance.rankingResetKey))
            {
                uiManager.ConfirmRankingReset(true);
            }

            // �t���[�c�𐶐�����|�W�V���������߂�
            fruitManager.MoveGeneratePos();

            // �t���[�c�𗎂Ƃ�
            fruitManager.PutFruit();

            // �����L���O���X�V����
            ranking.UpdateRanking(score.GetScore());
        }
        else
        {
            // �|�[�Y��ʂ����
            if (Input.GetKeyDown(KeyConfig.instance.pauseKey))
            {
                uiManager.Pause(false);
            }

            // �G�X�P�[�v�L�[�Ń����L���O�����Z�b�g
            if (Input.GetKeyDown(KeyConfig.instance.rankingResetKey))
            {
                uiManager.ConfirmRankingReset(false);
            }
        }
    }

    private void GameOver()
    {
        if (!uiManager.GetGameOverDisplayFlg())
        {
            Time.timeScale = 0;
            uiManager.GameOverDisplay();

            // �����L���O�𔽉f����
            ranking.SaveScore(score.GetScore());

            // �����L���O���f�[�^�x�[�X�ɑ���
            uploadScore.UploadScores(ranking.GetRankingScore());
        }
    }

    // �����L���O�����Z�b�g����
    public void ResetRanking()
    {
        // �����L���O�����Z�b�g
        ranking.InitRanking();

        // �����L���O���Z�b�g�̊m�F��ʂ����
        uiManager.ConfirmRankingReset(false);
    }

    public Score GetScoreClass()
    {
        return score;
    }
}
