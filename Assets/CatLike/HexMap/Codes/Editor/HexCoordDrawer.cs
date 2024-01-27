using UnityEditor;
using UnityEngine;

namespace CatLike.HexMap.Codes.Editor
{
    /// <summary>
    /// 自定义绘制HexCoord坐标
    /// </summary>
    [CustomPropertyDrawer(typeof(HexCoord))]
    public class HexCoordDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            position = EditorGUI.PrefixLabel(position, label);
            HexCoord coord = new HexCoord(property.FindPropertyRelative("X").intValue,
                property.FindPropertyRelative("Z").intValue);
            
            GUI.Label(position,coord.ToString());
        }
    }
}