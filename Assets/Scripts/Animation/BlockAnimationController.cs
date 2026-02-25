using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockAnimationController : MonoBehaviour
{
    [Header("Jelly Animation")]
    [SerializeField] private Vector3 _squashScale;
    [SerializeField] private float _squashDuration;
    [SerializeField] private Vector3 _stretchScale;
    [SerializeField] private float _stretchDuration;
    [SerializeField] private float _recoveryDuration;

    private void Update()
    {
        TestWithSpace.TestCode(PlayJellySequence);
    }
    public void PlayJellySequence()
    {
        transform.DOKill();
        Vector3 originalScale = Vector3.one;

        Sequence jellySequence = DOTween.Sequence();

        jellySequence.Append(transform.DOScale(_squashScale, _squashDuration).SetEase(Ease.InQuad));

        jellySequence.Append(transform.DOScale(_stretchScale, _stretchDuration).SetEase(Ease.InQuad));

        jellySequence.Append(transform.DOScale(originalScale, _recoveryDuration).SetEase(Ease.OutElastic));
    }

}
