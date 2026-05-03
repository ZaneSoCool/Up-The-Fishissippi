using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneDirector : MonoBehaviour
{
    public enum Speaker { Land, Water }

    [Serializable]
    public class DialogLine
    {
        public Speaker speaker;
        [TextArea] public string text;
    }

    [SerializeField] private DialogBox landBox;
    [SerializeField] private DialogBox waterBox;

    public bool IsPlaying { get; private set; }

    private player _playerScript;
    private InputAction _interactAction;

    private void Start()
    {
        _playerScript = GameObject.FindWithTag("Player")?.GetComponent<player>();
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    // onComplete fires after the last line is dismissed
    public void Play(DialogLine[] lines, Action onComplete = null)
    {
        if (IsPlaying || lines == null || lines.Length == 0)
        {
            onComplete?.Invoke();
            return;
        }
        StartCoroutine(PlayRoutine(lines, onComplete));
    }

    private IEnumerator PlayRoutine(DialogLine[] lines, Action onComplete)
    {
        IsPlaying = true;
        if (_playerScript != null) _playerScript.inputEnabled = false;

        foreach (DialogLine line in lines)
        {
            DialogBox box = line.speaker == Speaker.Land ? landBox : waterBox;
            box.ShowLine(line.text);

            // Skip one frame so the input that triggered Play() doesn't immediately advance
            yield return null;

            // Wait for player to press Interact
            bool advanced = false;
            while (!advanced)
            {
                if (_interactAction != null && _interactAction.WasPressedThisFrame())
                    advanced = true;
                yield return null;
            }

            box.Hide();
            yield return new WaitForSeconds(0.1f);
        }

        if (_playerScript != null) _playerScript.inputEnabled = true;
        IsPlaying = false;
        onComplete?.Invoke();
    }
}
