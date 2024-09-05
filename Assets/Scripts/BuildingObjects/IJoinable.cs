using UnityEngine;

public interface IJoinable
{
    Vector3 GetJoinPoint();
    void Join(BuildingObject buildingObject);
}
