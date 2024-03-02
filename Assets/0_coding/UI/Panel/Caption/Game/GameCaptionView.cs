using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameCaptionView : ViewBase
{
    [Header("親キャンバス")]
    [SerializeField]
    private Canvas _canvas;
    [Header("タイトルテキスト")]
    [SerializeField]
    private TMP_Text _titleText;
    /// <summary>
    /// タイトルテキスト
    /// </summary>
    public TMP_Text TitleText => _titleText;
    [Header("プログラマーメンバーテキスト")]
    [SerializeField]
    private TMP_Text _coadingMemberText;
    /// <summary>
    /// プログラマーメンバーテキスト
    /// </summary>
    public TMP_Text CoadingMemberText => _coadingMemberText;
    [Header("2Dメンバーテキスト")]
    [SerializeField]
    private TMP_Text _illustrationMemberText;
    /// <summary>
    /// 2Dメンバーテキスト
    /// </summary>
    public TMP_Text IllustrationMemberText => _illustrationMemberText;
    [Header("3Dメンバーテキスト")]
    [SerializeField]
    private TMP_Text _modelMemberText;
    /// <summary>
    /// 3Dメンバーテキスト
    /// </summary>
    public TMP_Text ModelMemberText => _modelMemberText;
    [Header("DTMメンバーテキスト")]
    [SerializeField]
    private TMP_Text _dtmMemberText;
    /// <summary>
    /// DTMメンバーテキスト
    /// </summary>
    public TMP_Text DtmMemberText => _dtmMemberText;
    [Header("URLテキスト")]
    [SerializeField]
    private TMP_Text _urlText;
    /// <summary>
    /// URLテキスト
    /// </summary>
    public TMP_Text UrlText => _urlText;
    [Header("説明テキスト")]
    [SerializeField]
    private TMP_Text _explainText;
    /// <summary>
    /// 説明テキスト
    /// </summary>
    public TMP_Text ExplainText => _explainText;
    [Header("キャプションボタン")]
    [SerializeField]
    private UnderButton _captionButton;
    /// <summary>
    /// キャプションボタン
    /// </summary>
    public UnderButton CaptionButton => _captionButton;
    [Header("キャプションの最上位位置")]
    [SerializeField]
    private float _maxPosY;
    [Header("アンカーとパネルの距離")]
    [SerializeField]
    private float _distance;

    private RectTransform _canvasRectTransform;
    public RectTransform CanvasRectTransform
    {
        get
        {
            if(_canvasRectTransform == null)
            {
                _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            }

            return _canvasRectTransform;
        }
    }
    private Camera _canvasCamera;
    public Camera CanvasCamera
    {
        get
        {
            if(_canvasCamera == null)
            {
                _canvasCamera = _canvas.worldCamera;
            }

            return _canvasCamera;
        }
    }

    private float _minPosY;

    protected override void Init()
    {
        _minPosY = RectTransform.anchoredPosition.y - _distance;
    }

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// パネルをY軸方向に移動
    /// </summary>
    /// <param name="value"> 動かす量 </param>
    public void MovePosY(float value)
    {
        RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, Mathf.Clamp(value, _minPosY, _maxPosY-_distance) + _distance);
    }

    /// <summary>
    /// テキストを消す
    /// </summary>
    /// <param name="targetText"> テキスト </param>
    public void HideText(TMP_Text targetText)
    {
        targetText.transform.parent.gameObject.SetActive(false);
    }
}