using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UniRx;
using UnityEngine;

public class ExplainText : UIFadeAnimationUIPartBase
{
    protected override void Init()
    {
        CanvasGroup.alpha = 0f;
        ChangeInteractive(false);
    }
}
