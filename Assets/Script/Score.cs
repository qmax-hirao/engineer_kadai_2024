using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �X�R�A�̌v�Z��\���̏������Ǘ�����
public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;   // �X�R�A�̃e�L�X�g
    [SerializeField] private int[] scorePointArray; // �ʕ����i���������̓_���̔z��
    private int score;

    private void Start()
    {
        scoreText.text = "0";
    }

    // �X�R�A��\��������
    public void ShowScore(Fruit.FruitType fruit)
    {
        scoreText.text = AddScore(fruit).ToString();
    }

    // �X�R�A��Ԃ�
    private int AddScore(Fruit.FruitType fruit)
    {
        score += CalculateScore(fruit);
        return score;
    }

    // �X�R�A���v�Z����
    private int CalculateScore(Fruit.FruitType fruit)
    {
        return scorePointArray[(int)fruit];
    }

    public int GetScore()
    {
        return score;
    }
}
