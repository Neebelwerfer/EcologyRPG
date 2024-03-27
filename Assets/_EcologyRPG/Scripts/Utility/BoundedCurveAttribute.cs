using UnityEngine;

public class BoundedCurveAttribute : PropertyAttribute
{
    // Start is called before the first frame update
    public readonly Rect range;

    public BoundedCurveAttribute(float width, float height)
    {
        range = new Rect(0, 0, width, height);
    }

    public BoundedCurveAttribute(float startingPointX, float startingPointY, float width, float height)
    {
        range = new Rect(startingPointX, startingPointY, width, height);
    }
}