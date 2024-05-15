using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class BoolExtension {

        public static string GetLangName(this bool value) {
            var states = SharpEssentials._Config.Language.ToggleStates;
            return value ? states.Enabled : states.Disabled;
        }

        

    }

}