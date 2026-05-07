using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다.");
        }
    }

    void LateUpdate()
    {
        if (_mainCamera == null) return;

        // 카메라 방향을 바라보도록 회전
        transform.forward = _mainCamera.transform.forward;
    }
}