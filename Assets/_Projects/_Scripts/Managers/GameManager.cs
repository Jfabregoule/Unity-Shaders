using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string SOUND_MANAGER_TAG = "SoundManager";

    public Character Player;
    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public SoundManager SoundManager;

    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        Player.Init();

        SoundManager = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundManager>();
        SoundManager.SetAudioListener(MainCamera.GetComponent<AudioListener>());
        SoundManager.SetPlayer(Player.gameObject);
        SoundManager.ChangeMusicByKey("Peaceful");
        SoundManager.AddAmbianceSoundByKey("Wind");

        InputManager.Instance.EnableAllControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
