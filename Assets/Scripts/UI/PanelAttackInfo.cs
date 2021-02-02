using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAttackInfo : MonoBehaviour
{
    private struct AtkInfo
    {
        public string DamageType;
        public float Value;
        public object Param;

        public override string ToString() { return $"{DamageType},{Value},[{Param}]"; }
    }

    [SerializeField] private Text _info;
    [SerializeField] private ScrollRect _scroll;

    private List<AtkInfo> _infoList;

    private void Awake() { _infoList = new List<AtkInfo>(); }

    public void AddInfo(string damageType, float value, object param)
    {
        _infoList.Add(new AtkInfo {DamageType = damageType, Param = param, Value = value});
        var txt = _info.text;
        if (_infoList.Count == 1)
        {
            txt += $"1:{_infoList[0].ToString()}";
        }
        else
        {
            txt += $"\n{_infoList.Count}:{_infoList[0].ToString()}";
        }

        _info.text = txt;
        Canvas.ForceUpdateCanvases();
        _scroll.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    public void OnClearBtnPress()
    {
        _infoList.Clear();
        _info.text = string.Empty;
        Canvas.ForceUpdateCanvases();
        _scroll.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}