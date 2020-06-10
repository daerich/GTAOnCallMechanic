/* ------------------------------------------
			COPYRIGHT © DAERICH 2020
ALL RIGHTS RESERVED EXCEPT OTHERWISE STATED IN COPYRIGHT.TXT
   ------------------------------------------ */
using System.Reflection;
using Rage;
using LSPD_First_Response.Mod.API;
using OnCallMechanic.API;


namespace OnCallMechanic
{
    class Main : Plugin
    {
        public override void Finally()
        {
            
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyHandler;
        }

        private void OnDutyHandler(bool onDuty)
        {
            if (onDuty && PoliceSmartRadioPresent())
            {
                Game.DisplayNotification($"Car Mechanic Version {Assembly.GetExecutingAssembly().GetName().Version} loaded!");
                SmartRadio.Initialize();

            }
            else if (!PoliceSmartRadioPresent())
            {
                Game.DisplayNotification("PoliceSmartRadio not present!");
            }

        }

        private bool PoliceSmartRadioPresent()
        {
           foreach(Assembly assembly in Functions.GetAllUserPlugins())
            {
              if(assembly.GetName().Name == "PoliceSmartRadio")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
