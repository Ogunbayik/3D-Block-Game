using DG.Tweening;
using UnityEngine;

public class ShapeAnimationController : MonoBehaviour
{
    [Header("Jelly Animation")]
    [SerializeField] private Vector3 _squashScale;
    [SerializeField] private float _squashDuration;
    [SerializeField] private Vector3 _stretchScale;
    [SerializeField] private float _stretchDuration;
    [SerializeField] private float _recoveryDuration;
    [Header("Growth Animation")]
    [SerializeField] private Vector3 _growthScale;
    [SerializeField] private float _growthDuration;
    [Header("Shrinkage Animation")]
    [SerializeField] private Vector3 _shrinkageScale;
    [SerializeField] private float _shrinkageDuration;
    public void PlayJellySequence()
    {
        transform.DOKill();
        Vector3 originalScale = Vector3.one;

        Sequence jellySequence = DOTween.Sequence();

        jellySequence.Append(transform.DOScale(_squashScale, _squashDuration)).SetEase(Ease.InQuad);

        jellySequence.Append(transform.DOScale(_stretchScale, _stretchDuration)).SetEase(Ease.InQuad);

        jellySequence.Append(transform.DOScale(originalScale, _recoveryDuration).SetEase(Ease.OutElastic));
    }
    public void PlayGrowthAnimation() => transform.DOScale(_growthScale,_growthDuration).SetEase(Ease.InBack);
    public void PlayShrinkageAnimation() => transform.DOScale(_shrinkageScale, _shrinkageDuration).SetEase(Ease.OutBack);
}
