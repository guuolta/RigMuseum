using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresenter
{
    /// <summary>
    /// UIを表示
    /// </summary>
    /// <returns></returns>
    public UniTask ShowAsync();

    /// <summary>
    /// UIを消す
    /// </summary>
    /// <returns></returns>
    public UniTask HideAsync();
}
