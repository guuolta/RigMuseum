using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectBase<T> : ScriptableObject
{
    [SerializeField]
    private List<T> _dataList = new List<T>();

    /// <summary>
    /// データを取得
    /// </summary>
    /// <param name="index"> 要素番号 </param>
    /// <returns></returns>
    public T GetGameData(int index)
    {
        if (_dataList.Count == 0)
        {
            Debug.Log("スクリプタブルオブジェクトに値を設定してください");
            return default(T);
        }

        if (index >= _dataList.Count)
        {
            Debug.Log("out of index");
            return default(T);
        }

        return _dataList[index];
    }

    /// <summary>
    /// データリストを取得
    /// </summary>
    /// <returns></returns>
    public List<T> GetDataList()
    {
        return _dataList;
    }

    /// <summary>
    /// データ数を取得
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        return _dataList.Count;
    }
}
