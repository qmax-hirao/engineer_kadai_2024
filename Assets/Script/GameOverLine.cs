using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームオーバーの判定
public class GameOverLine : MonoBehaviour
{
    private bool gameOverFlg;

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Fruit")
        {
            Fruit collFruit = coll.gameObject.GetComponent<Fruit>();

            // Fruitスクリプトがnullの場合処理しない
            if (!collFruit) return;

            // 他の果物に接触していたらゲームオーバーフラグを立てる
            if (collFruit.GetCollDifferentFruitFlg())
            {
                gameOverFlg = true;
            }
        }
    }

    public bool GetGameOverFlg()
    {
        return gameOverFlg;
    }
}