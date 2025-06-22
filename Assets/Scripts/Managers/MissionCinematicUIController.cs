using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

public class MissionCinematicUIController : Singleton<MissionCinematicUIController>
{
    [field: SerializeField] public PlayableDirector PlayableDirector { get; private set; }
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeInDuration = .5f;
    [SerializeField] private float fadeOutDuration = .5f;

    public void PlayCinematic(MissionData mission, PlayableAsset playableAsset)
    {
        text.text = mission.MissionName;
        PlayableDirector.playableAsset = playableAsset;
        canvasGroup.alpha = 0f;
        text.color = text.color.With(a: 0);
        canvasGroup.blocksRaycasts = true;
        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, fadeInDuration));
        sequence.Append(text.DOFade(1, fadeInDuration / 2));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            text.DOFade(0, fadeOutDuration / 2);
            PlayableDirector.Play();
        });
        sequence.Append(canvasGroup.DOFade(0f,fadeOutDuration));
    }
}