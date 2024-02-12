using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PanelViewBase : MonoBehaviour
{
    private void Awake()
    {
        this.transform.localScale = Vector3.zero;
    }

    public async UniTask ShowPanelAsync(float animeTime)
    {
        await this.transform.DOScale(Vector2.one, animeTime).AsyncWaitForCompletion();
    }

    public async UniTask ClosePanelAsync(float animeTime)
    {
        await this.transform.DOScale(Vector2.zero, animeTime).AsyncWaitForCompletion();
    }
}
