using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyConfig : MonoBehaviour
{
    static public KeyConfig instance;
    public KeyCode leftMoveKey;
    public KeyCode rightMoveKey;
    public KeyCode pauseKey;
    public KeyCode fruitPutKey;
    public KeyCode rankingResetKey;
    public string leftMoveKeyText;
    public string rightMoveKeyText;
    public string pauseKeyText;
    public string fruitPutKeyText;
    public string rankingResetKeyText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LeftMoveKeyConfig(Dropdown ddtmp)
    {
        leftMoveKeyText = ddtmp.options[ddtmp.value].text;

        switch (ddtmp.value)
        {
            case 0:
                leftMoveKey = KeyCode.LeftArrow;
                break;
            case 1:
                leftMoveKey = KeyCode.A;
                break;
            default:
                break;
        }
    }

    public void RightMoveKeyConfig(Dropdown ddtmp)
    {
        rightMoveKeyText = ddtmp.options[ddtmp.value].text;

        switch (ddtmp.value)
        {
            case 0:
                rightMoveKey = KeyCode.RightArrow;
                break;
            case 1:
                rightMoveKey = KeyCode.D;
                break;
            default:
                break;
        }
    }

    public void PauseKeyConfig(Dropdown ddtmp)
    {
        pauseKeyText = ddtmp.options[ddtmp.value].text;

        switch (ddtmp.value)
        {
            case 0:
                pauseKey = KeyCode.X;
                break;
            case 1:
                pauseKey = KeyCode.P;
                break;
            default:
                break;
        }
    }

    public void FruitPutKeyConfig(Dropdown ddtmp)
    {
        fruitPutKeyText = ddtmp.options[ddtmp.value].text;

        switch (ddtmp.value)
        {
            case 0:
                fruitPutKey = KeyCode.Space;
                break;
            case 1:
                fruitPutKey = KeyCode.Return;
                break;
            default:
                break;
        }
    }

    public void RankingResetKeyConfig(Dropdown ddtmp)
    {
        rankingResetKeyText = ddtmp.options[ddtmp.value].text;

        switch (ddtmp.value)
        {
            case 0:
                rankingResetKey = KeyCode.Escape;
                break;
            case 1:
                rankingResetKey = KeyCode.RightShift;
                break;
            default:
                break;
        }
    }
}
