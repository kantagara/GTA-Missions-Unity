using UnityEditor;
using UnityEngine;

class MissionCreatorWindow : EditorWindow {
    
    private static MissionCreatorWindow _window;
    
    [MenuItem ("Tools/Mission Creator")]
    public static void  ShowWindow ()
    {
        if (_window != null) return;
        _window = GetWindow<MissionCreatorWindow>();
        _window.titleContent = new GUIContent("Mission Creator");
    }
    
    void OnGUI () {
        
    }
}