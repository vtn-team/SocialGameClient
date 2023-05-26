using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using MD;
using Unity.VisualScripting;

/// <summary>
/// ローカライズテキスト表示用エディタ拡張
/// </summary>
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LocalizeTextAttribute))]
public class LocalizeTextAttributeDrawer : PropertyDrawer
{
    string[] _array = null;
    List<string> _list = null;
    int _selected = 0;

    VisualElement _element = null;
    VisualElement _container = null;
    bool _isInit = false;

    void ContainerBuild(SerializedProperty property)
    {
        if (_isInit) return;

        _element.Clear();

        var attr = attribute as LocalizeTextAttribute;
        var master = attr.Master;
        
        _list = new List<string>();
        _list.Add("None");
        _list = _list.Concat(MasterData.GetTextKeys()).ToList();

        _array = _list.ToArray();
        _selected = _list.IndexOf(property.stringValue);
        if (_selected == -1) _selected = 0;

        var text = new TextField();
        text.isReadOnly = true;

        bool isInit = true;
        var popup = new PopupField<string>(property.name.Replace("_",""), _list, _selected, (string s) =>
        {
            //tags[idx].Index = tags[idx].TagArray.ToList().IndexOf(s);
            //BuildContainer(property);
            if (s == "None")
            {
                text.style.visibility = Visibility.Hidden;
                text.style.height = 0;
                text.value = "";

                property.stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
                return s;
            }

            text.style.visibility = Visibility.Visible;
            text.style.textOverflow = TextOverflow.Ellipsis;
            text.style.alignItems = Align.Stretch;
            text.value = MasterData.GetLocalizedText(s);
            text.MarkDirtyRepaint();

            if (isInit) return s;

            property.stringValue = s;
            property.serializedObject.ApplyModifiedProperties();
            ContainerBuild(property);

            return s;
        });

        _element.Add(popup);
        VisualElement ct = new VisualElement();
        ct.style.flexDirection = FlexDirection.Row;
        _element.Add(text);
        _element.MarkDirtyRepaint();

        isInit = false;
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _element = new VisualElement();
        _container = new VisualElement();

        if (!MasterData.Instance.IsSetupComplete)
        {
            _element.Clear();
            _element.Add(new Label() { text = "マスタデータを読み込み中です…" });
            _element.Add(new Button(() =>
            {
                ContainerBuild(property);
            }) { text = "再読み込み" });
            //label.style.backgroundColor = new StyleColor(Color.gray);
            MasterData.Instance.Setup(() =>
            {
                ContainerBuild(property);
            },true);
            return _element;
        }

        ContainerBuild(property);
        return _element;
    }
}
#endif