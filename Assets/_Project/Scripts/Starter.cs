using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Starter : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic(music);
    }
}
