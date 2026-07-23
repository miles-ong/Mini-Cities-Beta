using System.Collections.Generic;
using _.Enums;
using _.Interfaces;
using UnityEngine;

namespace _.Managers
{
    public class SignalManager : MonoBehaviour
    {
        private Dictionary<Signal, IEnumerable<IReceiver>> _receivers = new();
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
