﻿using System;
using UnityEngine;

namespace CyberNinja.Models.Unit
{
    [Serializable]
    public class Damage
    {
        public float value;
        public Transform damageOrigin;
        public Vector3 push;
        public Transform attacker;
    }
}