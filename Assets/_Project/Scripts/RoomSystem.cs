using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSystem : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] rooms;

    public void SetRoom(int index)
    {
        foreach (var room in rooms)
            room.alpha = 0;

        rooms[index].alpha = 1;
    }
}
