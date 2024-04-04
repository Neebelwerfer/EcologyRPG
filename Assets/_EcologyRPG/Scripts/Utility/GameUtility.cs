using UnityEngine;

public static class GameUtility
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}