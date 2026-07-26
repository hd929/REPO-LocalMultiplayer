using com.github.zehsteam.LocalMultiplayer.Managers;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;

namespace com.github.zehsteam.LocalMultiplayer.Patches;

[HarmonyPatch(typeof(DataDirector))]
internal static class DataDirector_Patches
{
    [HarmonyPatch(nameof(DataDirector.PhotonSetAppId))]
    [HarmonyPostfix]
    private static void PhotonSetAppId_Patch()
    {
        if (!ConfigManager.HasPhotonAppIds)
        {
            Logger.LogError("Photon App IDs are missing. Set Photon/AppIdRealtime and Photon/AppIdVoice in the global config.");
            return;
        }

        AppSettings appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        appSettings.AppIdRealtime = ConfigManager.Photon_AppIdRealtime.Value.Trim();
        appSettings.AppIdVoice = ConfigManager.Photon_AppIdVoice.Value.Trim();
    }

    [HarmonyPatch(nameof(DataDirector.SaveSettings))]
    [HarmonyPrefix]
    private static bool SaveSettings_Patch()
    {
        if (!SteamAccountManager.IsUsingSpoofAccount)
        {
            return true;
        }

        return false;
    }

    [HarmonyPatch(nameof(DataDirector.ColorSetBody))]
    [HarmonyPrefix]
    private static bool ColorSetBody_Patch(int colorID)
    {
        if (!SteamAccountManager.IsUsingSpoofAccount)
        {
            return true;
        }

        SteamAccountManager.SetCurrentSpoofAccountColor(colorID);
        return false;
    }

    [HarmonyPatch(nameof(DataDirector.ColorGetBody))]
    [HarmonyPrefix]
    private static bool ColorGetBody_Patch(ref int __result)
    {
        if (!SteamAccountManager.IsUsingSpoofAccount)
        {
            return true;
        }

        __result = SteamAccountManager.SpoofAccount.ColorId;
        return false;
    }
}
