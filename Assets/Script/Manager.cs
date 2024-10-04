using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance; // シングルトン化
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
        // セーブデータの有無をチェック(0:データなし　1:データあり)
        if (PlayerPrefs.GetInt("SaveKey") == 0)
        {
            // ランキングを初期化
            ranking.InitRanking();
        }
        else
        {
            // セーブされたランキングをセットする
            ranking.SetRanking();
        }

        // 最初に生成するフルーツを決める
        fruitManager.CurrentFruit(fruitManager.RandomGenerateFruitIndex());

        // ネクストのフルーツを設定する
        fruitManager.NextFruit(fruitManager.RandomGenerateFruitIndex());
    }

    private void Update()
    {
        // ゲームの状態の判定
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
        // ポーズ画面、ランキングリセット画面が開かれていない場合
        if (!uiManager.GetPauseActiveSelf() && !uiManager.GetRankingResetPanelActiveSelf())
        {
            // ポーズ画面を開く
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiManager.Pause(true);
            }

            // エスケープキーでランキングをリセット
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiManager.ConfirmRankingReset(true);
            }

            // フルーツを生成するポジションを決める
            fruitManager.MoveGeneratePos();

            // フルーツを落とす
            fruitManager.PutFruit();

            // ランキングを更新する
            ranking.UpdateRanking(score.GetScore());
        }
        else
        {
            // ポーズ画面を閉じる
            if (Input.GetKeyDown(KeyCode.X))
            {
                uiManager.Pause(false);
            }

            // エスケープキーでランキングをリセット
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

            // ランキングをセーブする
            ranking.SaveScore(score.GetScore());
        }
    }

    // ランキングをリセットする
    public void ResetRanking()
    {
        // ランキングをリセット
        ranking.InitRanking();

        // ランキングリセットの確認画面を閉じる
        uiManager.ConfirmRankingReset(false);
    }

    public Score GetScoreClass()
    {
        return score;
    }
}
