using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Tanks.Models;

[Serializable]
public class Tab : MonoBehaviour
{
    private Tanks.Models.Room room;

    private bool IsClanTab { get { return room.type == ChatRoom.Clan; } }
    private bool IsCountryTab { get { return room.type == ChatRoom.Country; } }

    public string Flag
    {
        set
        {
            
        }
    }

    [HideInInspector]

    private void Awake()
    {
        Dispatcher.Subscribe(EventId.OnLanguageChange, OnLanguageChanged);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EventId.OnLanguageChange, OnLanguageChanged);
    }

    /*public static Tab Create(Tanks.Models.Room room, tk2dUILayoutContainerSizer parentContainerSizer, Tab tabsPrefab)
    {
        var tab = Instantiate(tabsPrefab);
        tab.transform.parent = parentContainerSizer.transform;
        tab.transform.localPosition = Vector3.zero;
        tab.room = room;
        tab.name = room.type.ToString();

        if(tab.IsCountryTab)
            tab.Flag = room.Code.ToLower();

        tab.UpdateLabels();
        return tab;
    }*/

    public void UpdateLabels()
    {
        string text = "";

        if (IsCountryTab)
        {
            text = Localizer.GetText(string.Format("lblStats{0}", room.type), room.Name);
        }
        else if (IsClanTab)
        {
            if (ProfileInfo.Clan == null || string.IsNullOrEmpty(ProfileInfo.Clan.Name))
                text = Localizer.GetText("lblClan");
            else
                text = string.Format("{0}: {1}", Localizer.GetText("lblClan"), ProfileInfo.Clan.Name);
        }

    }

    private void OnLanguageChanged(EventId id, EventInfo info)
    {
        UpdateLabels();
    }
}