using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// スコアの計算や表示の処理を管理する
public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;   // スコアのテキスト
    [SerializeField] private int[] scorePointArray; // 果物が進化した時の点数の配列
    private int score;

    private void Start()
    {
        scoreText.text = "0";
    }

    // スコアを表示させる
    public void ShowScore(Fruit.FruitType fruit)
    {
        scoreText.text = AddScore(fruit).ToString();
    }

    // スコアを返す
    private int AddScore(Fruit.FruitType fruit)
    {
        score += CalculateScore(fruit);
        return score;
    }

    // スコアを計算する
    private int CalculateScore(Fruit.FruitType fruit)
    {
        return scorePointArray[(int)fruit];
    }

    public int GetScore()
    {
        return score;
    }
}
