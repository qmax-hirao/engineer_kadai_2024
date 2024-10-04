using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ランキングを管理する
public class Ranking : MonoBehaviour
{
    private const int rankNumber = 3; // ランキングに表示させる数
    private int[] rankingScore;       // ランキングごとのスコア([0]→1位 [1]→2位 [2]→3位)
    [SerializeField] private Text[] rankingScoreText; // ランキングごとのスコアのテキスト

    private void Awake()
    {
        rankingScore = new int[rankNumber];
    }

    // ランキングの初期化
    public void InitRanking()
    {
        // ランキングのスコアにすべて0を代入する
        for (int i = 0; i < rankNumber; i++)
        {
            rankingScore[i] = 0;
            rankingScoreText[i].text = rankingScore[i].ToString();

            //PlayerPrefs.SetInt("RankingScore" + i.ToString(), rankingScore[i]);
        }
    }

    // 保存されているランキングのスコアを取得し設定する
    public void SetRanking(int[] score)
    {
        for (int i = 0; i < rankNumber; i++)
        {
            rankingScore[i] = score[i];
            rankingScoreText[i].text = rankingScore[i].ToString();
        }
    }

    // ランキングのスコアを更新していたらリアルタイムで反映させる
    public void UpdateRanking(int score)
    {
        for (int i = 0; i < rankNumber; i++)
        {
            if (score > rankingScore[i])
            {
                rankingScoreText[i].text = score.ToString();
                score = rankingScore[i];
            }
        }
    }

    // スコアをセーブする
    public void SaveScore(int score)
    {
        for (int i = 0; i < rankNumber; i++)
        {
            if (score > rankingScore[i])
            {
                int temp = rankingScore[i];
                rankingScore[i] = score;
                score = temp;
            }
        }
    }

    public int GetRankNumber()
    {
        return rankNumber;
    }

    public int[] GetRankingScore()
    {
        return rankingScore;
    }
}