﻿using System.Collections;
using UnityEngine;

public abstract class BotState
{
    public virtual float FindingPosDelay { get { return botAI.CurrentBehaviour.FindingPosDelay; } }
    public virtual bool CanSwitchToThisState { get { return true; } }

    protected BotAI botAI;
    protected VehicleController thisVehicle;

    protected BotState(BotAI botAI)
    {
        this.botAI = botAI;
        thisVehicle = botAI.ThisVehicle;
    }

    public virtual void OnStart()
    {
        botAI.OnStateChange();
    }

    public virtual void FindPositionToMove()
    {
        botAI.FindPositionToMove();
    }
}
