using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SearchableTagDropdown : AdvancedDropdown
{
    private readonly Action<string> onSelected;

    public SearchableTagDropdown(AdvancedDropdownState state, Action<string> onSelected) : base(state)
    {
        this.onSelected = onSelected;
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("Tags");

        foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            root.AddChild(new AdvancedDropdownItem(tag));
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        onSelected?.Invoke(item.name);
    }
}