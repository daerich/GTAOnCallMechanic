/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace OnCallMechanic
{
    internal class Mechanic
    {
        internal Vehicle MCar { get; private set; }


        private Vector3 Spawn = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(50f));
        private Vector3 Position;
        private Blip blip;
        internal Ped PMechanic { get; private set; }
        private  Ped Player = Game.LocalPlayer.Character;
    

        internal Mechanic()
        {
            Position = Player.Position;
            PMechanic = new Ped("s_m_y_xmech_01", Spawn, 0f)
            {
                IsInvincible = true,
                IsPersistent = true,
                IsFireProof = true,
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
            PMechanic.Tasks.DriveToPosition(MCar, Position, 15f, VehicleDrivingFlags.Emergency, 5f);
        }

        internal void Repair()
        {
            PMechanic.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion();
            MCar = PMechanic.LastVehicle;
            PMechanic.WarpIntoVehicle(Player.LastVehicle, (int)VehicleSeat.Driver);
            GameFiber.Sleep(2000);
            PMechanic.Tasks.Clear();
            Player.LastVehicle.Repair();
        }

        internal void LeaveAssignment()
        {
            PMechanic.WarpIntoVehicle(MCar, (int)VehicleSeat.Driver);
            PMechanic.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.Normal);
        }

        internal bool isValid()
        {
            return PMechanic.IsValid() && MCar.IsValid();
        }
        internal void Dismiss()
        {
            if(PMechanic.Exists()){ PMechanic.Dismiss(); }
            if(MCar.Exists()) { MCar.Dismiss(); }
            if (blip.Exists()) { blip.Delete(); }

        }

    }
}