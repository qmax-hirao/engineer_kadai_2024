// �f�[�^�x�[�X�̑S��
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetDataBaseScore : MonoBehaviour
{
    private string[] score;

    // �f�[�^�x�[�X�̏����擾���邽�߂�URL
    static string URL_SELECT = "http://118.27.11.211/2024kadai/student_02004/suika_get_data.php";

    // �R���[�`���̊J�n
    public IEnumerator init(Ranking ranking)
    {
        // UnityWebRequest���쐬����URL_SELECT�̃y�[�W�ɃA�N�Z�X
        UnityWebRequest request = UnityWebRequest.Get(URL_SELECT);

        yield return request.SendWebRequest();  // ���N�G�X�g�𑗐M���A���X�|���X��҂�

        if (request.result == UnityWebRequest.Result.Success) // ���X�|���X�̌��ʂ��`�F�b�N
        {
            // ���X�|���X�f�[�^���擾
            string data = request.downloadHandler.text;

            // ���s���� "<br>" ����؂�Ƃ��ăf�[�^�𕪊�
            score = data.Split(new string[] { "<br>" }, System.StringSplitOptions.None);
        }

        ranking.SetRanking(GetScore(ranking));
    }

    public int[] GetScore(Ranking ranking)
    {
        int[] intArray = new int[ranking.GetRankNumber()];

        for (int i = 0; i < ranking.GetRankNumber(); i++)
        {
            if (int.TryParse(score[i], out int parsedScore))
            {
                intArray[i] = parsedScore; // ���������ꍇ�͔z��Ɋi�[
                print(intArray[i]);
            }
            else
            {
                Debug.Log("�G���["); // �G���[���O
                intArray[i] = 0; // �f�t�H���g�l��G���[�����i�K�v�ɉ����ĕύX�j
            }
        }

        return intArray;
    }
}