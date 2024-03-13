using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class NameUIPanelManager : ObjectBase
{
    [Header("フォルダのパス(Assets/5_source/Prefabs/UI/Panel/ObjectName/フォルダ名)")]
    [SerializeField]
    private string _folderPath = "Assets/5_source/Prefabs/UI/Panel/ObjectName/";
    public string FolderPath => _folderPath;
}

#if UNITY_EDITOR
[CustomEditor(typeof(NameUIPanelManager))]
public class NameUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Create NameUI"))
        {
            var nameUIManager = (NameUIPanelManager)target;
            var files = Directory.GetFiles(nameUIManager.FolderPath, "*.prefab");

            if(files.Length == 0)
            {
                Debug.LogError("There is no prefab in the folder.");
                return;
            }

            foreach (var file in files)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
                if(GameObject.Find(prefab.name) != null)
                {
                    continue;
                }

                var nameUI = (GameObject)prefab;
                var obj = Instantiate(nameUI, nameUIManager.transform);
                obj.name = obj.name.Replace("(Clone)", "");
            }

            Debug.Log("Create NameUI");
        }

        base.OnInspectorGUI();
    }
}
#endif