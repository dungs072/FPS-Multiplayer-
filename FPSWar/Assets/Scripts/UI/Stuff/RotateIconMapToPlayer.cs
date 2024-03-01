using UnityEngine;
using Mirror;
public class RotateIconMapToPlayer : MonoBehaviour
{
    private PlayerController playerController;
    private void Start() {
        playerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
    }
    
}
