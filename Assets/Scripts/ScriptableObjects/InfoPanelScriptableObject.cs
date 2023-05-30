using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoPanelScriptableObject", menuName = "shoutr labs/Create InfoPanel asset", order = 1)]
public class InfoPanelScriptableObject : ScriptableObject
{
    public List<InfoPanelItem> InfoPanelItems = new List<InfoPanelItem>();
}

[Serializable]
public class InfoPanelItem
{
    public Sprite MainImage;
    [TextArea(1, 4)]
    public string Captions;
    public string RightWrongTitle;
    public string Title;
    [TextArea(3,10)]
    public string Description;
}
