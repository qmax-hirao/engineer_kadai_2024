using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject KeyConfigDisplayObj;
    public void GameStart()
    {
        SoundManager.instance.PlayButtonSE();
        SceneManager.LoadScene("GameScene");
    }

    public void KeyConfigDisplay(bool flg)
    {
        SoundManager.instance.PlayButtonSE();
        KeyConfigDisplayObj.SetActive(flg);
    }
}