using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject rankingResetPanel;

    // ゲームオーバー画面を表示させる
    public void GameOverDisplay()
    {
        gameOver.SetActive(true);
    }

    // ゲームオーバー画面が表示されているかどうか
    public bool GetGameOverDisplayFlg()
    {
        return gameOver.activeSelf;
    }

    // ポーズ画面
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

    // ランキングリセット画面
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

    // ポーズ画面が表示されているかを調べる
    public bool GetPauseActiveSelf()
    {
        return pause.activeSelf;
    }

    // ランキング画面が表示されているかを調べる
    public bool GetRankingResetPanelActiveSelf()
    {
        return rankingResetPanel.activeSelf;
    }

    // タイトルへ戻る
    public void BackToTitle()
    {
        SoundManager.instance.PlayButtonSE();
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }

    // リスタート
    public void Retry()
    {
        SoundManager.instance.PlayButtonSE();
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
}
