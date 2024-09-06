using UnityEngine;

public interface IJoinable
{
    bool IsAbleToJoin(BuildingObject buildingObject);
    Vector3 GetJoinPoint(BuildingObject buildingObject);
    Quaternion GetJoinRotation();
}
