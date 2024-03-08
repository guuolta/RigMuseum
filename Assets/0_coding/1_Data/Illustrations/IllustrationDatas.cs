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
public class IllustrationDatasEditor : Editor
{
    private const string MATERIAL_PATH = "Universal Render Pipeline/Lit";

    private const string ENUM_FILE_NAME = "FrameType";

    private const string NAME_UI_FILE_NAME_ENDING = "NameUI";
    private const string CAPTION_NAME_UI_FILE_NAME_ENDING = "CaptionNameUI";

    private const string ENUM_FOLDER_PATH = "Assets/0_coding/1_Data/Illustrations";
    private const string ILLUSTRATION_FOLDER_PATH = "Assets/3_2D/0_Prefabs/Illustration";
    private const string CAPTION_FOLDER_PATH = "Assets/3_2D/0_Prefabs/Caption";
    private const string ILLUSTRATION_OBJECT_FOLDER_PATH = "Assets/3_2D/0_Prefabs/IllustrationObjects";
    private const string MATERIAL_FOLDER_PATH = "Assets/3_2D/1_Materials";
    private const string ILLUSTRATION_NAME_UI_FOLDER_PATH = "Assets/5_source/Prefabs/UI/Panel/ObjectName/IllustrationNamePanels";
    private const string ILLUSTRATION_CAPTION_NAME_UI_FOLDER_PATH = "Assets/5_source/Prefabs/UI/Panel/ObjectName/IllustrationCaptionNamePanels";

    private const string PREFAB_EXTENSION = ".prefab";
    private const string MATERIAL_EXTENSION = ".mat";
    
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
            string enumCode = GetEnumCode(GetFrameNameList(illustrationDatas.Frame));
            string savePath = ENUM_FOLDER_PATH + "/" + ENUM_FILE_NAME + ".cs";

            File.WriteAllText(savePath, enumCode);
            AssetDatabase.Refresh();
        }

        //プレハブ生成
        if (createPrefabButton)
        {
            var illustrationDatas = (IllustrationDatas)target;
            var illustrationDatasList = illustrationDatas.GetDataList();
            int count = illustrationDatas.GetCount();
            //イラスト番号設定
            if(_listCount != count)
            {
                _listCount = count;
                for (int i = 0; i < count; i++)
                {
                    illustrationDatasList[i].SetIndex(i);
                }
            }

            string[] prefabFiles = Directory.GetFiles(ILLUSTRATION_FOLDER_PATH);
            string[] materialFiles = Directory.GetFiles(MATERIAL_FOLDER_PATH);
            string[] captionFiles = Directory.GetFiles(CAPTION_FOLDER_PATH);

            foreach (var illustrationData in illustrationDatasList)
            {
                string index = illustrationData.Index.ToString();
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

                Material material = CreateMaterial(illustrationData);
                CreateNmaeUI(index, title, illustrationDatas.NameUIBase);
                GameObject illustration = CreateIllustrationPrefab(illustrationData, illustrationDatas.Frame[(int)illustrationData.FrameType], material);
                if (illustrationData.CaptionDirection != Direction.None)
                {
                    CreateCaptionNameUI(index, title, illustrationDatas.NameUIBase);
                    GameObject caption = CreateCaption(illustrationData, illustrationDatas.CaptionBase.GameObject);
                    CreateIllustrationObject(illustrationData, illustration, caption, illustrationDatas.Offset);
                    
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
    /// Enumのコードを取得
    /// </summary>
    /// <param name="enumList"> 設定するEnumリスト </param>
    /// <returns></returns>
    private string GetEnumCode(List<string> enumList)
    {
        string enumCode = "public enum " + ENUM_FILE_NAME + "\n{\n";
        
        int i;
        for(i = 0; i < enumList.Count-1; i++)
        {
            enumCode += "    " + enumList[i] + ",\n";
        }
        enumCode += "    " + enumList[i] + "\n}";

        return enumCode;
    }

    /// <summary>
    /// 同じ名前のファイルがあるか確かめる
    /// </summary>
    /// <param name="files"> 探すフォルダのファイル </param>
    /// <param name="name"> 対象のファイル名 </param>
    /// <returns> 同じファイルがあるか </returns>
    private bool SearchDistinctFile(string[] files, string name)
    {
        foreach (string file in files)
        {
            if (file.Contains(name))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// プレハブ生成
    /// </summary>
    /// <param name="illustration"> イラストデータ </param>
    /// <param name="frame"> 設定するオブジェクト </param>
    /// <param name="material"> 設定するマテリアル </param>
    private GameObject CreateIllustrationPrefab(IllustrationData illustration, IllustrationObject frame, Material material)
    {
        GameObject prefab = GetNewPrefab(frame.GameObject);
        var illustrationObject = prefab.GetComponent<IllustrationObject>();
        illustrationObject.SetIllustration(illustration.Index, illustration.Image, material);
        SetNameUI(illustrationObject, illustration.Index.ToString(), illustration.Title);
        PrefabUtility.SaveAsPrefabAsset(prefab, ILLUSTRATION_FOLDER_PATH + "/" 
            + illustration.Index.ToString() + "_" + illustration.Title
            + PREFAB_EXTENSION);

        return prefab;
    }

    /// <summary>
    /// キャプション生成
    /// </summary>
    /// <param name="illustration"> イラストデータ </param>
    /// <param name="captionBase"> キャプションのオブジェクトベース </param>
    private GameObject CreateCaption(IllustrationData illustration, GameObject captionBase)
    {
        var caption = GetNewPrefab(captionBase).GetComponent<ArtCaptionObject>();
        SetCaptionNameUI(caption, illustration.Index.ToString(), illustration.Title);

        var captionUI = caption.CaptionUI;
        captionUI.SetTitleText(illustration.Title);
        captionUI.SetExplain(illustration.Description);
        captionUI.SetAuthorText(illustration.Members);

        PrefabUtility.SaveAsPrefabAsset(caption.GameObject, CAPTION_FOLDER_PATH + "/"
            + illustration.Index.ToString() + "_" + illustration.Title + "Caption"
            + PREFAB_EXTENSION);

        return caption.GameObject;
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
        GameObject obj = GetNewObject(illustrationData.Index.ToString() + "_" + illustrationData.Title);
        illustration.transform.SetParent(obj.transform);

        caption.transform.SetParent(obj.transform);
        SetPosition(illustration, caption, illustrationData.CaptionDirection, offset);

        PrefabUtility.SaveAsPrefabAsset(obj, ILLUSTRATION_OBJECT_FOLDER_PATH + "/" + illustrationData.Index.ToString() + "_" + illustrationData.Title + PREFAB_EXTENSION);
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
        GameObject obj = GetNewObject(illustrationData.Index.ToString() + "_" + illustrationData.Title);
        illustration.transform.SetParent(obj.transform);

        PrefabUtility.SaveAsPrefabAsset(obj, ILLUSTRATION_OBJECT_FOLDER_PATH + "/" + illustrationData.Index.ToString() + "_" + illustrationData.Title + PREFAB_EXTENSION);
        DestroyImmediate(obj);
        DestroyImmediate(illustration);
    }

    /// <summary>
    /// 名前のUI生成
    /// </summary>
    /// <param name="index"> イラスト番号 </param>
    /// <param name="name"> イラスト名 </param>
    /// <param name="nameUIBase"> 名前UIベース </param>
    private void CreateNmaeUI(string index, string name, ObjectNamePanelPresenter nameUIBase)
    {
        GameObject nameUI = GetNewPrefab(nameUIBase.gameObject);
        nameUI.GetComponent<ObjectNamePanelPresenter>().SetText(name);

        PrefabUtility.SaveAsPrefabAsset(nameUI, ILLUSTRATION_NAME_UI_FOLDER_PATH + "/" + index + "_" + name + NAME_UI_FILE_NAME_ENDING + PREFAB_EXTENSION);
        DestroyImmediate(nameUI);
    }

    /// <summary>
    /// キャプションの名前UI生成
    /// </summary>
    /// <param name="index"> イラスト番号 </param>
    /// <param name="name"> イラスト名 </param>
    /// <param name="nameUIBase"> 名前UIベース </param>
    private void CreateCaptionNameUI(string index, string name, ObjectNamePanelPresenter nameUIBase)
    {
        GameObject nameUI = GetNewPrefab(nameUIBase.gameObject);
        nameUI.GetComponent<ObjectNamePanelPresenter>().SetText(name + "\nキャプション");

        PrefabUtility.SaveAsPrefabAsset(nameUI, ILLUSTRATION_CAPTION_NAME_UI_FOLDER_PATH + "/" + index + "_" + name + CAPTION_NAME_UI_FILE_NAME_ENDING + PREFAB_EXTENSION);
        DestroyImmediate(nameUI);
    }

    /// <summary>
    /// 新しいオブジェクトを取得
    /// </summary>
    /// <param name="name"> オブジェクト名 </param>
    /// <returns></returns>
    private GameObject GetNewObject(string name)
    {
        GameObject obj = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);

        return obj;
    }

    /// <summary>
    /// 新しいプレハブを取得
    /// </summary>
    /// <param name="baseObejct"> ベースオブジェクト </param>
    /// <returns></returns>
    private GameObject GetNewPrefab(GameObject baseObejct)
    {
        return PrefabUtility.InstantiatePrefab(baseObejct) as GameObject;
    }

    /// <summary>
    /// 名前UIを設定
    /// </summary>
    /// <param name="target"> 設定するオブジェクト </param>
    /// <param name="nameUI"> 名前 </param>
    private void SetNameUI(TouchObjectBase target, string index, string name)
    {
        target.SetNameUI(index + "_" + name + NAME_UI_FILE_NAME_ENDING);
    }

    /// <summary>
    /// キャプションの名前UIを設定
    /// </summary>
    /// <param name="target"> 設定するオブジェクト </param>
    /// <param name="nameUI"> 名前 </param>
    private void SetCaptionNameUI(TouchObjectBase target, string index, string name)
    {
        target.SetNameUI(index + "_" + name + CAPTION_NAME_UI_FILE_NAME_ENDING);
    }

    /// <summary>
    /// 対象のオブジェクトの位置を設定する
    /// </summary>
    /// <param name="refObj"> 基準オブジェクト </param>
    /// <param name="targetObj"> 対象のオブジェクト </param>
    /// <param name="direction"> 方向 </param>
    /// <param name="offset"> 基準と対象の距離 </param>
    private void SetPosition(GameObject refObj, GameObject targetObj, Direction direction, float offset)
    {
        var renderer = refObj.GetComponent<MeshRenderer>();
        if(renderer == null)
        {
            var child = refObj.transform.GetChild(0);
            if(child == null)
            {
                return;
            }

            renderer = child.GetComponent<MeshRenderer>();
            if(renderer == null)
            {
                Debug.Log("Renderer is not found");
                return;
            }
        }

        switch(direction)
        {
            case Direction.Left:
                targetObj.transform.position = GetMovePos(refObj.transform.position,
                    -targetObj.transform.right,
                    renderer.bounds.size.x,
                    offset);
                break;
            case Direction.Right:
                targetObj.transform.position = GetMovePos(refObj.transform.position,
                    targetObj.transform.right,
                    renderer.bounds.size.x,
                    offset); 
                break;
            case Direction.Up:
                targetObj.transform.position = GetMovePos(refObj.transform.position,
                    targetObj.transform.up,
                    renderer.bounds.size.y,
                    offset); 
                break;
            case Direction.Down:
                targetObj.transform.position = GetMovePos(refObj.transform.position,
                    -targetObj.transform.up,
                    renderer.bounds.size.y,
                    offset);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 移動後の位置を取得
    /// </summary>
    /// <param name="refPos"> 基準 </param>
    /// <param name="direction"> 方向 </param>
    /// <param name="size"> 基準オブジェクトの大きさ </param>
    /// <param name="offset"> 基準と対象の距離 </param>
    /// <returns></returns>
    private Vector3 GetMovePos(Vector3 refPos, Vector3 direction, float size, float offset)
    {
        return refPos + direction * (size / 2 + offset);
    }

    /// <summary>
    /// マテリアルを作成
    /// </summary>
    /// <param name="name"> 名前 </param>
    /// <param name="illustlation"> 設定するイラスト </param>
    private Material CreateMaterial(IllustrationData illustration)
    {
        Material material = GetNewMaterial(illustration.Image.texture);

        AssetDatabase.CreateAsset(material, MATERIAL_FOLDER_PATH + "/"
            + illustration.Index.ToString() + "_" + illustration.Title
            + MATERIAL_EXTENSION);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        return material;
    }

    /// <summary>
    /// 新しいマテリアルを取得
    /// </summary>
    /// <param name="texture"> 設定するテクスチャ </param>
    /// <returns></returns>
    private Material GetNewMaterial(Texture2D texture)
    {
        Material material = new Material(Shader.Find(MATERIAL_PATH));
        material.mainTexture = texture;
        material.SetFloat("_WorkflowMode", 0);

        return material;
    }
}
#endif

/// <summary>
/// イラストのデータ
/// </summary>
[System.Serializable]
public class IllustrationData
{
    private int _index = -1;
    /// <summary>
    /// イラスト番号
    /// </summary>
    public int Index => _index;
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

    /// <summary>
    /// イラスト番号を設定
    /// </summary>
    /// <param name="index"> イラスト番号 </param>
    public void SetIndex(int index)
    {
        if(index < 0)
        {
            return;
        }

        _index = index;
    }
}

public enum Direction
{
    None,
    Left,
    Right,
    Up,
    Down
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