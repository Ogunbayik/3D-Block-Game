using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockAnimationController : MonoBehaviour
{
    [Header("Pulse Dissolve Settings")]
    [SerializeField] private Vector3 _growthScale;
    [SerializeField] private float _growthDuration;
    [SerializeField] private float _dissolveDuration;
    public void PlayPulseDissolve()
    {
        Sequence dissolveSequnce = DOTween.Sequence();

        dissolveSequnce.Append(transform.DOScale(_growthScale,_growthDuration));

        dissolveSequnce.Append(transform.DOScale(Vector3.zero, _dissolveDuration));
    }
}
