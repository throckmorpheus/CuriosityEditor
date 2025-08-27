
using System;
using System.Numerics;
using ImGuiNET;
using OWML.Common;

namespace CuriosityEditor;

public class Console {
    private static Console Instance;
    
	private static IModConsole ModConsole => Main.Instance?.ModHelper?.Console;

    public void Start() {
        if (Instance is not null) throw new Exception($"Attempted to initialise more than one {GetType().Name}");
        Instance = this;
    }

    public static event Action<string, MessageType, Type> OnMessage;
    
    public static void Info(string message)                 => Message(message, MessageType.Info);
    public static void Info<T>(string message)              => Message(message, MessageType.Info, typeof(T));
    public static void Info<T>(T _from, string message)     => Message(message, MessageType.Info, typeof(T));
    
    public static void Warning(string message)              => Message(message, MessageType.Warning);
    public static void Warning<T>(string message)           => Message(message, MessageType.Warning, typeof(T));
    public static void Warning<T>(T _from, string message)  => Message(message, MessageType.Warning, typeof(T));

    public static void Error(string message)                => Message(message, MessageType.Error);
    public static void Error<T>(string message)             => Message(message, MessageType.Error, typeof(T));
    public static void Error<T>(T _from, string message)    => Message(message, MessageType.Error, typeof(T));
    
    public static void Debug(string message)                => Message(message, MessageType.Debug);
    public static void Debug<T>(string message)             => Message(message, MessageType.Debug, typeof(T));
    public static void Debug<T>(T _from, string message)    => Message(message, MessageType.Debug, typeof(T));
    
    public static void Success(string message)              => Message(message, MessageType.Success);
    public static void Success<T>(string message)           => Message(message, MessageType.Success, typeof(T));
    public static void Success<T>(T _from, string message)  => Message(message, MessageType.Success, typeof(T));
    
    public static void Fatal(string message)                => Message(message, MessageType.Fatal);
    public static void Fatal<T>(string message)             => Message(message, MessageType.Fatal, typeof(T));
    public static void Fatal<T>(T _from, string message)    => Message(message, MessageType.Fatal, typeof(T));
    
    private static void Message(string message, MessageType messageType, Type senderType = null) {
        // Send message to OWML console
        if (senderType is null) ModConsole?.WriteLine(message, messageType);
        else                    ModConsole?.WriteLine(message, messageType, senderType.Name);

        // Trigger event (for ImGui console to hook on to)
        OnMessage?.Invoke(message, messageType, senderType);
    }
}