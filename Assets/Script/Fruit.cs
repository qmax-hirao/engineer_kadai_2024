using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool evolutionFlg; // �����s�[�X���Փ˂���(�i��)�t���O
    private bool collDifferentFruitFlg; // �قȂ�t���[�c�̏Փ˃t���O
    private Vector2 hitPos; // �Փ˂������W
    [SerializeField] private Sprite sprite; // �t���[�c�̃X�v���C�g
    public enum FruitType // �t���[�c�̎��
    {
        CHERRY,     // �`�F���[
        STRAWBERRY, // �C�`�S
        GRAPE,      // �u�h�E
        DEKOPON,    // �f�R�|��
        PERSIMMON,  // �J�L
        APPLE,      // �����S
        PEAR,       // �i�V
        PEACH,      // ����
        PINEAPPLE,  // �p�C�i�b�v��
        MELON,      // ������
        WATERMELON, // �X�C�J
    }
    [SerializeField] private FruitType fruitType; // ���̃I�u�W�F�N�g�̃t���[�c�̎��
    private Action<FruitType, Vector2> evolutionFruit; // FruitManager.cs��EvolutionFruit�֐�������ϐ�

    // �Փ˂����Ƃ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fruit collFruit = collision.gameObject.GetComponent<Fruit>();

        // Piece�X�N���v�g��null�̏ꍇ�������Ȃ�
        if (!collFruit) return;

        // ���łɂ�������œ����蔻�菈������Ă���ꍇ�������Ȃ�(�i���̏�����2����s���Ȃ��悤��)
        if (GetEvolutionFlg()) return;
        if (collFruit.GetEvolutionFlg()) return;

        // �Փ˂����s�[�X�̎�ނɂ���ď����𕪊�
        if (collFruit.fruitType == fruitType)
        {
            // SE
            SoundManager.instance.PlayEvolutionSE();

            // �i���t���O
            evolutionFlg = true;
            collFruit.SetEvolutionFlg();

            // �X�R�A�𔽉f������
            Manager.instance.GetScoreClass().ShowScore(fruitType);

            // �s�[�X��i��������
            if (collFruit.fruitType != FruitType.WATERMELON)
            {
                EvolutionFruit(collision);
            }

            // �G�ꂽ�s�[�X���m���폜
            DestroyPiece(collision.gameObject);
        }
        else
        {
            // �ʂ̃s�[�X���Փ˂��Ă���t���O
            collDifferentFruitFlg = true;
        }
    }

    // �G��Ă���I�u�W�F�N�g�����ꂽ��
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {
            collDifferentFruitFlg = false;
        }
    }

    // �s�[�X������
    private void DestroyPiece(GameObject collObj)
    {
        Destroy(gameObject);
        Destroy(collObj);
    }

    // �s�[�X��i��������
    private void EvolutionFruit(Collision2D collision)
    {
        // �s�[�X���m���Փ˂������W�����߂�
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

    // �s�[�X��i��������֐���ݒ肷��(�R�[���o�b�N)
    public void SetFuncEvolutionFruit(Action<FruitType, Vector2> callback)
    {
        evolutionFruit = callback;
    }
}
