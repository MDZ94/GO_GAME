using UnityEngine;
public interface ITokenManager
{
    void RemoveToken(Vector3 vector);
    void SpawnToken(int index, Vector3 vector);
    void FactoryInit();
}