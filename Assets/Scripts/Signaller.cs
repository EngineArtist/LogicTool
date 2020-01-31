using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public struct SignalOutput {
    public GameObject target;
    public string slot;
}


public struct SignalInput {
    public string name;
    public UnityEvent callback;
}


public class Signaller: MonoBehaviour {
    public List<(GameObject target, string slot)> outputs;
}