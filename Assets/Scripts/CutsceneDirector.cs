using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneDirector : MonoBehaviour
{
    public enum Speaker { Bobby, Angler, Monger, Gillgamesh }

    [Serializable]
    public class DialogLine
    {
        public Speaker speaker;
        [TextArea] public string text;
    }

    [SerializeField] private DialogBox landBox;
    [SerializeField] private DialogBox waterBox;

    [Header("Speaker Transforms")]
    [SerializeField] private Transform bobbySpeaker;
    [SerializeField] private Transform anglerSpeaker;
    [SerializeField] private Transform mongerSpeaker;
    [SerializeField] private Transform gillgameshSpeaker;

    [SerializeField] private float bobAmount = 0.15f;
    [SerializeField] private float bobSpeed = 10f;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform boatCameraTarget;

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

    private DialogBox BoxForSpeaker(Speaker speaker)
    {
        return speaker == Speaker.Gillgamesh ? waterBox : landBox;
    }

    private Transform TransformForSpeaker(Speaker speaker)
    {
        return speaker switch
        {
            Speaker.Bobby      => bobbySpeaker,
            Speaker.Angler     => anglerSpeaker,
            Speaker.Monger     => mongerSpeaker,
            Speaker.Gillgamesh => gillgameshSpeaker,
            _                  => null,
        };
    }

    private IEnumerator PlayRoutine(DialogLine[] lines, Action onComplete)
    {
        IsPlaying = true;
        if (_playerScript != null) _playerScript.inputEnabled = false;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        Transform originalFollow = virtualCamera != null ? virtualCamera.Follow : null;

        foreach (DialogLine line in lines)
        {
            DialogBox box = BoxForSpeaker(line.speaker);
            Transform speakerTransform = TransformForSpeaker(line.speaker);

            if (virtualCamera != null)
                virtualCamera.Follow = line.speaker == Speaker.Gillgamesh
                    ? _playerScript?.transform
                    : boatCameraTarget;

            box.ShowLine(line.text);

            Vector3 speakerOrigin = speakerTransform != null ? speakerTransform.localPosition : Vector3.zero;
            Coroutine bob = speakerTransform != null ? StartCoroutine(BobRoutine(speakerTransform)) : null;

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

            if (bob != null) StopCoroutine(bob);
            if (speakerTransform != null) speakerTransform.localPosition = speakerOrigin;

            box.Hide();
            yield return new WaitForSecondsRealtime(0.1f);
        }

        if (virtualCamera != null) virtualCamera.Follow = originalFollow;
        Time.timeScale = originalTimeScale;
        if (_playerScript != null) _playerScript.inputEnabled = true;
        IsPlaying = false;
        onComplete?.Invoke();
    }

    private IEnumerator BobRoutine(Transform target)
    {
        Vector3 origin = target.localPosition;
        float t = 0f;
        while (true)
        {
            t += Time.unscaledDeltaTime * bobSpeed;
            target.localPosition = origin + Vector3.up * Mathf.Sin(t) * bobAmount;
            yield return null;
        }
    }
}
