using System.Reflection;
using HarmonyLib;
using VRage.Plugins;

namespace thejoun.JetpackGravityAlignment
{
    public class Plugin : IPlugin
    {
        public void Init(object gameObject)
        {
            new Harmony("JetpackGravityAlignment").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Update()
        {
            
        }

        public void Dispose()
        {
        }
    }
}