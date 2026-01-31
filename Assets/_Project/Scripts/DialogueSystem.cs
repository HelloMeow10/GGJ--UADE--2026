using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [HideInInspector]
    public DataDialogue dialogoActual;
    [HideInInspector]
    public Character personaje1;
    [HideInInspector]
    public Character personaje2;
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
        
        personaje1.Iluminar();
        personaje2.Iluminar();
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
        string talker = dialogoActual.Dialogue[lineaActual].Talker.ToString();
        string text = dialogoActual.Dialogue[lineaActual].Line.GetLocalizedString();
        textoDeDialogo.text = text;
        nombreDelActor.text = talker;

        if(talker == personaje1.talkerName)
        {
            personaje1.Hablar();
            personaje1.Iluminar();
            personaje2.Oscurecer();
        }
        else if(talker == personaje2.talkerName)
        {
            personaje2.Hablar();
            personaje2.Iluminar();
            personaje1.Oscurecer();
        }
        else{
            personaje1.Oscurecer();
            personaje2.Oscurecer();
        }
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
