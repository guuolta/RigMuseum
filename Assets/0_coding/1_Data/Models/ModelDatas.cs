using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelDatas", menuName = "ScriptableObjects/CreateModelDatas")]
public class ModelDatas : ScriptableObjectBase<ModelData>
{
    //[Header("ケース")]
    //[SerializeField]
    //private List<>
}

[System.Serializable]
public class ModelData
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
    [Header("ケースの位置")]
    [SerializeField]
    private Direction _caseDirection = Direction.None;
    /// <summary>
    /// ケースの位置
    /// </summary>
    public Direction CaseDirection => _caseDirection;
}