using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
}

#if UNITY_EDITOR
[CustomEditor(typeof(IllustrationDatas))]
public class IllustrationDatasEditor : Editor
{
    private const string ENUM_FILE_NAME = "FrameType";
    private const string ENUM_FOLDER_PATH = "Assets/0_coding/1_Data/Illustrations";
    private const string PREFAB_FOLDER_PATH = "Assets/3_2D/0_Prefabs/Illustration";
    private const string MATERIAL_FOLDER_PATH = "Assets/3_2D/1_Materials";
    private const string MATERIAL_PATH = "Universal Render Pipeline/Lit";
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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_dataList"));
        serializedObject.ApplyModifiedProperties();

        //Enum生成
        if(createEnumButton)
        {
            IllustrationDatas illustrationDatas = (IllustrationDatas)target;
            string enumCode = GetEnumCode(GetFrameNameList(illustrationDatas.Frame));
            string savePath = ENUM_FOLDER_PATH + "/" + ENUM_FILE_NAME + ".cs";

            File.WriteAllText(savePath, enumCode);
            AssetDatabase.Refresh();
        }

        //プレハブ生成
        if (createPrefabButton)
        {
            IllustrationDatas illustrationDatas = (IllustrationDatas)target;
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

            string[] prefabFiles = Directory.GetFiles(PREFAB_FOLDER_PATH);
            string[] materialFiles = Directory.GetFiles(MATERIAL_FOLDER_PATH);

            foreach (var illustrationData in illustrationDatasList)
            {
                string index = illustrationData.Index.ToString();
                string title = illustrationData.Title;

                if(title == ""
                    || illustrationData.Image == null
                    || (SearchDistinctIllustlationPrefab(prefabFiles, index + "_" + title, illustrationData.Index)
                        && SearchDistinctMaterial(materialFiles, index + "_" + title)))
                {
                    continue;
                }

                Material material = CreateMaterial(illustrationData);
                CreatePrefab(illustrationData, illustrationDatas.Frame[(int)illustrationData.FrameType], material);
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
    /// 同じ名前のイラストのプレハブがあるか調べる
    /// </summary>
    /// <param name="files"> 探すフォルダのプレハブ </param>
    /// <param name="name"> 対象のイラスト名 </param>
    /// <param name="index"> 対象のイラストの番号 </param>
    /// <returns> 同じイラストのプレハブがあるか </returns>
    private bool SearchDistinctIllustlationPrefab(string[] files, string name, int index)
    {
        if (!SearchDistinctFile(files, name))
        {
            return false; 
        }

        var illustrationObject = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_FOLDER_PATH + "/" + name + PREFAB_EXTENSION);
        if (illustrationObject == null)
        {
            return false;
        }

        var illustration = illustrationObject.GetComponent<IllustrationObject>();
        if (illustration == null)
        {
            return false;
        }

        if (illustration.Index != index)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 同じマテリアルがあるか調べる
    /// </summary>
    /// <param name="files"> 探すフォルダのマテリアル </param>
    /// <param name="name"> マテリアルの名前 </param>
    /// <returns> 同じマテリアルがあるか </returns>
    private bool SearchDistinctMaterial(string[] files, string name)
    {
        if(!SearchDistinctFile(files, name))
        {
            return false;
        }

        var materialPath = MATERIAL_FOLDER_PATH + "/" + name + MATERIAL_EXTENSION;
        var materialExists = File.Exists(materialPath);
        if(!materialExists)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// プレハブ生成
    /// </summary>
    /// <param name="illustration"> イラストデータ </param>
    /// <param name="frame"> 設定するオブジェクト </param>
    /// <param name="material"> 設定するマテリアル </param>
    private void CreatePrefab(IllustrationData illustration, IllustrationObject frame, Material material)
    {
        GameObject prefab = GetNewPrefab(frame.GameObject);
        prefab.GetComponent<IllustrationObject>().SetIllustration(illustration.Index, illustration.Image, material);
        PrefabUtility.SaveAsPrefabAsset(prefab, PREFAB_FOLDER_PATH + "/" + illustration.Index.ToString() + "_" + illustration.Title + PREFAB_EXTENSION);
        DestroyImmediate(prefab);
    }

    /// <summary>
    /// 新しいプレハブを取得
    /// </summary>
    /// <param name="frame"> フレーム </param>
    /// <returns></returns>
    private GameObject GetNewPrefab(GameObject frame)
    {
        return PrefabUtility.InstantiatePrefab(frame) as GameObject;
    }

    /// <summary>
    /// マテリアルを作成
    /// </summary>
    /// <param name="name"> 名前 </param>
    /// <param name="illustlation"> 設定するイラスト </param>
    private Material CreateMaterial(IllustrationData illustration)
    {
        Material material = GetNewMaterial(illustration.Image.texture);

        AssetDatabase.CreateAsset(material, MATERIAL_FOLDER_PATH + "/" + illustration.Index.ToString() + "_" + illustration.Title + MATERIAL_EXTENSION);
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