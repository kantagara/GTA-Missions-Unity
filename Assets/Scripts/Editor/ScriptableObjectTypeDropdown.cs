using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

internal class ScriptableObjectTypeDropdown : AdvancedDropdown
{
    private readonly string _dropdownTitle;
    private readonly Action<Type> _onSelected;
    private readonly List<(Type type, string displayName)> _types;

    public ScriptableObjectTypeDropdown(string dropdownTitle, AdvancedDropdownState state,
        List<(Type, string)> types, Action<Type> onSelected)
        : base(state)
    {
        _dropdownTitle = dropdownTitle;
        _onSelected = onSelected;
        _types = types;
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem(_dropdownTitle);
        foreach (var (type, name) in _types) root.AddChild(new ScriptableObjectDropdownItem(name, type));

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        if (item is ScriptableObjectDropdownItem dropdownItem) _onSelected?.Invoke(dropdownItem.Type);
    }

    private class ScriptableObjectDropdownItem : AdvancedDropdownItem
    {
        public ScriptableObjectDropdownItem(string name, Type type) : base(name)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}