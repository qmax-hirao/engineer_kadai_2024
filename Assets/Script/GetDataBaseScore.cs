// データベースの全て
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetDataBaseScore : MonoBehaviour
{
    private string[] score;

    // データベースの情報を取得するためのURL
    static string URL_SELECT = "http://118.27.11.211/2024kadai/student_02004/suika_get_data.php";

    // コルーチンの開始
    public IEnumerator init(Ranking ranking)
    {
        // UnityWebRequestを作成してURL_SELECTのページにアクセス
        UnityWebRequest request = UnityWebRequest.Get(URL_SELECT);

        yield return request.SendWebRequest();  // リクエストを送信し、レスポンスを待つ

        if (request.result == UnityWebRequest.Result.Success) // レスポンスの結果をチェック
        {
            // レスポンスデータを取得
            string data = request.downloadHandler.text;

            // 改行文字 "<br>" を区切りとしてデータを分割
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
                intArray[i] = parsedScore; // 成功した場合は配列に格納
                print(intArray[i]);
            }
            else
            {
                Debug.Log("エラー"); // エラーログ
                intArray[i] = 0; // デフォルト値やエラー処理（必要に応じて変更）
            }
        }

        return intArray;
    }
}