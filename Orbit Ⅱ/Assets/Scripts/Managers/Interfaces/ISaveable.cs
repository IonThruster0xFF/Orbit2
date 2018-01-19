using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    SaveData toSaveData();
    void fromSaveData(SaveData saveData);
}