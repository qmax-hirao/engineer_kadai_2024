using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フルーツの生成と進化、フルーツを落とす座標などを管理する
public class FruitManager : MonoBehaviour
{
    [SerializeField] private Fruit[] fruitPrefab; // フルーツのプレハブ
    [SerializeField] private Transform putPos;    // フルーツを置く時の座標
    [SerializeField] private float moveSpeed;     // フルーツを置く座標の移動速度
    [SerializeField] private GameObject currentFruitImage; // 現在のフルーツの画像
    [SerializeField] private GameObject nextFruitImage;    // 次のフルーツの画像
    [SerializeField] private float moveLimitMin; // ピースを置く座標の制限座標(最低値)
    [SerializeField] private float moveLimitMax; // ピースを置く座標の制限座標(最高値)
    private int currentFruitIndex; // 現在のフルーツの要素番号
    private int nextFruitIndex;    // 次のフルーツの要素番号
    private bool isCoolTime;       // クールタイムかどうか
    [SerializeField] private float putCoolTime; // ピースを置くクールタイムの時間

    // 現在の落とすフルーツの設定をする
    public void CurrentFruit(int index)
    {
        // 現在のフルーツの番号
        currentFruitIndex = index;

        //画像を設定
        SetImage(currentFruitImage, currentFruitIndex);
    }

    // 次の落とすフルーツの設定をする
    public void NextFruit(int index)
    {
        // 次のフルーツの番号
        nextFruitIndex = index;

        //画像を設定
        SetImage(nextFruitImage, nextFruitIndex);
    }

    // フルーツを生成する座標を決める
    public void MoveGeneratePos()
    {
        Vector2 pos = putPos.position;

        // 左右キーで移動する
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos = new Vector2(putPos.position.x - moveSpeed * Time.deltaTime, putPos.position.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            pos = new Vector2(putPos.position.x + moveSpeed * Time.deltaTime, putPos.position.y);
        }

        // 移動制限
        pos.x = Mathf.Clamp(pos.x, moveLimitMin + (currentFruitImage.transform.localScale.x / 10), 
                                   moveLimitMax - (currentFruitImage.transform.localScale.x / 10));

        putPos.position = pos;
    }

    // フルーツを落とす
    public void PutFruit()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCoolTime)
        {
            // SE
            SoundManager.instance.PlayFallSE();

            // クールタイム
            isCoolTime = true;
            StartCoroutine(CoolTime());

            // 落とすフルーツを生成
            Fruit fruitInstance = Instantiate(fruitPrefab[currentFruitIndex], putPos.position, transform.rotation);

            // フルーツを進化させる関数を設定する(コールバック)
            fruitInstance.SetFuncEvolutionFruit(EvolutionFruit);

            // 現在のフルーツを設定
            CurrentFruit(nextFruitIndex);

            // ネクストのフルーツを設定
            NextFruit(RandomGenerateFruitIndex());
        }
    }

    // フルーツを進化させる
    private void EvolutionFruit(Fruit.FruitType fruit, Vector2 pos)
    {
        // 進化したフルーツを生成
        Fruit fruitInstance = Instantiate(fruitPrefab[(int)fruit + 1], pos, transform.rotation);

        // 進化前がメロン未満の場合、コールバックを設定する
        if (fruit < Fruit.FruitType.MELON)
        {
            // フルーツを進化させる関数を設定する(コールバック)
            fruitInstance.SetFuncEvolutionFruit(EvolutionFruit);
        }
    }

    // 次にフルーツを落とすまでのクールタイム
    private IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(putCoolTime);
        isCoolTime = false;
    }

    // 画像や大きさを設定する
    private void SetImage(GameObject image, int index)
    {
        // 画像を設定
        image.GetComponent<SpriteRenderer>().sprite = fruitPrefab[index].GetSprite();

        // 画像の大きさを設定
        image.transform.localScale = fruitPrefab[index].transform.localScale;
    }

    public int RandomGenerateFruitIndex()
    {
        return Random.Range(0, 5); // チェリーからカキまで
    }

    public int GetCurrentFruitIndex()
    {
        return currentFruitIndex;
    }

    public int GetNextFruitIndex()
    {
        return nextFruitIndex;
    }
}