using System.Collections.Generic;
using CyberNinja.Models;
using CyberNinja.Models.Unit;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct TargetsComponent
    {
        public List<Target> Targets;
    }
}