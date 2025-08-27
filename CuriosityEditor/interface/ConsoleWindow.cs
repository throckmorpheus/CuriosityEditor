
using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using OWML.Common;

namespace CuriosityEditor.Interface;

public class ConsoleWindow : Window {
    private readonly Dictionary<MessageType, Vector4> messageColours = [];
    private readonly List<(string Message, MessageType Type, Type sender)> lines = [];

    public ConsoleWindow() {
        Console.OnMessage += OnMessage;

        messageColours[MessageType.Error]   = new Vector4(1f, 0f, 0f, 1f);
        messageColours[MessageType.Fatal]   = new Vector4(1f, 0f, 0f, 1f);
        messageColours[MessageType.Warning] = new Vector4(1f, 0.871f, 0.129f, 1f);
        messageColours[MessageType.Debug]   = new Vector4(0.729f, 0.333f, 0.827f, 1f);
        messageColours[MessageType.Success] = new Vector4(0f, 1f, 0f, 1f);
    }
    ~ConsoleWindow() { Console.OnMessage -= OnMessage; }

    private void OnMessage(string message, MessageType messageType, Type senderType) {
        lines.Add((message, messageType, senderType));
        wasLineAddedSinceLastDraw = true;
    }

    private bool wasLineAddedSinceLastDraw = false;

    public override ImGuiWindowFlags WindowConfig() {
        ImGui.SetNextWindowSize(new Vector2(800f, 300f), ImGuiCond.Appearing);
        ImGui.SetNextWindowBgAlpha(0.5f);
        return ImGuiWindowFlags.None;
    }

    public override void Content() {
        ImGui.Columns(2);
        ImGui.SetColumnWidth(0, 200f);
        foreach (var (message, type, sender) in lines) {
            if (sender is not null) ImGui.Text(sender.Name);
            ImGui.NextColumn();

            if (messageColours.TryGetValue(type, out var colour)) ImGui.TextColored(colour, message);
            else ImGui.Text(message);
            ImGui.NextColumn();
        }
        if (wasLineAddedSinceLastDraw && ImGui.GetScrollY() == ImGui.GetScrollMaxY()) { ImGui.SetScrollHereY(1f); }
        ImGui.Columns(1);

        wasLineAddedSinceLastDraw = false;
    }
}