using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawn: MonoBehaviour
{
    [SerializeField] private GameObject roomManagerPrefab;

    void Start()
    {
        Instantiate(roomManagerPrefab);
    }

    
}
