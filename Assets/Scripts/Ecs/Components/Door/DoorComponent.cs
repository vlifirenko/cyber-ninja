using CyberNinja.Views;
using UnityEngine;

namespace CyberNinja.Ecs.Components.Door
{
    public struct DoorComponent
    {
        public bool IsInteractable;
        public DoorView View;
        public Transform TargetDoor;
    }
}