using DalaMock.Core.Mocks;

namespace DalaMock.Sample.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Dalamud.Plugin.Services;

public class MockSigScanner : ISigScanner, IMockService
{
    public MockSigScanner(IPluginLog pluginLog)
    {
        pluginLog.Verbose("Hello from a totally custom dalamud service!");
    }

    public IntPtr GetStaticAddressFromSig(string signature, int offset = 0)
    {
        throw new NotImplementedException();
    }

    public bool TryGetStaticAddressFromSig(string signature, out IntPtr result, int offset = 0)
    {
        throw new NotImplementedException();
    }

    public IntPtr ScanData(string signature)
    {
        throw new NotImplementedException();
    }

    public bool TryScanData(string signature, out IntPtr result)
    {
        throw new NotImplementedException();
    }

    public IntPtr ScanModule(string signature)
    {
        throw new NotImplementedException();
    }

    public bool TryScanModule(string signature, out IntPtr result)
    {
        throw new NotImplementedException();
    }

    public IntPtr ResolveRelativeAddress(IntPtr nextInstAddr, int relOffset)
    {
        throw new NotImplementedException();
    }

    public IntPtr ScanText(string signature)
    {
        throw new NotImplementedException();
    }

    public bool TryScanText(string signature, out IntPtr result)
    {
        throw new NotImplementedException();
    }

    public IntPtr[] ScanAllText(string signature)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IntPtr> ScanAllText(string signature, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public bool IsCopy { get; }

    public bool Is32BitProcess { get; }

    public IntPtr SearchBase { get; }

    public IntPtr TextSectionBase { get; }

    public long TextSectionOffset { get; }

    public int TextSectionSize { get; }

    public IntPtr DataSectionBase { get; }

    public long DataSectionOffset { get; }

    public int DataSectionSize { get; }

    public IntPtr RDataSectionBase { get; }

    public long RDataSectionOffset { get; }

    public int RDataSectionSize { get; }

    public ProcessModule Module { get; }

    public string ServiceName => "Sig Scanner";
}
