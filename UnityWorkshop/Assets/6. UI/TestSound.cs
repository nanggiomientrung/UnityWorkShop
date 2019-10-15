using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestSound : MonoBehaviour
{
    [SerializeField] private Button testButton;
    [SerializeField] private AudioClip testClip;
    void Start()
    {
        testButton.onClick.AddListener(TestSoundOnClick);
    }

    private void TestSoundOnClick()
    {
        //SoundManager.instance.PlaySingleEffect(testClip);
        //SoundManager.instance.EffectSoundVolume = 1;
        //ShakeTheButton();
        MoveButton();
    }

    private void ShakeTheButton()
    {
        transform.DOShakePosition(1f, 2f);
    }

    private void MoveButton()
    {
        //transform.DOMoveY(200f, 0.5f);
        //transform.DOLocalMoveY(100, 0.5f).SetEase(Ease.OutExpo);
        //transform.DOLocalMoveY(100, 0.5f).SetLoops(2, LoopType.Yoyo);
        //transform.DOLocalMoveY(100, 0.5f).SetLoops(2,LoopType.Yoyo).
        //    OnComplete(PlaySound);
        //transform.DOShakeScale(1f,0.5f,5);
        transform.DOLocalMoveY(100, 0.5f).OnComplete(() => { transform.DOLocalMoveY(0, 0.5f).OnComplete(PlaySound); });
    }

    private void PlaySound()
    {
        SoundManager.instance.PlaySingleEffect(testClip);
    }
}
