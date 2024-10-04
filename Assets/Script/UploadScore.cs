using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UploadScore : MonoBehaviour
{
    public void UploadScores(int[] rankingScore)
    {
        StartCoroutine(SendScoresToServer(rankingScore));
    }

    private IEnumerator SendScoresToServer(int[] rankingScore)
    {
        string url = "http://118.27.11.211/2024kadai/student_02004/suika_send_data.php";

        // JSON形式でデータを作成
        string json = JsonUtility.ToJson(new ScoreData(rankingScore));

        Debug.Log("Sending JSON data: " + json);

        using (UnityWebRequest www = UnityWebRequest.Put(url, json))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
        }
    }
}

[System.Serializable]
public class ScoreData
{
    public int[] rankingScores;

    // コンストラクタでスコア配列を受け取る
    public ScoreData(int[] scores)
    {
        this.rankingScores = scores;
    }
}