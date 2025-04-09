
namespace CuriosityEditor;

public static class VectorConversionExtensions
{
    public static System.Numerics.Vector2 ToSystemVector(this UnityEngine.Vector2 vector) => new(vector.x, vector.y);
    public static System.Numerics.Vector3 ToSystemVector(this UnityEngine.Vector3 vector) => new(vector.x, vector.y, vector.z);
    public static System.Numerics.Vector4 ToSystemVector(this UnityEngine.Vector4 vector) => new(vector.x, vector.y, vector.z, vector.w);
    public static System.Numerics.Vector4 ToSystemVector(this UnityEngine.Color color) => new(color.r, color.g, color.b, color.a);

    public static UnityEngine.Vector2 ToUnityVector(this System.Numerics.Vector2 vector) => new(vector.X, vector.Y);
    public static UnityEngine.Vector3 ToUnityVector(this System.Numerics.Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static UnityEngine.Vector4 ToUnityVector(this System.Numerics.Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
}