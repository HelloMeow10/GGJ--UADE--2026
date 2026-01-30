using System;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] 
    private CanvasGroup habitacionRenderizada;
    public DataDialogue dialogo;
    public DialogueSystem sistemaDeDialogo;
    [SerializeField]
    private Character personaje1;
    [SerializeField]
    private Character personaje2;
    public void Entrar(){
        ActivarCanvasgroup(habitacionRenderizada);

        sistemaDeDialogo.dialogoActual = dialogo;
        sistemaDeDialogo.personaje1 = personaje1;
        sistemaDeDialogo.personaje2 = personaje2;
        sistemaDeDialogo.HabilitarSistema();
    }
    public void Salir(){
        DesactivarCanvasgroup(habitacionRenderizada);

        sistemaDeDialogo.DeshabilitarSistema();
    }
    public void DesactivarCanvasgroup(CanvasGroup canvasGroup){
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
