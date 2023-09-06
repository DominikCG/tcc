//https://gamedev.stackexchange.com/questions/144575/how-to-force-keep-the-aspect-ratio-and-specific-resolution-without-stretching-th/144578#144578

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    // Set this to your target aspect ratio, eg. (16, 9) or (4, 3).
    public Vector2 targetAspect = new Vector2(9, 16);
    Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();

        if (GameObject.FindGameObjectsWithTag("cameraBG").Length <= 0)
        {
            CreateCamera();
        }
        UpdateCrop();
    }

    private void Update()
    {
        UpdateCrop();
    }

    public void CreateCamera()
    {
        GameObject camGO = new GameObject();
        Camera cameraBG;
        camGO.AddComponent<Camera>();
        cameraBG = camGO.GetComponent<Camera>();

        cameraBG.clearFlags = CameraClearFlags.Color;
        cameraBG.backgroundColor = _camera.backgroundColor;
        cameraBG.cullingMask = 0 << LayerMask.NameToLayer("Default");
        camGO.name = "BGCamera";
        camGO.tag = "cameraBG";

        camGO.transform.SetParent(gameObject.transform);
    }

    // Call this method if your window size or target aspect change.
    public void UpdateCrop()
    {
        // Determine ratios of screen/window & target, respectively.
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = targetAspect.x / targetAspect.y;

        if (Mathf.Approximately(screenRatio, targetRatio))
        {
            // Screen or window is the target aspect ratio: use the whole area.
            _camera.rect = new Rect(0, 0, 1, 1);
        }
        else if (screenRatio > targetRatio)
        {
            // Screen or window is wider than the target: pillarbox.
            float normalizedWidth = targetRatio / screenRatio;
            float barThickness = (1f - normalizedWidth) / 2f;
            _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
        }
        else
        {
            // Screen or window is narrower than the target: letterbox.
            float normalizedHeight = screenRatio / targetRatio;
            float barThickness = (1f - normalizedHeight) / 2f;
            _camera.rect = new Rect(0, barThickness, 1, normalizedHeight);
        }
    }
}
