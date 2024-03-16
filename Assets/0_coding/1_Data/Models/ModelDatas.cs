using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ModelDatas", menuName = "ScriptableObjects/CreateModelDatas")]
public class ModelDatas : ScriptableObjectBase<ModelData>
{
    //[Header("ケース")]
    //[SerializeField]
    //private List<>
}
#if UNITY_EDITOR


[CustomEditor(typeof(ModelDatas))]
public class ModelDatasEditor : ArtDatasEditorBase
{
    
}
#endif

[System.Serializable]
public class ModelData:IArtData
{
    private int _id;
    /// <summary>
    /// モデル番号
    /// </summary>
    public int ID => _id;
    [Header("モデル名")]
    [SerializeField]
    private string _name;
    /// <summary>
    /// モデル名
    /// </summary>
    public string Name => _name;
    [Header("モデル")]
    [SerializeField]
    private GameObject _model;
    /// <summary>
    /// モデル
    /// </summary>
    public GameObject Model => _model;
    [Header("製作者")]
    [SerializeField]
    private string[] _members = new string[0];
    /// <summary>
    /// 製作者
    /// </summary>
    public string[] Members => _members;
    [Header("モデルの説明")]
    [SerializeField]
    private string _description;
    /// <summary>
    /// モデルの説明
    /// </summary>
    public string Description => _description;
    [Header("ケースの種類")]
    [SerializeField]
    private CaseType _caseType = CaseType.None;
    /// <summary>
    /// ケースの種類
    /// </summary>
    public CaseType CaseType => _caseType;
    [Header("ターゲット時の距離")]
    [SerializeField]
    private float _offset;
    /// <summary>
    /// ターゲット時の距離
    /// </summary>
    public float Offset => _offset;

    public void SetIndex(int index)
    {
        if (index < 0)
        {
            return;
        }

        _id = index;
    }
}