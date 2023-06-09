using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public string instanceID;
    public string unitName;

    public UnitInfo(string id, string name)
    {
        instanceID = id;
        unitName = name;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        UnitInfo otherUnit = (UnitInfo)obj;
        return instanceID == otherUnit.instanceID;
    }

    public override int GetHashCode()
    {
        return instanceID.GetHashCode();
    }
}