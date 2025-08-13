using HarmonyLib;
using MelonLoader;
using UnityEngine;
using System.Collections.Generic;
#if Il2Cpp
using Il2CppScheduleOne.Misc;
#elif Mono
using ScheduleOne.Misc;
#endif

namespace Global_Light_Switch
{
    public class Core : MelonMod
    {
        public const string ModName = "Global Light Switch";
        public const string Version = "1.0.0";
        public const string ModDesc = "Toggles all lights in the game off and on with a single key";

        public static bool LightsOn = false;
        public static bool Initialized = false;
        public static readonly HashSet<ToggleableLight> AllLights = new HashSet<ToggleableLight>();
        public static MelonPreferences_Category PrefsCategory;
        public static MelonPreferences_Entry<KeyCode> PrefsKeyCode;

        public override void OnInitializeMelon() { }
        public override void OnUpdate()
        {
            if (!Initialized)
            {
                PrefsCategory = MelonPreferences.CreateCategory("GlobalLightSwitch_prefs", ModName);
                PrefsKeyCode = PrefsCategory.CreateEntry("KeyCode", default_value: KeyCode.F5, "Toggle all lights");
                Initialized = true;
            }

            if (Input.GetKeyDown(PrefsKeyCode.Value))
            {
                foreach (ToggleableLight light in AllLights)
                    if (LightsOn) light.TurnOn();
                    else light.TurnOff();
                LightsOn = !LightsOn;
            }
        }
    }

    [HarmonyPatch(typeof(ToggleableLight), nameof(ToggleableLight.Awake))]
    static class ToggleableLightAwakePatch
    {
        static void Postfix (ToggleableLight __instance)
        {
            if (__instance is null) return;
            Core.AllLights.Add(__instance);
        }
    }
}