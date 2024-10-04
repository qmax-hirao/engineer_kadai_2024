using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Card CardPrefab;   // カードのプレハブ
    [SerializeField] private GameObject Dealer; // ディーラーのカード置き場
    [SerializeField] private GameObject Player; // プレイヤーのカード置き場
    [SerializeField] private GameObject BetsInputDialog; // ベットする金額を決めるダイアログ
    [SerializeField] private InputField BetsInput;     // 賭け金のインプットフィールド
    [SerializeField] private Button BetsInputOKButton; // ベットした金額のの決定ボタン
    [SerializeField] private Text BetsText;   // ベットした金額を表示
    [SerializeField] private Text PointText;  // 現在自分が持っている金額
    [SerializeField] private Text ResultText; // 結果を表示
    [SerializeField] private float WaitResultSeconds; // リザルトを表示させる秒数
    [SerializeField] private int StartPoint; // プレイヤーが持っている初期の金額
    private int currentPoint; // プレイヤーの所持金
    private int currentBets;  // 賭け金
    [Min(100)] [SerializeField] private int ShuffleCount; // シャッフルする回数
    private List<Card.Data> cards; // 山札のカードのリスト
    [SerializeField] private Text DealerTotalPointText; // ディーラーのハンドの合計点数
    [SerializeField] private Text PlayerTotalPointText; // プレイヤーのハンドの合計点数
    [SerializeField] private float dealerDrawTime;
    private bool waitAction;

    // 行動の種類
    private enum Action
    {
        WaitAction = 0,
        Hit = 1,
        Stand = 2,
    }
    private Action CurrentAction; // 現在の行動の種類

    // 勝敗
    private enum Result
    {
        Win,
        Lose,
        Draw,
    }
    private Result result; // 現在の行動の種類

    private void Awake()
    {
        // 初期値
        CurrentAction = Action.WaitAction;

        BetsInput.onValidateInput = BetsInputOnValidateInput;
        BetsInput.onValueChanged.AddListener(BetsInputOnValueChanged);
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    // 現在の行動を設定
    public void SetAction(int action)
    {
        CurrentAction = (Action)action;
    }

    // 入力された文字が数字かどうかをチェック
    private char BetsInputOnValidateInput(string text, int startIndex, char addedChar)
    {
        // 数字でない場合は無効な入力として '\0' を返す
        if (!char.IsDigit(addedChar)) return '\0';
        // 数字の場合はそのまま返す
        return addedChar;
    }

    // ボタンの有効化判定
    private void BetsInputOnValueChanged(string text)
    {
        // 初期状態ではOKボタンを無効化する
        BetsInputOKButton.interactable = false;

        // 入力されたテキストが整数に変換できるかチェック
        if (int.TryParse(BetsInput.text, out int bets))
        {
            // 変換できた場合、ベット額が0より大きく、かつ所持ポイント以内であればOKボタンを有効化する
            if (0 < bets && bets <= currentPoint)
            {
                BetsInputOKButton.interactable = true;
            }
        }
    }

    private IEnumerator GameLoop()
    {
        // 初期値を代入
        currentPoint = StartPoint;
        BetsText.text = "0";
        PointText.text = currentPoint.ToString();
        ResultText.gameObject.SetActive(false);

        while (true)
        {
            //カードを初期化する
            InitCards();

            //ベットを決めるまで待つ
            do
            {
                BetsInputDialog.SetActive(true);

                // BetsInputDialogが表示されている間、処理を一時停止する
                yield return new WaitWhile(() => BetsInputDialog.activeSelf);

                // 入力されたテキストが整数に変換できるか
                if (int.TryParse(BetsInput.text, out int bets))
                {
                    // ベット額が0より大きく、かつ所持ポイント以内の場合
                    if (0 < bets && bets <= currentPoint)
                    {
                        // 現在のベット額として設定
                        currentBets = bets;
                        break;
                    }
                }
            } while (true);

            // 画面の更新
            BetsInputDialog.SetActive(false);
            BetsText.text = currentBets.ToString();

            // カードを配る
            DealCards();

            yield return null;

            // 合計点の計算
            DealerTotalPointText.text = CalculationTotalPoint(Dealer).ToString();
            PlayerTotalPointText.text = CalculationTotalPoint(Player).ToString();

            // プレイヤーが行動を決めるまで待つ
            waitAction = true;

            do
            {
                CurrentAction = Action.WaitAction;
                yield return new WaitWhile(() => CurrentAction == Action.WaitAction);
                
                // 行動に合わせて処理を分岐する
                switch (CurrentAction)
                {
                    // ヒットの場合
                    case Action.Hit:
                        PlayerDealCard();
                        // 合計点の計算
                        PlayerTotalPointText.text = CalculationTotalPoint(Player).ToString();
                        waitAction = true;
                        if (!CheckPlayerCard())
                        {
                            waitAction = false;
                            result = Result.Lose;
                        }
                        break;
                    // スタンドの場合
                    case Action.Stand:
                        StartCoroutine(StandAction());
                        yield return new WaitWhile(() => waitAction == true);
                        break;
                    // 想定外の処理の場合
                    default:
                        waitAction = true;
                        print("エラーです");
                        break;
                }
            } while (waitAction);

            //ゲームの結果を判定する
            ResultText.gameObject.SetActive(true);

            switch (result)
            {
                // 勝ち
                case Result.Win:
                    currentPoint += currentBets;
                    ResultText.text = "Win!! +" + currentBets;
                    break;
                // 負け
                case Result.Lose:
                    currentPoint -= currentBets;
                    ResultText.text = "Lose... -" + currentBets;
                    break;
                // 引き分け
                case Result.Draw:
                    ResultText.text = "Draw";
                    break;
                default:
                    break;
            }

            PointText.text = currentPoint.ToString();

            yield return new WaitForSeconds(WaitResultSeconds);
            ResultText.gameObject.SetActive(false);

            //ゲームオーバー・ゲームクリア処理
            if (currentPoint <= 0)
            {
                ResultText.gameObject.SetActive(true);
                ResultText.text = "Game Over...";
                break;
            }
        }
    }

    // 山札を作成する
    private void InitCards()
    {
        cards = new List<Card.Data>(13 * 4);
        List<Card.Mark> marks = new List<Card.Mark>() {
            Card.Mark.Heart,
            Card.Mark.Diamond,
            Card.Mark.Spade,
            Card.Mark.Crub,
        };

        foreach (Card.Mark cardMark in marks)
        {
            for (int num = 1; num <= 13; ++num)
            {
                Card.Data card = new Card.Data()
                {
                    mark = cardMark,
                    number = num,
                };
                cards.Add(card);
            }
        }

        // 山札をシャッフルする
        ShuffleCards();
    }

    // 山札をシャッフルする
    private void ShuffleCards()
    {
        for (int i = 0; i < ShuffleCount; ++i)
        {
            // ランダムで要素番号を生成する
            int index  = Random.Range(0, cards.Count);
            int index2 = Random.Range(0, cards.Count);

            //カードの位置を入れ替える
            Card.Data tmp = cards[index];
            cards[index] = cards[index2];
            cards[index2] = tmp;
        }
    }

    // カードを配る
    private Card.Data DealCard()
    {
        if (cards.Count <= 0) return null;

        Card.Data card = cards[0];
        cards.Remove(card);
        return card;
    }

    // ディーラーとプレイヤーにカードを配る
    private void DealCards()
    {
        // 前回のカードのオブジェクトを削除する
        foreach (Transform card in Dealer.transform)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in Player.transform)
        {
            Destroy(card.gameObject);
        }

        //ディーラーに２枚カードを配る
        Card holeCardObj = Instantiate(CardPrefab, Dealer.transform);
        Card.Data holeCard = DealCard();
        holeCardObj.SetCard(holeCard.number, holeCard.mark, true);

        Card upCardObj = Instantiate(CardPrefab, Dealer.transform);
        Card.Data upCard = DealCard();
        upCardObj.SetCard(upCard.number, upCard.mark, false);

        //プレイヤーにカードを２枚配る
        for (int i = 0; i < 2; ++i)
        {
            Card cardObj = Instantiate(CardPrefab, Player.transform);
            Card.Data card = DealCard();
            cardObj.SetCard(card.number, card.mark, false);
        }
    }

    // プレイヤーにカードを配る
    private void PlayerDealCard()
    {
        Card cardObj = Instantiate(CardPrefab, Player.transform);
        Card.Data card = DealCard();
        cardObj.SetCard(card.number, card.mark, false);
    }

    // プレイヤーのカードをチェックする
    private bool CheckPlayerCard()
    {
        int sumNumber = CalculationTotalPoint(Player);
        return (sumNumber <= 21);
    }

    // スタンド時の処理
    private IEnumerator StandAction()
    {
        foreach (Card card in Dealer.transform.GetComponentsInChildren<Card>())
        {
            if (card.IsReverse)
            {
                card.SetCard(card.Number, card.CurrentMark, false);
            }
        }

        DealerTotalPointText.text = CalculationTotalPoint(Dealer).ToString();

        yield return new WaitForSeconds(dealerDrawTime);

        int sumPlayerNumber = CalculationTotalPoint(Player);
        int sumDealerNumber = CalculationTotalPoint(Dealer);

        while (sumDealerNumber < 17)
        {
            Card holeCardObj = Instantiate(CardPrefab, Dealer.transform);
            Card.Data card = DealCard();
            holeCardObj.SetCard(card.number, card.mark, false);

            sumDealerNumber = CalculationTotalPoint(Dealer);
            DealerTotalPointText.text = sumDealerNumber.ToString();

            yield return new WaitForSeconds(dealerDrawTime);
        }

        // 勝敗の結果を変数に入れる
        if (sumDealerNumber > 21 || sumPlayerNumber > sumDealerNumber)
        {
            result = Result.Win;
        }
        else if(sumPlayerNumber < sumDealerNumber)
        {
            result = Result.Lose;
        }
        else
        {
            result = Result.Draw;
        }

        waitAction = false;
    }

    // 合計点を計算する
    private int CalculationTotalPoint(GameObject member)
    {
        int aceCount = 0;
        int point = 0;

        foreach (Card card in member.transform.GetComponentsInChildren<Card>())
        {
            if (!card.IsReverse)
            {
                if (card.UseNumber == 1)
                {
                    aceCount++;
                    point += 11;
                }
                else
                {
                    point += card.UseNumber;
                }

                while (point > 21 && aceCount > 0)
                {
                    aceCount--;
                    point -= 10;
                }
            }
        }

        return point;
    }
}
