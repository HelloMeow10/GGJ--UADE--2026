using UnityEngine;

public interface INotepadEntry
{
    Transform SelfTransform { get; }
    string GetText();
    
    void SetText(string text);

    void OnAssignButtonPressed();
    void OnModifyButtonPressed();
    void OnDeleteButtonPressed();
}