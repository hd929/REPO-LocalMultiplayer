using HarmonyLib;
using Photon.Pun;
using System;

namespace com.github.zehsteam.LocalMultiplayer.Patches;

[HarmonyPatch(typeof(PunManager))]
internal static class PunManager_Patches
{
    [HarmonyPatch(nameof(PunManager.SetItemNameRPC))]
    [HarmonyPrefix]
    private static bool SetItemNameRPC_Prefix(string _name, int photonViewID, PhotonMessageInfo _info)
    {
        try
        {
            PhotonView view = PhotonView.Find(photonViewID);
            if (view == null || view.gameObject == null)
            {
                Logger.LogWarning($"SetItemNameRPC bypass: photonViewID {photonViewID} is null or destroyed. (More Shop Items conflict?)");
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"SetItemNameRPC bypass error: {ex.Message}");
            return false; // Chặn RPC nếu lỗi để tránh crash PhotonHandler
        }
        return true;
    }
}
