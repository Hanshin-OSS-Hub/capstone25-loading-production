using System;
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public class Recorder : MonoBehaviour
{
    RecorderController m_Controller;
    bool m_IsRecording = false;

    [Header("공통 설정")]
    public int width = 1920;
    public int height = 1080;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) ToggleRecording();
        if (Input.GetKeyDown(KeyCode.P)) TakeScreenshot();
    }

    void ToggleRecording()
    {
        if (!m_IsRecording)
        {
            var settings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            var video = ScriptableObject.CreateInstance<MovieRecorderSettings>();
            
            video.name = "Video_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            video.Enabled = true;

            foreach (var input in video.InputsSettings)
            {
                if (input is GameViewInputSettings gameView)
                {
                    gameView.OutputWidth = width;
                    gameView.OutputHeight = height;
                }
            }

            settings.AddRecorderSettings(video);
            settings.SetRecordModeToManual();

            m_Controller = new RecorderController(settings);
            m_Controller.PrepareRecording();
            m_Controller.StartRecording();
            m_IsRecording = true;
            Debug.Log("<color=red>● 녹화 시작</color>");
        }
        else
        {
            m_Controller?.StopRecording();
            m_IsRecording = false;
            Debug.Log("■ 녹화 종료 및 저장");
        }
    }

    void TakeScreenshot()
    {
        var settings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        var image = ScriptableObject.CreateInstance<ImageRecorderSettings>();

        image.name = "Shot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        image.Enabled = true;

        foreach (var input in image.InputsSettings)
        {
            if (input is GameViewInputSettings gameView)
            {
                gameView.OutputWidth = width;
                gameView.OutputHeight = height;
            }
        }

        settings.AddRecorderSettings(image);
        settings.SetRecordModeToSingleFrame(0);

        var shotController = new RecorderController(settings);
        shotController.PrepareRecording();
        shotController.StartRecording();
        Debug.Log("<color=yellow>★ 스크린샷 저장</color>");
    }
}