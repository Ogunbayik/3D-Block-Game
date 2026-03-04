using DG.Tweening;
using UnityEngine;

public class ShapeAnimationController : MonoBehaviour
{
    [Header("Shape References")]
    [SerializeField] private BaseShape _shape;
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
    [SerializeField] private float _releasedDuration;
    private void OnEnable()
    {
        _shape.OnClicked += Shape_OnClicked;
        _shape.OnReleased += Shape_OnReleased;
    }
    private void OnDisable()
    {
        _shape.OnClicked -= Shape_OnClicked;
        _shape.OnReleased -= Shape_OnReleased;
    }
    private void Shape_OnClicked() => PlayGrowthAnimation();
    private void Shape_OnReleased() => PlayReleasedSequence();
    public void PlayGrowthAnimation() => transform.DOScale(_growthScale, _growthDuration).SetEase(Ease.OutQuad);
    public void PlayReleasedSequence()
    {
        transform.DOKill();

        Sequence releasedSequence = DOTween.Sequence();

        releasedSequence.Append(transform.DOMove(_shape.SpawnPosition, _releasedDuration));
        releasedSequence.JoinCallback(() => _shape.ChangeLayerOfAllBlocks(_shape.ReleasedLayer));

        releasedSequence.Append(transform.DOScale(_shrinkageScale, _shrinkageDuration)).SetEase(Ease.OutBack);

        releasedSequence.AppendCallback(() => _shape.ChangeLayerOfAllBlocks(_shape.DraggableLayer));
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
