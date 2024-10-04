
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private bool isReverse;
    private int number;
    private Mark currentMark;

    public class Data
    {
        public Mark mark;
        public int number;
    }

    public enum Mark
    {
        Heart,
        Diamond,
        Spade,
        Crub,
    }

    // カードの点数
    public int UseNumber
    {
        get
        {
            if (number > 10) return 10;
            return number;
        }
    }

    // カードの情報を設定する
    public void SetCard(int num, Mark mark, bool reverseFlg)
    {
        number = Mathf.Clamp(num, 1, 13);
        currentMark = mark;
        isReverse = reverseFlg;

        //カードの裏表に合わせて色などを設定する
        Image image = GetComponent<Image>();
        if (isReverse)
        {
            image.color = Color.black;
        }
        else
        {
            image.color = Color.white;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!isReverse);
        }

        //マークに合わせてGameObjectを設定する
        Transform markObj = transform.Find("Mark");
        Text markText = markObj.GetComponent<Text>();
        switch (currentMark)
        {
            case Mark.Heart:
                markText.text = "❤️";
                markText.color = Color.red;
                break;
            case Mark.Diamond:
                markText.text = "♦️";
                markText.color = Color.red;
                break;
            case Mark.Spade:
                markText.text = "♠️";
                markText.color = Color.black;
                break;
            case Mark.Crub:
                markText.text = "♣️";
                markText.color = Color.black;
                break;
        }

        //数字に合わせてGameObjectを設定する
        Transform numberObj = transform.Find("NumberText");
        Text numberText = numberObj.GetComponent<Text>();
        if (number == 1)
        {
            numberText.text = "A";
        }
        else if (number == 11)
        {
            numberText.text = "J";
        }
        else if (number == 12)
        {
            numberText.text = "Q";
        }
        else if (number == 13)
        {
            numberText.text = "K";
        }
        else
        {
            numberText.text = number.ToString();
        }
    }

    public bool IsReverse { get { return isReverse; } }

    public int Number { get { return number; } }

    public Mark CurrentMark { get { return currentMark; } }
}