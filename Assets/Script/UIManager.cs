using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject rankingResetPanel;

    // �Q�[���I�[�o�[��ʂ�\��������
    public void GameOverDisplay()
    {
        gameOver.SetActive(true);
    }

    // �Q�[���I�[�o�[��ʂ��\������Ă��邩�ǂ���
    public bool GetGameOverDisplayFlg()
    {
        return gameOver.activeSelf;
    }

    // �|�[�Y���
    public void Pause(bool isOpen)
    {
        SoundManager.instance.PlayButtonSE();
        if (isOpen)
        {
            pause.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // �����L���O���Z�b�g���
    public void ConfirmRankingReset(bool isOpen)
    {
        if (isOpen)
        {
            rankingResetPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            rankingResetPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // �|�[�Y��ʂ��\������Ă��邩�𒲂ׂ�
    public bool GetPauseActiveSelf()
    {
        return pause.activeSelf;
    }

    // �����L���O��ʂ��\������Ă��邩�𒲂ׂ�
    public bool GetRankingResetPanelActiveSelf()
    {
        return rankingResetPanel.activeSelf;
    }

    // �^�C�g���֖߂�
    public void BackToTitle()
    {
        SoundManager.instance.PlayButtonSE();
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }

    // ���X�^�[�g
    public void Retry()
    {
        SoundManager.instance.PlayButtonSE();
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
}
