﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StardewUI.Framework.Diagnostics;

/// <summary>
/// Maintains the state of a single trace and writes it to disk when completed.
/// </summary>
/// <param name="mod">Info about the requesting mod.</param>
/// <param name="configSelector">Function to retrieve the current tracing configuration.</param>
internal class TraceWriter(IManifest mod, Func<TraceConfig> configSelector) : StardewUI.Diagnostics.ITraceWriter
{
    /// <inheritdoc />
    public bool IsTracing => currentTrace is not null;

    private static readonly JsonSerializer serializer =
        new()
        {
            ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() },
            Formatting = Formatting.None,
        };

    private readonly string exporter = $"{mod.UniqueID}@{mod.Version}";

    private TraceFile? currentTrace;

    /// <inheritdoc />
    public IDisposable BeginSlice(string name)
    {
        if (currentTrace is null)
        {
            throw new InvalidOperationException("Cannot begin a trace slice when no trace is active.");
        }
        int frame = currentTrace.OpenFrame(name);
        return new FrameCloser(currentTrace, frame);
    }

    /// <inheritdoc />
    public void BeginTrace()
    {
        if (currentTrace is not null)
        {
            throw new InvalidOperationException("Cannot begin a new trace when tracing has already started.");
        }
        var config = configSelector();
        currentTrace = new() { Exporter = exporter };
        LogTraceMessage("Trace started", config.EnableHudNotifications);
    }

    /// <inheritdoc />
    public void EndTrace()
    {
        if (currentTrace is null)
        {
            throw new InvalidOperationException("Cannot end trace when no trace is active.");
        }
        var config = configSelector();
        var outputDirectory = Path.IsPathFullyQualified(config.OutputDirectory)
            ? config.OutputDirectory
            : Path.Combine(Constants.DataPath, config.OutputDirectory);
        Directory.CreateDirectory(outputDirectory);
        var traceName = $"StardewUI_{currentTrace.CreationDate:yyyyMMdd_HHmmss}.json";
        var fileName = Path.Combine(outputDirectory, traceName);
        using var file = File.CreateText(fileName);
        serializer.Serialize(file, currentTrace);
        file.Flush();
        currentTrace = null;
        LogTraceMessage($"Trace written to {traceName}", config.EnableHudNotifications);
    }

    private static void LogTraceMessage(string message, bool enableHudMessage)
    {
        Logger.Log(message, LogLevel.Info);
        if (enableHudMessage)
        {
            Game1.addHUDMessage(HUDMessage.ForCornerTextbox("StardewUI: " + message));
        }
    }

    class FrameCloser(TraceFile trace, int frame) : IDisposable
    {
        public void Dispose()
        {
            trace.CloseFrame(frame);
            GC.SuppressFinalize(this);
        }
    }
}
