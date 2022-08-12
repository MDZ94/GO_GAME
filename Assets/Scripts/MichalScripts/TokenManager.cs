using UnityEngine;
using System.Collections.Generic;


public class TokenManager :MonoBehaviour, ITokenManager 
{
    private BoardManager manager;
    public static TokenManager Instance;
    public List<GameObject> tokenPrefabs;





    public void RemoveToken(Vector3 vector)
    {
        if (manager.Tokens[(int)vector.x, (int)vector.z] != null)
        {
            Destroy(manager.Tokens[(int)vector.x, (int)vector.z].gameObject);
        }
        manager.Tokens[(int)vector.x, (int)vector.z] = null;
    }
        public void SpawnToken(int index, Vector3 vector)
    {
        GameObject go = Instantiate(tokenPrefabs[index], vector, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        manager.Tokens[(int)vector.x, (int)vector.z] = go.GetComponent<Token>();
        manager.Tokens[(int)vector.x, (int)vector.z].SetPosition((int)vector.x, (int)vector.z);

    }
        public void FactoryInit(){
        manager = this.GetComponent<BoardManager>();
        Instance = this;
    }
}