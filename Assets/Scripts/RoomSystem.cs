using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
    private int currentRoom = 0;

    void Start()
    {
        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogWarning("No rooms assigned");
            return;
        }

        SetRoom(currentRoom);
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeRoom(1);
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeRoom(-1);
    }
    private void ChangeRoom(int direction)
    {
        rooms[currentRoom].SetActive(false);

        currentRoom = (currentRoom + direction + rooms.Length) % rooms.Length;

        rooms[currentRoom].SetActive(true);
    }

    private void SetRoom(int index)
    {
        foreach (var room in rooms)
            room.SetActive(false);

        rooms[index].SetActive(true);
    }
}
