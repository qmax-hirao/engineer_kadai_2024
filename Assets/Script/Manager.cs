using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance; // シングルトン化
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

        userGuideText.text = kc_ins.leftMoveKeyText     + ":左移動　" + 
                             kc_ins.rightMoveKeyText    + ":右移動　" + 
                             kc_ins.pauseKeyText        + ":ポーズ　" + 
                             kc_ins.fruitPutKeyText     + ":落とす　" + 
                             kc_ins.rankingResetKeyText + ":ランキングリセット";

        // データベースのスコアを読み込んでランキングにセットする
        StartCoroutine(getDataBaseScore.init(ranking));

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
            if (Input.GetKeyDown(KeyConfig.instance.pauseKey))
            {
                uiManager.Pause(true);
            }

            // エスケープキーでランキングをリセット
            if (Input.GetKeyDown(KeyConfig.instance.rankingResetKey))
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
            if (Input.GetKeyDown(KeyConfig.instance.pauseKey))
            {
                uiManager.Pause(false);
            }

            // エスケープキーでランキングをリセット
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

            // ランキングを反映する
            ranking.SaveScore(score.GetScore());

            // ランキングをデータベースに送る
            uploadScore.UploadScores(ranking.GetRankingScore());
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
