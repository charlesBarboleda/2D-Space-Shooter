using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{
    void RegisterEventHandlers();
    void UnregisterEventHandlers();
    bool isCompleted { get; }
}
