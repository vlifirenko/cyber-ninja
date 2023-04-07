using System;
using CyberNinja.Views.Projectile;

namespace CyberNinja.Models
{
    [Serializable]
    public class DroidShoot
    {
        public DroidProjectile projectile;
        public Target target;
    }
}