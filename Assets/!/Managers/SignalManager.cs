using System.Collections.Generic;
using _.Enums;
using _.Interfaces;
using UnityEngine;

namespace _.Managers
{
    public class SignalManager : MonoBehaviour
    {
        private Dictionary<Signal, List<IReceiver>> _receivers = new();
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        public void RegisterReceiver(IReceiver r, Signal s) => _receivers[s].Add(r);
        public void UnregisterReceiver(IReceiver r, Signal s) => _receivers[s].Remove(r);

        public void SendSignal(Signal s)
        {
            foreach (var r in _receivers[s]) r.Receive(s);
        }

        public void ClearReceivers() => _receivers.Clear();
    }
}
