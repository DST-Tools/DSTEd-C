using System;
using DSTEd.Core;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

[MoonSharpUserData]
public class TheSim {
    public static void ProfilerPush(string message) {
        Logger.Info("[TheSim] TheSim::ProfilerPush(" + message + ")");
    }

    public static void ProfilerPop() {
        Logger.Info("[TheSim] TheSim::ProfilerPop");
    }

    public static void SetSoundVolume(int k, int v) {
        Logger.Info("[TheSim] TheSim::SetSoundVolume(" + k + ", " + v + ")");
    }

    public static int GetSoundVolume() {
        Logger.Info("[TheSim] TheSim::GetSoundVolume()");
        return 0;
    }

    public static void SetSetting(string name, string key, string value) {
        Logger.Info("[TheSim] TheSim::SetSetting(" + name + ", " + key + ", " + value + ")");
    }

    public static Boolean IsDebugPaused() {
        Logger.Info("[TheSim] TheSim::IsDebugPaused()");
        return false;
    }

    public static void ToggleDebugPause() {
        Logger.Info("[TheSim] TheSim::ToggleDebugPause()");
    }

    public static string GetUsersName() {
        Logger.Info("[TheSim] TheSim::GetUsersName()");
        return "Guest";
    }

    public static string GetSteamUserID() {
        Logger.Info("[TheSim] TheSim::GetSteamUserID()");
        return "STEAMID1234567890";
    }

    public static void LogBulkMetric(object data) {
        Logger.Info("[TheSim] TheSim::LogBulkMetric(" + data + ")");
    }

    public static void SetReverbPreset(String value) {
        Logger.Info("[TheSim] TheSim::SetReverbPreset(" + value + ")");
    }

    public static void SendJSMessage(string value) {
        Logger.Info("[TheSim] TheSim::SendJSMessage(" + value + ")");
    }

    public static void SetDLCEnabled(int index, Boolean value) {
        Logger.Info("[TheSim] TheSim::SetDLCEnabled(" + index + ", " + value + ")");
    }

    public static void FindEntities(int x, int y, int z, int r, object options) {
        Logger.Info("[TheSim] TheSim::FindEntities(" + x + ", " + y + ", " + z + ", " + r + ", " + options + ")");
    }

    public static object GetPosition() {
        Logger.Info("[TheSim] TheSim::GetPosition()");
        return null;
    }

    public static object ProjectScreenPos() {
        Logger.Info("[TheSim] TheSim::ProjectScreenPos()");
        return null;
    }

    public static void QueryServer(string query, object json, object result) {
        Logger.Info("[TheSim] TheSim::QueryServer(" + query + ", " + json + ", " + result + ")");
    }

    public static void SetPersistentString(string name, object data, Boolean encoded, object callback) {
        Logger.Info("[TheSim] TheSim::SetPersistentString(" + name + ", " + data + ", " + encoded + ", " + callback + ")");
    }

    public static void SetRenderPassDefaultEffect(string name, string file) {
        Logger.Info("[TheSim] TheSim::SetRenderPassDefaultEffect(" + name + ", " + file + ")");
    }

    public static void SetErosionTexture(string name) {
        Logger.Info("[TheSim] TheSim::SetErosionTexture(" + name + ")");
    }

    public static int GetFileModificationTime(string file) {
        Logger.Info("[TheSim] TheSim::GetFileModificationTime(" + file + ")");
        return 0;
    }

    public static void LuaPrint(string message) {
        Logger.Info("[TheSim] TheSim::LuaPrint(" + message + ")");
    }

    public static void SetNetbookMode(Boolean value) {
        Logger.Info("[TheSim] TheSim::SetNetbookMode(" + value + ")");
    }

    public static void ClearFileSystemAliases() {
        Logger.Info("[TheSim] TheSim::ClearFileSystemAliases()");
    }

    public static int GetNumLaunches() {
        Logger.Info("[TheSim] TheSim::GetNumLaunches()");
        return 0; // 1 = RecordGameStartStats
    }
}
