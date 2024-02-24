using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelPresenter
{
    public UniTask OpenPanelAsync();
    public UniTask ClosePanelAsync();
}
