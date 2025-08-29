
namespace CuriosityEditor;

public static class StringStripExtensions {
    public static string StripFront(this string self, string strip)
        => self.StartsWith(strip) ? self.Substring(strip.Length) : self;
    
    public static string StripBack(this string self, string strip)
        => self.EndsWith(strip) ? self.Substring(0, self.Length - strip.Length) : self;
}