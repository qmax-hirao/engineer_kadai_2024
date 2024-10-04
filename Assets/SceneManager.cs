using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Card CardPrefab;   // �J�[�h�̃v���n�u
    [SerializeField] private GameObject Dealer; // �f�B�[���[�̃J�[�h�u����
    [SerializeField] private GameObject Player; // �v���C���[�̃J�[�h�u����
    [SerializeField] private GameObject BetsInputDialog; // �x�b�g������z�����߂�_�C�A���O
    [SerializeField] private InputField BetsInput;     // �q�����̃C���v�b�g�t�B�[���h
    [SerializeField] private Button BetsInputOKButton; // �x�b�g�������z�̂̌���{�^��
    [SerializeField] private Text BetsText;   // �x�b�g�������z��\��
    [SerializeField] private Text PointText;  // ���ݎ����������Ă�����z
    [SerializeField] private Text ResultText; // ���ʂ�\��
    [SerializeField] private float WaitResultSeconds; // ���U���g��\��������b��
    [SerializeField] private int StartPoint; // �v���C���[�������Ă��鏉���̋��z
    private int currentPoint; // �v���C���[�̏�����
    private int currentBets;  // �q����
    [Min(100)] [SerializeField] private int ShuffleCount; // �V���b�t�������
    private List<Card.Data> cards; // �R�D�̃J�[�h�̃��X�g
    [SerializeField] private Text DealerTotalPointText; // �f�B�[���[�̃n���h�̍��v�_��
    [SerializeField] private Text PlayerTotalPointText; // �v���C���[�̃n���h�̍��v�_��
    [SerializeField] private float dealerDrawTime;
    private bool waitAction;

    // �s���̎��
    private enum Action
    {
        WaitAction = 0,
        Hit = 1,
        Stand = 2,
    }
    private Action CurrentAction; // ���݂̍s���̎��

    // ���s
    private enum Result
    {
        Win,
        Lose,
        Draw,
    }
    private Result result; // ���݂̍s���̎��

    private void Awake()
    {
        // �����l
        CurrentAction = Action.WaitAction;

        BetsInput.onValidateInput = BetsInputOnValidateInput;
        BetsInput.onValueChanged.AddListener(BetsInputOnValueChanged);
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    // ���݂̍s����ݒ�
    public void SetAction(int action)
    {
        CurrentAction = (Action)action;
    }

    // ���͂��ꂽ�������������ǂ������`�F�b�N
    private char BetsInputOnValidateInput(string text, int startIndex, char addedChar)
    {
        // �����łȂ��ꍇ�͖����ȓ��͂Ƃ��� '\0' ��Ԃ�
        if (!char.IsDigit(addedChar)) return '\0';
        // �����̏ꍇ�͂��̂܂ܕԂ�
        return addedChar;
    }

    // �{�^���̗L��������
    private void BetsInputOnValueChanged(string text)
    {
        // ������Ԃł�OK�{�^���𖳌�������
        BetsInputOKButton.interactable = false;

        // ���͂��ꂽ�e�L�X�g�������ɕϊ��ł��邩�`�F�b�N
        if (int.TryParse(BetsInput.text, out int bets))
        {
            // �ϊ��ł����ꍇ�A�x�b�g�z��0���傫���A�������|�C���g�ȓ��ł����OK�{�^����L��������
            if (0 < bets && bets <= currentPoint)
            {
                BetsInputOKButton.interactable = true;
            }
        }
    }

    private IEnumerator GameLoop()
    {
        // �����l����
        currentPoint = StartPoint;
        BetsText.text = "0";
        PointText.text = currentPoint.ToString();
        ResultText.gameObject.SetActive(false);

        while (true)
        {
            //�J�[�h������������
            InitCards();

            //�x�b�g�����߂�܂ő҂�
            do
            {
                BetsInputDialog.SetActive(true);

                // BetsInputDialog���\������Ă���ԁA�������ꎞ��~����
                yield return new WaitWhile(() => BetsInputDialog.activeSelf);

                // ���͂��ꂽ�e�L�X�g�������ɕϊ��ł��邩
                if (int.TryParse(BetsInput.text, out int bets))
                {
                    // �x�b�g�z��0���傫���A�������|�C���g�ȓ��̏ꍇ
                    if (0 < bets && bets <= currentPoint)
                    {
                        // ���݂̃x�b�g�z�Ƃ��Đݒ�
                        currentBets = bets;
                        break;
                    }
                }
            } while (true);

            // ��ʂ̍X�V
            BetsInputDialog.SetActive(false);
            BetsText.text = currentBets.ToString();

            // �J�[�h��z��
            DealCards();

            yield return null;

            // ���v�_�̌v�Z
            DealerTotalPointText.text = CalculationTotalPoint(Dealer).ToString();
            PlayerTotalPointText.text = CalculationTotalPoint(Player).ToString();

            // �v���C���[���s�������߂�܂ő҂�
            waitAction = true;

            do
            {
                CurrentAction = Action.WaitAction;
                yield return new WaitWhile(() => CurrentAction == Action.WaitAction);
                
                // �s���ɍ��킹�ď����𕪊򂷂�
                switch (CurrentAction)
                {
                    // �q�b�g�̏ꍇ
                    case Action.Hit:
                        PlayerDealCard();
                        // ���v�_�̌v�Z
                        PlayerTotalPointText.text = CalculationTotalPoint(Player).ToString();
                        waitAction = true;
                        if (!CheckPlayerCard())
                        {
                            waitAction = false;
                            result = Result.Lose;
                        }
                        break;
                    // �X�^���h�̏ꍇ
                    case Action.Stand:
                        StartCoroutine(StandAction());
                        yield return new WaitWhile(() => waitAction == true);
                        break;
                    // �z��O�̏����̏ꍇ
                    default:
                        waitAction = true;
                        print("�G���[�ł�");
                        break;
                }
            } while (waitAction);

            //�Q�[���̌��ʂ𔻒肷��
            ResultText.gameObject.SetActive(true);

            switch (result)
            {
                // ����
                case Result.Win:
                    currentPoint += currentBets;
                    ResultText.text = "Win!! +" + currentBets;
                    break;
                // ����
                case Result.Lose:
                    currentPoint -= currentBets;
                    ResultText.text = "Lose... -" + currentBets;
                    break;
                // ��������
                case Result.Draw:
                    ResultText.text = "Draw";
                    break;
                default:
                    break;
            }

            PointText.text = currentPoint.ToString();

            yield return new WaitForSeconds(WaitResultSeconds);
            ResultText.gameObject.SetActive(false);

            //�Q�[���I�[�o�[�E�Q�[���N���A����
            if (currentPoint <= 0)
            {
                ResultText.gameObject.SetActive(true);
                ResultText.text = "Game Over...";
                break;
            }
        }
    }

    // �R�D���쐬����
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

        // �R�D���V���b�t������
        ShuffleCards();
    }

    // �R�D���V���b�t������
    private void ShuffleCards()
    {
        for (int i = 0; i < ShuffleCount; ++i)
        {
            // �����_���ŗv�f�ԍ��𐶐�����
            int index  = Random.Range(0, cards.Count);
            int index2 = Random.Range(0, cards.Count);

            //�J�[�h�̈ʒu�����ւ���
            Card.Data tmp = cards[index];
            cards[index] = cards[index2];
            cards[index2] = tmp;
        }
    }

    // �J�[�h��z��
    private Card.Data DealCard()
    {
        if (cards.Count <= 0) return null;

        Card.Data card = cards[0];
        cards.Remove(card);
        return card;
    }

    // �f�B�[���[�ƃv���C���[�ɃJ�[�h��z��
    private void DealCards()
    {
        // �O��̃J�[�h�̃I�u�W�F�N�g���폜����
        foreach (Transform card in Dealer.transform)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in Player.transform)
        {
            Destroy(card.gameObject);
        }

        //�f�B�[���[�ɂQ���J�[�h��z��
        Card holeCardObj = Instantiate(CardPrefab, Dealer.transform);
        Card.Data holeCard = DealCard();
        holeCardObj.SetCard(holeCard.number, holeCard.mark, true);

        Card upCardObj = Instantiate(CardPrefab, Dealer.transform);
        Card.Data upCard = DealCard();
        upCardObj.SetCard(upCard.number, upCard.mark, false);

        //�v���C���[�ɃJ�[�h���Q���z��
        for (int i = 0; i < 2; ++i)
        {
            Card cardObj = Instantiate(CardPrefab, Player.transform);
            Card.Data card = DealCard();
            cardObj.SetCard(card.number, card.mark, false);
        }
    }

    // �v���C���[�ɃJ�[�h��z��
    private void PlayerDealCard()
    {
        Card cardObj = Instantiate(CardPrefab, Player.transform);
        Card.Data card = DealCard();
        cardObj.SetCard(card.number, card.mark, false);
    }

    // �v���C���[�̃J�[�h���`�F�b�N����
    private bool CheckPlayerCard()
    {
        int sumNumber = CalculationTotalPoint(Player);
        return (sumNumber <= 21);
    }

    // �X�^���h���̏���
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

        // ���s�̌��ʂ�ϐ��ɓ����
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

    // ���v�_���v�Z����
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
