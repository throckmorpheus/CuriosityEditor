using OWML.Common;

namespace CuriosityEditor;

public static class ModConsoleExtensions
{
    public static void Message(this IModConsole console, string message, MessageType messageType = MessageType.Message) =>
        console?.WriteLine(message, messageType);

    public static void Message<T>(this IModConsole console, string message, MessageType messageType = MessageType.Message) =>
        console?.WriteLine(message, messageType, typeof(T).Name);

    public static void Message<T>(this IModConsole console, T _from, string message, MessageType messageType = MessageType.Message) =>
        console.Message<T>(message, messageType);
    
    public static void Error(this IModConsole console, string message) => console.Message(message, MessageType.Error);
    public static void Error<T>(this IModConsole console, string message) => console.Message<T>(message, MessageType.Error);
    public static void Error<T>(this IModConsole console, T _from, string message) => console.Error<T>(message);
    
    public static void Success(this IModConsole console, string message) => console.Message(message, MessageType.Success);
    public static void Success<T>(this IModConsole console, string message) => console.Message<T>(message, MessageType.Success);
    public static void Success<T>(this IModConsole console, T _from, string message) => console.Success<T>(message);
    
    public static void Debug(this IModConsole console, string message) => console.Message(message, MessageType.Debug);
    public static void Debug<T>(this IModConsole console, string message) => console.Message<T>(message, MessageType.Debug);
    public static void Debug<T>(this IModConsole console, T _from, string message) => console.Debug<T>(message);
    
    public static void Info(this IModConsole console, string message) => console.Message(message, MessageType.Info);
    public static void Info<T>(this IModConsole console, string message) => console.Message<T>(message, MessageType.Info);
    public static void Info<T>(this IModConsole console, T _from, string message) => console.Info<T>(message);
}