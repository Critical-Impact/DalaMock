namespace DalaMock.Core.Mocks;

using System;
using System.Collections.Generic;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

public class MockChatGui : IChatGui, IMockService
{
    public void Print(XivChatEntry chat)
    {
    }

    public void Print(string message, string? messageTag = null, ushort? tagColor = null)
    {
    }

    public void Print(SeString message, string? messageTag = null, ushort? tagColor = null)
    {
    }

    public void PrintError(string message, string? messageTag = null, ushort? tagColor = null)
    {
    }

    public void PrintError(SeString message, string? messageTag = null, ushort? tagColor = null)
    {
    }

    public IReadOnlyDictionary<(string PluginName, uint CommandId), Action<uint, SeString>> RegisteredLinkHandlers
    {
        get;
    }

    public int LastLinkedItemId { get; }

    public byte LastLinkedItemFlags { get; }

    public event IChatGui.OnMessageDelegate? ChatMessage;

    public event IChatGui.OnCheckMessageHandledDelegate? CheckMessageHandled;

    public event IChatGui.OnMessageHandledDelegate? ChatMessageHandled;

    public event IChatGui.OnMessageUnhandledDelegate? ChatMessageUnhandled;

    public string ServiceName => "Chat Gui";
}