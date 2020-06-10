﻿/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using Rage;
using System;

namespace OnCallMechanic.API
{
    internal class MechAction
    {
        private Mechanic mechanic;
        private bool CanBeCleaned;

        internal MechAction()
        {
            mechanic = new Mechanic();
        }
        internal void Drive()
        {
            if (!Game.LocalPlayer.Character.LastVehicle.Exists() || !Game.LocalPlayer.Character.LastVehicle.IsValid())
            {
                Game.DisplaySubtitle("[Mechanic] I'm sorry I could not pinpoint your vehicle!");
                mechanic.Dismiss();
            }
            else
            {
                GameFiber.StartNew(Checks, "EventChecker");
                mechanic.Dispatch();
                Arrived += (object sender, EventArgs e) =>
                {


                    Game.DisplaySubtitle("[Mechanic] Aight let's get to work!");
                    mechanic.Repair();
                    GameFiber.Yield();

                };
                Repaired += (object sender, EventArgs e) =>
                {
                    Game.DisplaySubtitle("[Mechanic] There you go!");
                    mechanic.LeaveAssignment();
                    mechanic.Dismiss();
                    GameFiber.Yield();
                };
            }

            /* Left += (object sender, EventArgs e) =>
             {
                 mechanic.Dismiss();
             };*/



        }

        private void Checks()
        {
            while(!CanBeCleaned)
            {
                EventChecks();
                GameFiber.Yield();
            }
            
        }

       private void EventChecks()
        {
            if (mechanic.PMechanic.DistanceTo(Game.LocalPlayer.Character.LastVehicle) <= 10f && mechanic.PMechanic.Speed <= 0.1f)
            {

                Arrived(this, EventArgs.Empty);

            }

            if (Game.LocalPlayer.Character.LastVehicle.Health == 1000)
            {
                CanBeCleaned = true;
                Repaired(this, EventArgs.Empty);
            }
        }
        internal event EventHandler Arrived;
        internal event EventHandler Repaired;
       // internal event EventHandler Left;



    }
}

