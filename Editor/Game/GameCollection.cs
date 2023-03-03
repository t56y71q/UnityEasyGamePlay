using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyGamePlay.Editor
{
    public class GameColloction
    {
        private Type type;

        public void Init()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly;
            for (int i = 0; i < assemblies.Length; i++)
            {
                assembly = assemblies[i];
                if (assembly != currentAssembly && assembly.GetName().Name != "EasyGamePlay" && HasEasyGamePlay(assembly))
                {
                    Type gameType = typeof(AGame);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!type.IsAbstract && type.IsSubclassOf(gameType))
                        {
                            this.type = type;
                            return;
                        }
                    }
                }
            }
        }

        private bool HasEasyGamePlay(Assembly assembly)
        {
            var refs = assembly.GetReferencedAssemblies();
            for (int i = 0; i < refs.Length; i++)
            {
                if (refs[i].Name == "EasyGamePlay")
                {
                    return true;
                }
            }
            return false;
        }

        public Type GetGameType()
        {
            return type;
        }

        public string[] GetNames()
        {
            string[] names = new string[1];
            names[0] = type.Name;
            return names;
        }
    }
}
