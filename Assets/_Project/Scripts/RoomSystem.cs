using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSystem : MonoBehaviour
{
    [SerializeField] private RoomScript habitacionActual;
    [SerializeField] private CanvasGroup selectorDeHabitacion;
    [SerializeField] private CanvasGroup botonSalirDeHabitacion;

    public void SetRoom(RoomScript room)
    {
        habitacionActual = room;
        habitacionActual.Entrar();
        desactivarCanvasgroup(selectorDeHabitacion);
        ActivarCanvasgroup(botonSalirDeHabitacion);
    }
    public void SalirDeLaHabitacion()
    {
        habitacionActual.Salir();
        ActivarCanvasgroup(selectorDeHabitacion);
        desactivarCanvasgroup(botonSalirDeHabitacion);
    }

    public void desactivarCanvasgroup(CanvasGroup canvasGroup){
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void ActivarCanvasgroup(CanvasGroup canvasGroup){
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
