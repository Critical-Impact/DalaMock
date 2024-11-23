using System.Collections.ObjectModel;

namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

public class MockChatGui : IChatGui, IMockService
{
    private readonly IPluginLog pluginLog;

    public MockChatGui(IPluginLog pluginLog)
    {
        this.pluginLog = pluginLog;
        this.RegisteredLinkHandlers = new ReadOnlyDictionary<(string PluginName, uint CommandId), Action<uint, SeString>>(new Dictionary<(string PluginName, uint CommandId), Action<uint, SeString>>());
    }

    /// <inheritdoc />
    public event IChatGui.OnMessageDelegate? ChatMessage;

    /// <inheritdoc />
    public event IChatGui.OnCheckMessageHandledDelegate? CheckMessageHandled;

    /// <inheritdoc />
    public event IChatGui.OnMessageHandledDelegate? ChatMessageHandled;

    /// <inheritdoc />
    public event IChatGui.OnMessageUnhandledDelegate? ChatMessageUnhandled;

    /// <inheritdoc />
    public IReadOnlyDictionary<(string PluginName, uint CommandId), Action<uint, SeString>> RegisteredLinkHandlers
    {
        get;
        set;
    }

    public void PrintError(ReadOnlySpan<byte> message, string? messageTag = null, ushort? tagColor = null)
    {
        var stringBuilder = new Lumina.Text.SeStringBuilder();
        stringBuilder.Append(message);
        this.PrintError(stringBuilder.ToSeString(), messageTag, tagColor);
    }

    /// <inheritdoc />
    public uint LastLinkedItemId { get; set; }

    /// <inheritdoc />
    public byte LastLinkedItemFlags { get; set; }

    /// <inheritdoc />
    public string ServiceName => "Chat Gui";

    /// <inheritdoc />
    public void Print(XivChatEntry chat)
    {
        this.pluginLog.Info($"[{chat.Type.ToString()}] [{chat.Timestamp:HH:mm:ss}] {chat.Message}");
    }

    /// <inheritdoc />
    public void Print(string message, string? messageTag = null, ushort? tagColor = null)
    {
        this.pluginLog.Info($"{(messageTag == null ? "" : $"[{messageTag}] ")}{message}");
    }

    /// <inheritdoc />
    public void Print(SeString message, string? messageTag = null, ushort? tagColor = null)
    {
        this.pluginLog.Info($"{(messageTag == null ? "" : $"[{messageTag}] ")}{message}");
    }

    /// <inheritdoc />
    public void PrintError(string message, string? messageTag = null, ushort? tagColor = null)
    {
        this.pluginLog.Info($"ERROR: {(messageTag == null ? "" : $"[{messageTag}] ")}{message}");
    }

    /// <inheritdoc />
    public void PrintError(SeString message, string? messageTag = null, ushort? tagColor = null)
    {
        this.pluginLog.Info($"ERROR: {(messageTag == null ? "" : $"[{messageTag}] ")}{message.TextValue}");
    }

    public void Print(ReadOnlySpan<byte> message, string? messageTag = null, ushort? tagColor = null)
    {
        var stringBuilder = new Lumina.Text.SeStringBuilder();
        stringBuilder.Append(message);
        this.Print(stringBuilder.ToSeString(), messageTag, tagColor);
    }

    public virtual void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool ishandled)
    {
        this.ChatMessage?.Invoke(type, timestamp, ref sender, ref message, ref ishandled);
    }

    public virtual void OnCheckMessageHandled(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool ishandled)
    {
        this.CheckMessageHandled?.Invoke(type, timestamp, ref sender, ref message, ref ishandled);
    }

    public virtual void OnChatMessageHandled(XivChatType type, int timestamp, SeString sender, SeString message)
    {
        this.ChatMessageHandled?.Invoke(type, timestamp, sender, message);
    }

    public virtual void OnChatMessageUnhandled(XivChatType type, int timestamp, SeString sender, SeString message)
    {
        this.ChatMessageUnhandled?.Invoke(type, timestamp, sender, message);
    }
}
