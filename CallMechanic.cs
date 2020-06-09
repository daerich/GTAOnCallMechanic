/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using Rage;
using System;
using OnCallMechanic;

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

             GameFiber.StartNew(Checks, "EventChecker");

   
            mechanic.Dispatch();
            Arrived += (object sender, EventArgs e) => {
               
                
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

           /* Left += (object sender, EventArgs e) =>
            {
                mechanic.Dismiss();
            };*/



        }

        private void Checks()
        {
            while(true)
            {
                if (CanBeCleaned)
                {
                    break;
                }
                EventChecks();
                GameFiber.Yield();
            }
            
        }

       private void EventChecks()
        {
            DateTime time = DateTime.Now;
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
