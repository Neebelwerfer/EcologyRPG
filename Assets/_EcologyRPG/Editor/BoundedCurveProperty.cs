using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BoundedCurveAttribute))]
public class BoundedCurveProperty : PropertyDrawer
{
    private bool validatedRanges;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // grab the first element of the array of type BoundedCurveAttribute[]
        BoundedCurveAttribute customAttr = (BoundedCurveAttribute)fieldInfo.GetCustomAttributes(typeof(BoundedCurveAttribute), true)[0];
        // begain drawing the property
        EditorGUI.BeginProperty(position, label, property);
        // validate the property type
        if (!ValidateField(property.type))
        {
            EditorGUILayout.HelpBox("Bounded Curve Attribute Can Be Used Only On Animation Curves", MessageType.Error);
            return;
        }
        // validate the ranges only once to check if after compiling changes the range changed
        if (!validatedRanges)
        {
            ValidateKeyFramesRanges(property, customAttr.range);
            validatedRanges = true;
        }
        // draw the curve field
        property.animationCurveValue = EditorGUILayout.CurveField(label, property.animationCurveValue, Color.green, customAttr.range);
        // end drawing
        EditorGUI.EndProperty();
    }

    /// <summary>
    /// validate the field type
    /// </summary>
    /// <returns>true if the field type is animation curve</returns>
    private bool ValidateField(string fieldType)
    {
        // check the type of the field
        if (fieldType != nameof(AnimationCurve))
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// validate the first and last keyframe on the curve
    /// and reset the curve if the validation failed
    /// </summary>
    private void ValidateKeyFramesRanges(SerializedProperty property, Rect range)
    {
        AnimationCurve curve = property.animationCurveValue;
        int curveLength = curve.length;


        if (curveLength < 1) return;


        if (!curve.keys[0].ValidateKeyFrameCor(range.min, range.max) ||
            !curve.keys[curveLength - 1].ValidateKeyFrameCor(range.min, range.max))
        {
            property.animationCurveValue = AnimationCurve.Constant(range.xMin, range.xMax, range.yMax);
        }
    }
}
public static class AnimationCurveExtention
{
    /// <summary>
    /// checks a key frame values if it's inside the rect range
    /// </summary>
    /// <returns>true if the keyframe is inside the rect range</returns>
    public static bool ValidateKeyFrameCor(this Keyframe keyframe, Vector2 minPoint, Vector2 MaxPoint)
    {
        if (keyframe.value >= minPoint.y && keyframe.value <= MaxPoint.y &&
            keyframe.time >= minPoint.x && keyframe.time <= MaxPoint.x)
            return true;


        return false;

    }
}