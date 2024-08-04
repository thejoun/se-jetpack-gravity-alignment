using System;
using HarmonyLib;
using VRage.Plugins;

namespace Mod
{
    public class Plugin : IPlugin, IDisposable
    {
        private const string Name = "JetpackGravityAlignment";
        
        public void Init(object gameObject)
        {
            var harmony = new Harmony(Name);
            harmony.PatchAll();
        }

        public void Update()
        {
            
        }

        public void Dispose()
        {
        }
    }
}