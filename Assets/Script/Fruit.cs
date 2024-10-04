using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool evolutionFlg; // 同じピースが衝突した(進化)フラグ
    private bool collDifferentFruitFlg; // 異なるフルーツの衝突フラグ
    private Vector2 hitPos; // 衝突した座標
    [SerializeField] private Sprite sprite; // フルーツのスプライト
    public enum FruitType // フルーツの種類
    {
        CHERRY,     // チェリー
        STRAWBERRY, // イチゴ
        GRAPE,      // ブドウ
        DEKOPON,    // デコポン
        PERSIMMON,  // カキ
        APPLE,      // リンゴ
        PEAR,       // ナシ
        PEACH,      // モモ
        PINEAPPLE,  // パイナップル
        MELON,      // メロン
        WATERMELON, // スイカ
    }
    [SerializeField] private FruitType fruitType; // このオブジェクトのフルーツの種類
    private Action<FruitType, Vector2> evolutionFruit; // FruitManager.csのEvolutionFruit関数を入れる変数

    // 衝突したとき
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fruit collFruit = collision.gameObject.GetComponent<Fruit>();

        // Pieceスクリプトがnullの場合処理しない
        if (!collFruit) return;

        // すでにもう一方で当たり判定処理されている場合処理しない(進化の処理を2回実行しないように)
        if (GetEvolutionFlg()) return;
        if (collFruit.GetEvolutionFlg()) return;

        // 衝突したピースの種類によって処理を分岐
        if (collFruit.fruitType == fruitType)
        {
            // SE
            SoundManager.instance.PlayEvolutionSE();

            // 進化フラグ
            evolutionFlg = true;
            collFruit.SetEvolutionFlg();

            // スコアを反映させる
            Manager.instance.GetScoreClass().ShowScore(fruitType);

            // ピースを進化させる
            if (collFruit.fruitType != FruitType.WATERMELON)
            {
                EvolutionFruit(collision);
            }

            // 触れたピース同士を削除
            DestroyPiece(collision.gameObject);
        }
        else
        {
            // 別のピースが衝突しているフラグ
            collDifferentFruitFlg = true;
        }
    }

    // 触れているオブジェクトが離れた時
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            collDifferentFruitFlg = false;
        }
    }

    // ピースを消す
    private void DestroyPiece(GameObject collObj)
    {
        Destroy(gameObject);
        Destroy(collObj);
    }

    // ピースを進化させる
    private void EvolutionFruit(Collision2D collision)
    {
        // ピース同士が衝突した座標を求める
        foreach (ContactPoint2D point in collision.contacts)
        {
            hitPos = point.point;
        }

        evolutionFruit(fruitType, hitPos);
    }

    public bool GetEvolutionFlg()
    {
        return evolutionFlg;
    }

    public void SetEvolutionFlg()
    {
        evolutionFlg = true;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }

    public bool GetCollDifferentFruitFlg()
    {
        return collDifferentFruitFlg;
    }

    // ピースを進化させる関数を設定する(コールバック)
    public void SetFuncEvolutionFruit(Action<FruitType, Vector2> callback)
    {
        evolutionFruit = callback;
    }
}
