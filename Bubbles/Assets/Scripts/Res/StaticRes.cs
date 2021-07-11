using UnityEngine;

public static class StaticRes
{
    public static float LookDir(Vector3 target)
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target);

        mousePos.x -= targetPos.x;
        mousePos.y -= targetPos.y;

        return Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    }

    public static float LookDir(Vector3 self, Vector3 target)
    {
        Vector3 dir = target - self;

        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static Vector3 LookDir(Vector3 target, bool returnVector3)
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target);

        mousePos.x -= targetPos.x;
        mousePos.y -= targetPos.y;

        return mousePos;
    }

    public static Vector3 LookDir(Vector3 self, Vector3 target, bool returnVector2) => target - self;
}