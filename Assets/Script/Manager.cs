using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance; // �V���O���g����
    [SerializeField] private FruitManager fruitManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameOverLine gameOverLine;
    [SerializeField] private Score score;
    [SerializeField] private Ranking ranking;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // �Z�[�u�f�[�^�̗L�����`�F�b�N(0:�f�[�^�Ȃ��@1:�f�[�^����)
        if (PlayerPrefs.GetInt("SaveKey") == 0)
        {
            // �����L���O��������
            ranking.InitRanking();
        }
        else
        {
            // �Z�[�u���ꂽ�����L���O���Z�b�g����
            ranking.SetRanking();
        }

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
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiManager.Pause(true);
            }

            // �G�X�P�[�v�L�[�Ń����L���O�����Z�b�g
            if (Input.GetKeyDown(KeyCode.Escape))
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
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiManager.Pause(false);
            }

            // �G�X�P�[�v�L�[�Ń����L���O�����Z�b�g
            if (Input.GetKeyDown(KeyCode.Escape))
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

            // �����L���O���Z�[�u����
            ranking.SaveScore(score.GetScore());
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
