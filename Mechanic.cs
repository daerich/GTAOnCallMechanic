/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using System;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Text;

namespace OnCallMechanic
{
    internal class Mechanic
    {
        internal Vehicle MCar { get; set; }
        private Vector3 Spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(50f));
        private Vector3 Position;
        private Blip blip;
        internal Ped PMechanic { get; set; }
        private static Ped Player = Game.LocalPlayer.Character;
        
        internal bool isDriving { get; set; }
        internal bool hasArrived { get; set; }

        internal Mechanic()
        {
            Position = Player.Position;
            PMechanic = new Ped("s_m_y_xmech_01", Spawn, 0f)
            {
                IsInvincible = true,
                IsPersistent = true,
                BlockPermanentEvents = true
            };


            MCar = new Vehicle("blista", Spawn)
            {
                IsInvincible = true,
                IsPersistent = true
            };
            PMechanic.WarpIntoVehicle(MCar, (int)VehicleSeat.Driver);
            blip = MCar.AttachBlip();
            blip.IsFriendly = true;
            

        }

        internal void Dispatch()
        {
            PMechanic.Tasks.DriveToPosition(MCar, Position, 20f, VehicleDrivingFlags.Emergency, 10f);
            isDriving = true;
            GameFiber.Yield();
        }

        internal void Repair()
        {
            PMechanic.Detach();
            PMechanic.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
            PMechanic.Tasks.GoStraightToPosition(Player.LastVehicle.Position, 20f, Player.LastVehicle.Heading, 0, 0);
            PMechanic.Tasks.Cower(1000);
            PMechanic.Tasks.Clear();
            Player.LastVehicle.Repair();
            GameFiber.Yield();
        }

        internal void LeaveAssignment()
        {
            PMechanic.Tasks.EnterVehicle(MCar,(int)VehicleSeat.Driver);
            PMechanic.Tasks.CruiseWithVehicle(20f);
            GameFiber.Yield();
        }
        internal void Dismiss()
        {
            if(PMechanic.Exists()){ PMechanic.Dismiss(); }
            if(MCar.Exists()) { MCar.Dismiss(); }
            if (blip.Exists()) { blip.Delete(); }

            GameFiber.Yield();
        }

    }
}
