﻿/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using Rage;

namespace OnCallMechanic.API
{
    internal static class SmartRadio
    {
        internal static void Initialize()
        {
            PoliceSmartRadio.API.Functions.AddActionToButton(Main, "CallMechanic");

        }

        internal static void Main()
        {
            MechAction mechAction = new MechAction();
            GameFiber.StartNew(mechAction.Drive);
        }

    }
}