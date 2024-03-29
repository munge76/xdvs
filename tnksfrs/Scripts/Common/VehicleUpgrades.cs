using System;
using System.Collections.Generic;
using System.Linq;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using XD;

[Serializable]
public class VehicleUpgrades
{
    public readonly int                     vehicleId;
    public ObscuredInt                      battlesCount = 0;
    public XD.ModuleType                    awaitedModule = XD.ModuleType.None;
    public double                           moduleReadyTime = -10;
    private int                             camouflageId;
    private int                             decalId;
    private double                          camouflageStopTime; // Ненужное поле, но оно сериализовано.
    private List<PurchasedPattern>          ownedCamouflages;
    private List<PurchasedDecal>            ownedDecals;
    private Dictionary<XD.ModuleType, int>  moduleLevels;
    private Status                          status = Status.None; 

    public Status Status
    {
        get
        {
            return status;
        }
    }

    public Dictionary<XD.ModuleType, int> ModuleLevels
    {
        get { return moduleLevels; }
    }

    public VehicleUpgrades(
        VehicleInfo                                 vehicleInfo,
        Dictionary<XD.ModuleType, int> levels              = null,
        XD.ModuleType                  _awaitedModule      = XD.ModuleType.None,
        double                                      _moduleReadyTime    = 0,
        int                                         _camouflageId       = 0,
        int                                         _decalId            = 0)
    {
        vehicleId = vehicleInfo.id;
        Init(levels, _awaitedModule, _moduleReadyTime, _camouflageId, _decalId);
    }

    public VehicleUpgrades(
        int                                         _vehicleId,
        int                                         _battlesCount,
        Dictionary<XD.ModuleType, int> levels              = null,
        XD.ModuleType                  _awaitedModule      = XD.ModuleType.None,
        double                                      _moduleReadyTime    = 0,
        int                                         _camouflageId       = 0,
        int                                         _decalId            = 0)
    {
        vehicleId = _vehicleId;
        battlesCount = _battlesCount;
        Init(levels, _awaitedModule, _moduleReadyTime, _camouflageId, _decalId);
    }

    public static VehicleUpgrades GetFullModuleUpgrades(VehicleInfo vehicleInfo)
    {
        return new VehicleUpgrades(vehicleInfo, new Dictionary<XD.ModuleType, int>
        {
            { XD.ModuleType.Armor, vehicleInfo.GetMaxUpgradeLevel(XD.ModuleType.Armor) },
            { XD.ModuleType.Cannon, vehicleInfo.GetMaxUpgradeLevel(XD.ModuleType.Cannon) },
            { XD.ModuleType.Engine, vehicleInfo.GetMaxUpgradeLevel(XD.ModuleType.Engine) },
            { XD.ModuleType.Reloader, vehicleInfo.GetMaxUpgradeLevel(XD.ModuleType.Reloader) },
            { XD.ModuleType.Tracks, vehicleInfo.GetMaxUpgradeLevel(XD.ModuleType.Tracks) },
        });
    }

    private void Init(
        Dictionary<XD.ModuleType, int>  levels = null,
        XD.ModuleType                   _awaitedModule      = XD.ModuleType.None,
        double                                  _moduleReadyTime    = 0,
        int                                     _camouflageId       = 0,
        int                                     _decalId            = 0)
    {
        //Debug.Log("Init tank: " + vehicleId + "; Modules: " + levels.Count);
        moduleLevels = levels ?? new Dictionary<XD.ModuleType, int>(5)
        {
           { XD.ModuleType.Reloader, 0 },
           { XD.ModuleType.Armor, 0 },
           { XD.ModuleType.Cannon, 0 },
           { XD.ModuleType.Engine, 0 },
           { XD.ModuleType.Tracks, 0 }
        };

        awaitedModule = _awaitedModule;
        moduleReadyTime = _moduleReadyTime;
        camouflageId = _camouflageId;
        decalId = _decalId;
        ownedCamouflages = ownedCamouflages ?? new List<PurchasedPattern>();
        ownedDecals = ownedDecals ?? new List<PurchasedDecal>();
    }

    public int CamouflageId
    {
        get
        {
            return OwnedCamouflages.Any(camo => camo.id == camouflageId)
                ? camouflageId
                : 0;
        }
    }

    public int DecalId
    {
        get
        {
            return OwnedDecals.Any(decal => decal.id == decalId)
                ? decalId
                : 0;
        }
    }

    public List<PurchasedPattern> OwnedCamouflages
    {
        get { return ownedCamouflages ?? new List<PurchasedPattern>(); }
    }

    public List<PurchasedDecal> OwnedDecals
    {
        get { return ownedDecals ?? new List<PurchasedDecal>(); }
    }

    public void SetModuleLevel(XD.ModuleType type, int level)
    {
        moduleLevels[type] = level;
    }

    public int GetModuleLevel(XD.ModuleType type)
    {
        return type == XD.ModuleType.None ? 0 : moduleLevels[type];
    }

    public void SetCamouflageId(int camoId)
    {
        camouflageId = camoId;
    }

    public void SetDecalId(int decId)
    {
        decalId = decId;
    }

    public void PurchaseDecal(Decal decal)
    {
        if (ownedDecals == null)
            ownedDecals = new List<PurchasedDecal>();

        if (!ownedDecals.Any())
            decalId = 0;

        ownedDecals.Add(new PurchasedDecal(decal.id, decal.lifetime));
    }

    public Dictionary<string, object> ToDictionary()
    {
        var dict = new Dictionary<string, object>();
        dict["tankId"] = (int)vehicleId;
        dict["awaitedModule"] = awaitedModule.ToString();
        dict["moduleReadyTime"] = (double)moduleReadyTime;
        dict["camouflageId"] = (int)CamouflageId;
        dict["decalId"] = (int)DecalId;
        dict["battlesCount"] = (int)battlesCount;

        if (null != ownedCamouflages)
            dict["ownedCamouflages"] = ownedCamouflages.Select(item => item.ToDictionary()).ToList();

        if (null != ownedDecals)
            dict["ownedDecals"] = ownedDecals.Select(item => item.ToDictionary()).ToList();

        dict["moduleLevels"] = moduleLevels;
        return dict;
    }

    public static VehicleUpgrades FromDictionary(Dictionary<string, object> dict)
    {
        var prefs = new JsonPrefs(dict);

        int l_vehicleId = prefs.ValueInt("tankId", 0);
        double l_moduleReadyTime = prefs.ValueDouble("moduleReadyTime", 0);
        int l_camouflageId = prefs.ValueInt("camouflageId", 0);
        int l_decalId = prefs.ValueInt("decalId", 0);
        int l_battlesCount = prefs.ValueInt("battlesCount", 0);
        int status = prefs.ValueInt("status");        

        XD.ModuleType l_awaitedModule = XD.ModuleType.None;
        try
        {
            l_awaitedModule = (XD.ModuleType)Enum.Parse(typeof(XD.ModuleType), prefs.ValueString("awaitedModule", "None"));
        }
        catch
        {
            l_awaitedModule = XD.ModuleType.None;
        }

        List<PurchasedPattern> l_ownedCamouflages 
            = prefs.ValueObjectList("ownedCamouflages")
                .Select(item => PurchasedPattern.FromDictionary<PurchasedPattern>(item as Dictionary<string, object>))
                .ToList();

        List<PurchasedDecal> l_ownedDecals
            = prefs.ValueObjectList("ownedDecals")
                .Select(item => PurchasedPattern.FromDictionary<PurchasedDecal>(item as Dictionary<string, object>))
                .ToList();

        Dictionary<XD.ModuleType, int> l_moduleLevels = ParseModules(prefs.ValueObjectDict("moduleLevels"));

        var obj = new VehicleUpgrades(l_vehicleId, l_battlesCount, l_moduleLevels, l_awaitedModule, l_moduleReadyTime, l_camouflageId, l_decalId);
        obj.ownedCamouflages = l_ownedCamouflages ?? new List<PurchasedPattern>();
        obj.ownedDecals = l_ownedDecals ?? new List<PurchasedDecal>();
        obj.status = (Status)status;

        return obj;
    }

    private static Dictionary<XD.ModuleType, int> ParseModules(Dictionary<string, object> dict)
    {
        if (dict == null)
        {
            return null;
        }

        var ret = new Dictionary<XD.ModuleType, int>();

        foreach (var el in dict)
        {
            try
            {
                ret[(XD.ModuleType)Enum.Parse(typeof(XD.ModuleType), el.Key, true)] = Convert.ToInt32(el.Value);
            }
            catch
            {
                Debug.LogWarning("Can't parse module level: " + el.Key + " - " + el.Value.ToString());
            }
        }

        return ret;
    }
}
