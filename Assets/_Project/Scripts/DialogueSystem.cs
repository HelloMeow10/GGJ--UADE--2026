using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [HideInInspector]
    public DataDialogue dialogoActual;
    private int lineaActual;
    [SerializeField]
    private CanvasGroup zonaDeDialogo;
    [SerializeField]
    private CanvasGroup BotonDeContinuar;
    [SerializeField]
    private CanvasGroup BotonDeIniciar;
    [SerializeField]
    private TMP_Text textoDeDialogo;
    [SerializeField]
    private TMP_Text nombreDelActor;

    public void HabilitarSistema()
    {
        ActivarCanvasgroup(BotonDeIniciar);
    }
    public void DeshabilitarSistema()
    {
        DesactivarCanvasgroup(BotonDeIniciar);
        DesactivarCanvasgroup(zonaDeDialogo);
        DesactivarCanvasgroup(BotonDeContinuar);
    }
    public void IniciarDialogo()
    {
        Debug.Log("Dialogo iniciado");
        lineaActual = 0;

        DesactivarCanvasgroup(BotonDeIniciar);
        ActivarCanvasgroup(BotonDeContinuar);
        ActivarCanvasgroup(zonaDeDialogo);

        MostrarDialogo();
    }
    public void TerminarDialogo()
    {
        ActivarCanvasgroup(BotonDeIniciar);
        DesactivarCanvasgroup(zonaDeDialogo);
        DesactivarCanvasgroup(BotonDeContinuar);
    }
    public void AvanzarDialogo()
    {
        lineaActual++;
        if (lineaActual >= dialogoActual.Dialogue.Count)
        {
            TerminarDialogo();
        }
        else
        {
            MostrarDialogo();
        }

    }
    public void MostrarDialogo()
    {
        textoDeDialogo.text = dialogoActual.Dialogue[lineaActual].text;
        nombreDelActor.text = dialogoActual.Dialogue[lineaActual].talker;
    }
    public void DesactivarCanvasgroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void ActivarCanvasgroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

}
