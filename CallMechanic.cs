/* ------------------------------------------
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
        private int counter;
        private bool HasArrived;
        internal MechAction()
        {
            mechanic = new Mechanic();
            counter = 0;
        }
        internal void Drive()
        {
            if (Game.LocalPlayer.Character.LastVehicle.Exists())
            {
                GameFiber.StartNew(Checks, "EventChecker");
                GameFiber.StartNew(Counter, "Counter");
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
                    if (mechanic.PMechanic.IsInAnyVehicle(false))
                    {
                        mechanic.LeaveAssignment();
                    }
                    mechanic.Dismiss();
                    GameFiber.Yield();
                };
                Broken += (object sender, EventArgs e) =>
                {
                    Game.DisplaySubtitle("*The Mechanic has been held up in traffic and never arrived*");
                    mechanic.PMechanic.Tasks.Clear();
                    mechanic.Dismiss();
                };
            }
            else
            {
                Game.DisplaySubtitle("[Mechanic] I'm sorry I could not pinpoint your vehicle!");
                mechanic.Dismiss();

            }

        }

        private void Counter()
        {
            while (!CanBeCleaned && mechanic.isValid())
            {
                counter++;
                GameFiber.Sleep(1000);
            }
        }

        private void Checks()
        {
            while (!CanBeCleaned && mechanic.isValid() && !HasArrived)
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
                HasArrived = true;

            }

            if (Game.LocalPlayer.Character.LastVehicle.Health == 1000)
            {
                CanBeCleaned = true;
                Repaired(this, EventArgs.Empty);
            }
            if(counter == 20)
            {
                CanBeCleaned = true;
                Broken(this, EventArgs.Empty);
            }

        }
        internal event EventHandler Arrived;
        internal event EventHandler Repaired;
        internal event EventHandler Broken;

    }
}