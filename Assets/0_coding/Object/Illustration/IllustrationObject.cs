using UnityEngine;

public class IllustrationObject : ArtObjectBase
{
    private int _id;
    /// <summary>
    /// イラストの番号
    /// </summary>
    public int ID => _id;
    private Sprite _illustration;
    [Header("イラストを設定するオブジェクト")]
    [SerializeField]
    private GameObject _canvas;
    protected GameObject canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = Transform.GetChild(0).gameObject;
            }

            return _canvas;
        }
    }

    /// <summary>
    /// イラストの設定
    /// </summary>
    /// <param name="index"> イラストの番号 </param>
    /// <param name="illustration"> イラスト </param>
    public void SetIllustration(int index, Sprite illustration, Material material)
    {
        _id = index;
        _illustration = illustration;
        SetIllustlationSize(illustration.texture.width, illustration.texture.height);
        SetMaterial(material);

    }

    /// <summary>
    /// イラストのサイズを設定
    /// </summary>
    /// <param name="width"> 横幅 </param>
    /// <param name="height"> 高さ </param>
    private void SetIllustlationSize(float width, float height)
    {
        float multiple = Mathf.Abs(
            Mathf.Floor(
                Mathf.Max(width / canvas.transform.localScale.x,
                    height / canvas.transform.localScale.y)));
        canvas.transform.localScale = new Vector3(width / multiple, height / multiple, canvas.transform.localScale.z);
    }

    /// <summary>
    /// マテリアルを設定
    /// </summary>
    /// <param name="material"></param>
    private void SetMaterial(Material material)
    {
        canvas.GetComponent<Renderer>().material = material;
    }
}