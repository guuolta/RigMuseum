using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNamePanelPresenter : PresenterBase<ObjectNamePanelView>
{
    public void SetText(string name)
    {
        View.NameText.text = name;
    }
}
