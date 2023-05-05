using System;
using CyberNinja.Views.Projectile;

namespace CyberNinja.Models.Unit
{
    [Serializable]
    public class DroidShoot
    {
        public DroidProjectile projectile;
        public Target target;
    }
}