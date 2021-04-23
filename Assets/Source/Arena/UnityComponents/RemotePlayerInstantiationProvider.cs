using System;
using UnityEngine;

namespace Source.Arena.UnityComponents
{
    public class RemotePlayerInstantiationProvider : MonoBehaviour
    {
        [HideInInspector] public PlayerBehaviour remotePlayerBehaviour;
        
        public event Action OnRemotePlayerInstantiated;

        public void OnRemotePlayerInstantiatedInvoke(PlayerBehaviour obj)
        {
            remotePlayerBehaviour = obj;
            OnRemotePlayerInstantiated?.Invoke();
        }
    }
}