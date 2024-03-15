using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;

public class ArtDatasEditorBase : Editor
{
    protected const string MATERIAL_PATH = "Universal Render Pipeline/Lit";

    protected const string NAME_UI_FILE_NAME_ENDING = "NameUI";
    protected const string CAPTION_NAME_UI_FILE_NAME_ENDING = "CaptionNameUI";

    protected const string PREFAB_EXTENSION = ".prefab";
    protected const string MATERIAL_EXTENSION = ".mat";

    private int _listCount = 0;

    /// <summary>
    /// Enum生成
    /// </summary>
    /// <param name="enumList"> 設定するEnumリスト </param>
    /// <param name="path"> Enumのファイルを格納するフォルダのパス </param>
    /// <param name="enumFileName"> Enumのファイル名 </param>
    /// <returns></returns>
    protected void CreateEnum(List<string> enumList, string path ,string enumFileName)
    {
        string enumCode = GetEnumCode(enumList, enumFileName);
        string savePath = path + "/" + enumFileName + ".cs";

        File.WriteAllText(savePath, enumCode);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Enumのコードを取得
    /// </summary>
    /// <param name="enumList"> 設定するEnumリスト </param>
    /// <param name="enumFileName"> Enumのファイル名 </param>
    /// <returns></returns>
    private string GetEnumCode(List<string> enumList, string enumFileName)
    {
        string enumCode = "public enum " + enumFileName + "\n{\n";

        int i;
        for (i = 0; i < enumList.Count - 1; i++)
        {
            enumCode += "    " + enumList[i] + ",\n";
        }
        enumCode += "    " + enumList[i] + "\n}";

        return enumCode;
    }

    /// <summary>
    /// 番号設定
    /// </summary>
    /// <typeparam name="TDatas"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="datas"></param>
    protected void SetIndexToData<TDatas, TData>(TDatas datas)
        where TDatas : ScriptableObjectBase<TData>
        where TData : IArtData
    {
        var datasList = datas.GetDataList();
        int count = datas.GetCount();
        
        if (_listCount != count)
        {
            _listCount = count;
            for (int i = 0; i < count; i++)
            {
                datasList[i].SetIndex(i);
            }
        }
    }


    /// <summary>
    /// 同じ名前のファイルがあるか確かめる
    /// </summary>
    /// <param name="files"> 探すフォルダのファイル </param>
    /// <param name="name"> 対象のファイル名 </param>
    /// <returns> 同じファイルがあるか </returns>
    protected bool SearchDistinctFile(string[] files, string name)
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
    /// キャプション生成
    /// </summary>
    /// <param name="captionBase"> キャプションのオブジェクトベース </param>
    /// <param name="path"> キャプションオブジェクトを格納するフォルダのパス </param>
    /// <param name="id"> ID </param>
    /// <param name="title"> タイトル </param>
    /// <param name="description"> 説明 </param>
    /// <param name="members"> 製作者 </param>
    protected GameObject CreateCaption(GameObject captionBase, string path, string id, string title, string description, string[] members)
    {
        var caption = GetNewPrefab(captionBase).GetComponent<ArtCaptionObject>();
        SetCaptionNameUI(caption, id.ToString(), title);

        var captionUI = caption.CaptionUI;
        captionUI.SetTitleText(title);
        captionUI.SetExplain(description);
        captionUI.SetAuthorText(members);

        PrefabUtility.SaveAsPrefabAsset(caption.GameObject, path + "/"
            + id.ToString() + "_" + title + "Caption"
            + PREFAB_EXTENSION);

        return caption.GameObject;
    }

    /// <summary>
    /// 名前のUI生成
    /// </summary>
    /// <param name="nameUIBase"> 名前UIベース </param>
    /// <param name="path"> 名前UIを格納するフォルダのパス </param>
    /// <param name="id"> 番号 </param>
    /// <param name="title"> タイトル </param>
    protected void CreateNmaeUI(ObjectNamePanelPresenter nameUIBase, string path, string id, string title)
    {
        GameObject nameUI = GetNewPrefab(nameUIBase.gameObject);
        nameUI.GetComponent<ObjectNamePanelPresenter>().SetText(title);

        PrefabUtility.SaveAsPrefabAsset(nameUI, path + "/" + id + "_" + title + NAME_UI_FILE_NAME_ENDING + PREFAB_EXTENSION);
        DestroyImmediate(nameUI);
    }

    /// <summary>
    /// キャプションの名前UI生成
    /// </summary>
    /// <param name="nameUIBase"> 名前UIベース </param>
    /// <param name="path"> 名前UIを格納するフォルダのパス </param>
    /// <param name="id"> 番号 </param>
    /// <param name="title"> タイトル </param>
    protected void CreateCaptionNameUI(ObjectNamePanelPresenter nameUIBase, string path, string id, string title)
    {
        GameObject nameUI = GetNewPrefab(nameUIBase.gameObject);
        nameUI.GetComponent<ObjectNamePanelPresenter>().SetText(title + "\nキャプション");

        PrefabUtility.SaveAsPrefabAsset(nameUI, path + "/" + id + "_" + title + CAPTION_NAME_UI_FILE_NAME_ENDING + PREFAB_EXTENSION);
        DestroyImmediate(nameUI);
    }

    /// <summary>
    /// 新しいオブジェクトを取得
    /// </summary>
    /// <param name="name"> オブジェクト名 </param>
    /// <returns></returns>
    protected GameObject GetNewObject(string name)
    {
        GameObject obj = EditorUtility.CreateGameObjectWithHideFlags(name, HideFlags.HideInHierarchy);

        return obj;
    }

    /// <summary>
    /// 新しいプレハブを取得
    /// </summary>
    /// <param name="baseObejct"> ベースオブジェクト </param>
    /// <returns></returns>
    protected GameObject GetNewPrefab(GameObject baseObejct)
    {
        return PrefabUtility.InstantiatePrefab(baseObejct) as GameObject;
    }

    /// <summary>
    /// 名前UIを設定
    /// </summary>
    /// <param name="target"> 設定するオブジェクト </param>
    /// <param name="nameUI"> 名前 </param>
    protected void SetNameUI(TouchObjectBase target, string index, string name)
    {
        target.SetNameUI(index + "_" + name + NAME_UI_FILE_NAME_ENDING);
    }

    /// <summary>
    /// キャプションの名前UIを設定
    /// </summary>
    /// <param name="target"> 設定するオブジェクト </param>
    /// <param name="nameUI"> 名前 </param>
    protected void SetCaptionNameUI(TouchObjectBase target, string index, string name)
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
    protected void SetPosition(GameObject refObj, GameObject targetObj, Direction direction, float offset)
    {
        var renderer = refObj.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            var child = refObj.transform.GetChild(0);
            if (child == null)
            {
                return;
            }

            renderer = child.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                Debug.Log("Renderer is not found");
                return;
            }
        }

        switch (direction)
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
    protected Vector3 GetMovePos(Vector3 refPos, Vector3 direction, float size, float offset)
    {
        return refPos + direction * (size / 2 + offset);
    }

    /// <summary>
    /// マテリアルを作成
    /// </summary>
    /// <param name="path"> マテリアルを格納するフォルダのパス </param>
    /// <param name="texture"> 設定する画像テクスチャ </param>
    /// <param name="title"> タイトル </param>
    /// <param name="id"> ID </param>
    protected Material CreateMaterial(string path, Texture2D texture, string id, string title)
    {
        Material material = GetNewMaterial(texture);

        AssetDatabase.CreateAsset(material, path + "/"
            + id + "_" + title
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
    protected Material GetNewMaterial(Texture2D texture)
    {
        Material material = new Material(Shader.Find(MATERIAL_PATH));
        material.mainTexture = texture;
        material.SetFloat("_WorkflowMode", 0);

        return material;
    }
}
#endif

public enum Direction
{
    None,
    Left,
    Right,
    Up,
    Down
}