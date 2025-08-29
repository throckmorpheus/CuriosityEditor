using System;

namespace CuriosityEditor.Config;

[AttributeUsage(AttributeTargets.Field)]
public class MetaAttribute(string displayName = null, object assume = null, bool ignore = false) : Attribute {
    public readonly string DisplayName = displayName;
    public readonly object Assumption = assume;
    public readonly bool Ignore = ignore;
}