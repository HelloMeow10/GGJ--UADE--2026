using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] 
    private CanvasGroup habitacionRenderizada;
    [SerializeField] 
    private CanvasGroup pantallaDeDialogo;
    public DataDialogue dialogo;
    public void Entrar(){
        ActivarCanvasgroup(habitacionRenderizada);
        ActivarCanvasgroup(pantallaDeDialogo);
    }
    public void Salir(){
        desactivarCanvasgroup(habitacionRenderizada);
        desactivarCanvasgroup(pantallaDeDialogo);
    }
    public void desactivarCanvasgroup(CanvasGroup canvasGroup){
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
    public void ActivarCanvasgroup(CanvasGroup canvasGroup){
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }
}
