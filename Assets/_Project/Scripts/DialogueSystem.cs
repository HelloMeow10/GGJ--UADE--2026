using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public DataDialogue dialogoActual;
    public int lineaActual;
    public GameObject zonaDeDialogo;
    public TMP_Text textoDeDialogo;
    public GameObject zonaDeActor;
    public TMP_Text nombreDelActor;
    public DataDialogue[] dialogos;
    public CanvasGroup BotonDeContinuar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void IniciarDialogo(DataDialogue Dialogo)
    {
        Debug.Log("Dialogo iniciado");
        dialogoActual = Dialogo;
        lineaActual = 0;
        ActivarCanvasgroup(BotonDeContinuar);
        zonaDeDialogo.SetActive(true);
        zonaDeActor.SetActive(true);
        MostrarDialogo();
    }
    public void TerminarDialogo()
    {
        zonaDeDialogo.SetActive(false);
        zonaDeActor.SetActive(false);
        desactivarCanvasgroup(BotonDeContinuar);
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
    public void MostrarDialogo(){
        textoDeDialogo.text = dialogoActual.Dialogue[lineaActual].text;
        nombreDelActor.text = dialogoActual.Dialogue[lineaActual].talker;
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
