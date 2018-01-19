using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ID 
{
    public static HashSet<ID> ids = new HashSet<ID>();  // all currently active IDs

    public int id = -1;
    public string className;
    public string sceneName;

    public void Init(MonoBehaviour obj) 
    {
        // validate the data for this ID
        Debug.Assert(!string.IsNullOrEmpty(className), "className is empty");
        Debug.Assert(!string.IsNullOrEmpty(sceneName), "sceneName is empty");
        Debug.Assert(!ids.Contains(this), "ID already exists: " + this);
        Debug.Assert(id >= 0, "id number should be >= zero: " + this);
        // add this ID to the list of all current IDs
        ids.Add(this);
    }

    public void OnDestroy() {
        // remove this ID from the list of all current IDs
        ids.Remove(this);
    }

    public override string ToString() {
        return "ID{sceneName: " + sceneName + ", className: " + className + ", id: " + id + "}";
    }

    // Compare two IDs
    public static bool operator == (ID left, ID right) {
        if (ReferenceEquals(left, right)) {
            return true;
        }
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) {
            return false;
        }
        return left.id == right.id && left.className == right.className && left.sceneName == right.sceneName;
    }

    // Compare two IDs
    public static bool operator != (ID left, ID right) {
        return !(left == right);
    }

    // Compare two IDs
    // https://msdn.microsoft.com/en-us/library/aa288467(v=vs.71).aspx
    public override bool Equals(object obj) 
    {
        try {
            return (bool) (this == (ID) obj);
        } catch {
            return false;
        }
    }

    // Get a hash for the ID
    // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
    public override int GetHashCode() 
    {
        unchecked//使用了unchecked则不会检查溢出
        { // Overflow is fine, just wrap
            int hash = (int) 2166136261;
            // Suitable nullity checks etc, of course :)
            hash = (hash * 16777619) ^ id.GetHashCode();
            hash = (hash * 16777619) ^ className.GetHashCode();
            hash = (hash * 16777619) ^ sceneName.GetHashCode();
            return hash;
        }
    }
}
