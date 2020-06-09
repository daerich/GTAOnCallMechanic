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

        internal MechAction()
        {
            mechanic = new Mechanic();
        }
        internal void Drive()
        {
          
            GameFiber.StartNew(Checks);

            mechanic.Dispatch();
            Arrived += (object sender, EventArgs e) => {

                mechanic.Repair();
                Game.DisplaySubtitle("[Mechanic] Aight let's get to work!");
                
            };
            Repaired += (object sender, EventArgs e) =>
            {
                Game.DisplaySubtitle("[Mechanic] There you go!");
                mechanic.LeaveAssignment();
                GameFiber.Sleep(10000);
                mechanic.Dismiss();
            };

           /* Left += (object sender, EventArgs e) =>
            {
                mechanic.Dismiss();
            };*/



        }

        private void Checks()
        {
            while (true)
            {
                EventChecks();
                GameFiber.Yield();
            }
            
        }

       private void EventChecks()
        {
            if (mechanic.PMechanic.DistanceTo(Game.LocalPlayer.Character) <= 10f && mechanic.PMechanic.Speed <= 0.1f)
            {
                mechanic.isDriving = false;
                Arrived(this, EventArgs.Empty);

            }
            if (Game.LocalPlayer.Character.LastVehicle.Health == 1000)
            {
                Repaired(this, EventArgs.Empty);
            }
        }
        internal event EventHandler Arrived;
        internal event EventHandler Repaired;
       // internal event EventHandler Left;



    }
}

