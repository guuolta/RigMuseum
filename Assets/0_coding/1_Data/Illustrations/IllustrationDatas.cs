using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

[CreateAssetMenu(fileName = "IllustrationDatas", menuName = "ScriptableObjects/CreateIllustrationDatas")]
public class IllustrationDatas : ScriptableObjectBase<IllustrationData>
{
    [Header("額縁")]
    [SerializeField]
    private List<IllustrationObject> _frame = new List<IllustrationObject>();
    /// <summary>
    /// 額縁
    /// </summary>
    public List<IllustrationObject> Frame => _frame;
    [Header("キャプションベース")]
    [SerializeField]
    private ArtCaptionObject _captionBase = null;
    /// <summary>
    /// キャプションベース
    /// </summary>
    public ArtCaptionObject CaptionBase => _captionBase;
    [Header("作品名UIベース")]
    [SerializeField]
    ObjectNamePanelPresenter _nameUIBase = null;
    /// <summary>
    /// 作品名UIベース
    /// </summary>
    public ObjectNamePanelPresenter NameUIBase => _nameUIBase;
    [Header("イラストとキャプションの距離")]
    [SerializeField]
    private float _offset = 10f;
    /// <summary>
    /// イラストとキャプションの距離
    /// </summary>
    public float Offset => _offset;
}

#if UNITY_EDITOR
[CustomEditor(typeof(IllustrationDatas))]
public class IllustrationDatasEditor : ArtDatasEditorBase
{
    private const string ENUM_FILE_NAME = "FrameType";

    private const string ENUM_FOLDER_PATH = "Assets/0_coding/1_Data/Illustrations";
    private const string ILLUSTRATION_FOLDER_PATH = "Assets/3_2D/0_Prefabs/Illustration";
    private const string CAPTION_FOLDER_PATH = "Assets/3_2D/0_Prefabs/Caption";
    private const string ILLUSTRATION_OBJECT_FOLDER_PATH = "Assets/3_2D/0_Prefabs/IllustrationObjects";
    private const string MATERIAL_FOLDER_PATH = "Assets/3_2D/1_Materials";
    private const string ILLUSTRATION_NAME_UI_FOLDER_PATH = "Assets/5_source/Prefabs/UI/Panel/ObjectName/IllustrationNamePanels";
    private const string ILLUSTRATION_CAPTION_NAME_UI_FOLDER_PATH = "Assets/5_source/Prefabs/UI/Panel/ObjectName/IllustrationCaptionNamePanels";
    
    private int _listCount = 0;

    public override void OnInspectorGUI()
    {
        //ボタンを表示
        var createEnumButton = GUILayout.Button("Create Enum");
        var createPrefabButton = GUILayout.Button("Create Prefab");

        //データを表示
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_frame"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_captionBase"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_nameUIBase"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_offset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_dataList"));
        serializedObject.ApplyModifiedProperties();

        //Enum生成
        if(createEnumButton)
        {
            var illustrationDatas = (IllustrationDatas)target;
            CreateEnum(GetFrameNameList(illustrationDatas.Frame), ENUM_FOLDER_PATH, ENUM_FILE_NAME);
        }

        //プレハブ生成
        if (createPrefabButton)
        {
            var illustrationDatas = (IllustrationDatas)target;
            var illustrationDatasList = illustrationDatas.GetDataList();

            SetIndexToData<IllustrationDatas, IllustrationData>(illustrationDatas);

            string[] prefabFiles = Directory.GetFiles(ILLUSTRATION_FOLDER_PATH);
            string[] materialFiles = Directory.GetFiles(MATERIAL_FOLDER_PATH);
            string[] captionFiles = Directory.GetFiles(CAPTION_FOLDER_PATH);

            foreach (var illustrationData in illustrationDatasList)
            {
                string index = illustrationData.ID.ToString();
                string title = illustrationData.Title;

                if(title == ""
                    || illustrationData.Image == null
                    || illustrationData.Description == ""
                    || illustrationData.Members == new string[0]
                    || (SearchDistinctFile(prefabFiles, index + "_" + title + PREFAB_EXTENSION)
                        && SearchDistinctFile(materialFiles, index + "_" + title + MATERIAL_EXTENSION)
                        && SearchDistinctFile(captionFiles, index + "_" + title + "Caption" + PREFAB_EXTENSION)))
                {
                    continue;
                }

                Material material = CreateMaterial(MATERIAL_FOLDER_PATH,
                    illustrationData.Image.texture,
                    illustrationData.ID.ToString(),
                    illustrationData.Title);
                
                CreateNmaeUI(illustrationDatas.NameUIBase,
                    ILLUSTRATION_NAME_UI_FOLDER_PATH,
                    index,
                    title);
                
                GameObject illustration = CreateIllustrationPrefab(illustrationData,
                    illustrationDatas.Frame[(int)illustrationData.FrameType],
                    material);

                if (illustrationData.CaptionDirection != Direction.None)
                {
                    CreateCaptionNameUI(illustrationDatas.NameUIBase,
                        ILLUSTRATION_CAPTION_NAME_UI_FOLDER_PATH,
                        index,
                        title);
                    
                    GameObject caption = CreateCaption(illustrationDatas.CaptionBase.GameObject, CAPTION_FOLDER_PATH,
                        illustrationData.ID.ToString(),
                        illustrationData.Title,
                        illustrationData.Description,
                        illustrationData.Members);
                    
                    CreateIllustrationObject(illustrationData,
                        illustration,
                        caption,
                        illustrationDatas.Offset);
                    
                    continue;
                }

                CreateIllustrationObject(illustrationData, illustration);
            }
        }
    }

    /// <summary>
    /// フレームの名前のリストを取得
    /// </summary>
    /// <param name="illustrationDatas"></param>
    /// <returns></returns>
    private List<string> GetFrameNameList(List<IllustrationObject> illustrationDatas)
    {
        List<string> valueList = new List<string>();

        foreach (var illustrationData in illustrationDatas)
        {
            if (illustrationData.GameObject.name == "")
            {
                continue;
            }

            valueList.Add(illustrationData.GameObject.name);
        }

        return valueList;
    }

    /// <summary>
    /// イラストプレハブ生成
    /// </summary>
    /// <param name="illustration"> イラストデータ </param>
    /// <param name="frame"> 設定するオブジェクト </param>
    /// <param name="material"> 設定するマテリアル </param>
    private GameObject CreateIllustrationPrefab(IllustrationData illustration, IllustrationObject frame, Material material)
    {
        GameObject prefab = GetNewPrefab(frame.GameObject);
        var illustrationObject = prefab.GetComponent<IllustrationObject>();
        illustrationObject.SetIllustration(illustration.ID, illustration.Image, material);
        SetNameUI(illustrationObject, illustration.ID.ToString(), illustration.Title);
        PrefabUtility.SaveAsPrefabAsset(prefab, ILLUSTRATION_FOLDER_PATH + "/" 
            + illustration.ID.ToString() + "_" + illustration.Title
            + PREFAB_EXTENSION);

        return prefab;
    }

    /// <summary>
    /// イラストオブジェクト生成
    /// </summary>
    /// <param name="illustrationData"> イラストデータ </param>
    /// <param name="illustration"> イラスト </param>
    /// <param name="caption"> キャプション </param>
    /// <param name="offset"> キャプションとイラストの距離 </param>
    private void CreateIllustrationObject(IllustrationData illustrationData, GameObject illustration, GameObject caption, float offset)
    {
        GameObject obj = GetNewObject(illustrationData.ID.ToString() + "_" + illustrationData.Title);
        illustration.transform.SetParent(obj.transform);

        caption.transform.SetParent(obj.transform);
        SetPosition(illustration, caption, illustrationData.CaptionDirection, offset);

        PrefabUtility.SaveAsPrefabAsset(obj, ILLUSTRATION_OBJECT_FOLDER_PATH + "/" + illustrationData.ID.ToString() + "_" + illustrationData.Title + PREFAB_EXTENSION);
        DestroyImmediate(obj);
        DestroyImmediate(illustration);
        DestroyImmediate(caption);
    }

    /// <summary>
    /// イラストオブジェクト生成
    /// </summary>
    /// <param name="illustrationData"> イラストデータ </param>
    /// <param name="illustration"> イラスト </param>
    private void CreateIllustrationObject(IllustrationData illustrationData, GameObject illustration)
    {
        GameObject obj = GetNewObject(illustrationData.ID.ToString() + "_" + illustrationData.Title);
        illustration.transform.SetParent(obj.transform);

        PrefabUtility.SaveAsPrefabAsset(obj, ILLUSTRATION_OBJECT_FOLDER_PATH + "/" + illustrationData.ID.ToString() + "_" + illustrationData.Title + PREFAB_EXTENSION);
        DestroyImmediate(obj);
        DestroyImmediate(illustration);
    }
}
#endif

/// <summary>
/// イラストのデータ
/// </summary>
[System.Serializable]
public class IllustrationData: IArtData
{
    private int _id = -1;
    /// <summary>
    /// イラスト番号
    /// </summary>
    public int ID => _id;
    [Header("イラスト名")]
    [SerializeField]
    private string _title = "";
    /// <summary>
    /// イラスト名
    /// </summary>
    public string Title => _title;
    [Header("イラスト画像")]
    [SerializeField]
    private Sprite _image = null;
    /// <summary>
    /// イラストの画像
    /// </summary>
    public Sprite Image => _image;
    [Header("製作者")]
    [SerializeField]
    private string[] _members = new string[0];
    /// <summary>
    /// 製作者
    /// </summary>
    public string[] Members => _members;
    [Header("イラストの説明")]
    [TextArea(1, 10)]
    [SerializeField]
    private string _description = "";
    /// <summary>
    /// イラストの説明
    /// </summary>
    public string Description => _description;
    [Header("フレームの種類")]
    [SerializeField]
    private FrameType _frameType = FrameType.None;
    /// <summary>
    /// フレームの種類
    /// </summary>
    public FrameType FrameType => _frameType;
    [Header("キャプションの位置")]
    [SerializeField]
    private Direction _captionDirection = Direction.None;
    /// <summary>
    /// キャプションの位置
    /// </summary>
    public Direction CaptionDirection => _captionDirection;

    public void SetIndex(int index)
    {
        if(index < 0)
        {
            return;
        }

        _id = index;
    }
}



///// <summary>
///// 同じ名前のイラストのプレハブがあるか調べる
///// </summary>
///// <param name="files"> 探すフォルダのプレハブ </param>
///// <param name="name"> 対象のイラスト名 </param>
///// <param name="index"> 対象のイラストの番号 </param>
///// <returns> 同じイラストのプレハブがあるか </returns>
//private bool SearchDistinctIllustlationPrefab(string[] files, string name, int index)
//{
//    if (!SearchDistinctFile(files, name))
//    {
//        return false; 
//    }

//    var illustrationObject = AssetDatabase.LoadAssetAtPath<GameObject>(ILLUSTRATION_FOLDER_PATH + "/" + name + PREFAB_EXTENSION);
//    if (illustrationObject == null)
//    {
//        return false;
//    }

//    var illustration = illustrationObject.GetComponent<IllustrationObject>();
//    if (illustration == null)
//    {
//        return false;
//    }

//    if (illustration.Index != index)
//    {
//        return false;
//    }

//    return true;
//}

///// <summary>
///// 同じマテリアルがあるか調べる
///// </summary>
///// <param name="files"> 探すフォルダのマテリアル </param>
///// <param name="name"> マテリアルの名前 </param>
///// <returns> 同じマテリアルがあるか </returns>
//private bool SearchDistinctMaterial(string[] files, string name)
//{
//    if(!SearchDistinctFile(files, name))
//    {
//        return false;
//    }

//    var materialPath = MATERIAL_FOLDER_PATH + "/"+ name + MATERIAL_EXTENSION;
//    var materialExists = File.Exists(materialPath);
//    if(!materialExists)
//    {
//        return false;
//    }

//    return true;
//}

///// <summary>
///// 同じ名前のキャプションがあるか調べる
///// </summary>
///// <param name="files"> 探すフォルダのキャプション </param>
///// <param name="name"> キャプション </param>
///// <returns></returns>
//private bool SearchDistinctCaption(string[] files, string name)
//{
//    return SearchDistinctFile(files, name);
//}