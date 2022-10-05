using FMODUnity;
using UnityEngine;

namespace CyberNinja.Views.Unit
{
    public class UnitAnimatorEventsView : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter fmodFootsteps;

        private void EventAudioPlayFootsteps() => fmodFootsteps.Play();
    }
}