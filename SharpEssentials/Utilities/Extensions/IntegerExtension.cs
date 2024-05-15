﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class IntegerExtension {

        public static int Clamp(this int value, int min, int max) {
            if(value > max) {
                return max;
            } else if(value < min) {
                return min;
            } else {
                return value;
            }
        }

    }

}