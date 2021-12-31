using System;
using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    public PlayerController player;
    public Transform playerModel;
    public Transform cameraPosition;
    public Transform prepareCam;
    public Transform mainGameCam;
    public Transform finishCam;
    public Transform gameOverCam;


    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        CamFollow();
    }


    private void CamFollow()
    {
        if (!player.finishCam && GameManager.Instance.CurrentGameState != GameState.GameOver &&
            GameManager.Instance.CurrentGameState != GameState.Prepare)
        {
            if (_camera.transform.parent != mainGameCam)
            {
                _camera.transform.SetParent(mainGameCam);
            }

            cameraPosition.position =
                Vector3.Lerp(cameraPosition.position, player.transform.position, Time.deltaTime * 2);

            _camera.transform.localRotation =
                Quaternion.Lerp(_camera.transform.localRotation, Quaternion.identity, Time.deltaTime * 2);
        }

        else if (!player.finishCam && GameManager.Instance.CurrentGameState == GameState.GameOver)
        {
            SmoothGameOver();
        }

        else if (player.finishCam && GameManager.Instance.CurrentGameState == GameState.FinishGame)
        {
            playerModel.transform.rotation = new Quaternion(0, 180, 0, 0);
            SmoothFinish();
        }

        else if (!player.finishCam && GameManager.Instance.CurrentGameState == GameState.Prepare)
        {
            if (_camera.transform.parent != prepareCam)
            {
                _camera.transform.SetParent(prepareCam);
            }
        }

        if (GameManager.Instance.CurrentGameState == GameState.Prepare ||
            GameManager.Instance.CurrentGameState == GameState.MainGame)
        {
            _camera.transform.localPosition =
                Vector3.Lerp(_camera.transform.localPosition, Vector3.zero, Time.deltaTime * 0.75f);
        }
    }

    private void SmoothFinish()
    {
        Quaternion calculateRotation = Quaternion.LookRotation(playerModel.position - finishCam.transform.position);

        _camera.transform.localRotation =
            Quaternion.Lerp(_camera.transform.localRotation, calculateRotation, 0.8f * Time.deltaTime);

        mainGameCam.transform.localPosition =
            Vector3.Lerp(mainGameCam.transform.localPosition, Vector3.zero, 0.8f * Time.deltaTime);

        mainGameCam.transform.localRotation = Quaternion.Lerp(mainGameCam.transform.localRotation, Quaternion.identity,
            0.8f * Time.deltaTime);

        _camera.transform.localPosition =
            Vector3.Lerp(_camera.transform.localPosition, finishCam.transform.localPosition, Time.deltaTime * 0.8f);
    }

    private void SmoothGameOver()
    {
        Quaternion calculateRotation = Quaternion.LookRotation(playerModel.position - gameOverCam.transform.position);

        _camera.transform.localRotation =
            Quaternion.Lerp(_camera.transform.localRotation, calculateRotation, 0.8f * Time.deltaTime);

        mainGameCam.transform.localPosition =
            Vector3.Lerp(mainGameCam.transform.localPosition, Vector3.zero, 0.8f * Time.deltaTime);

        mainGameCam.transform.localRotation = Quaternion.Lerp(mainGameCam.transform.localRotation, Quaternion.identity,
            0.8f * Time.deltaTime);

        _camera.transform.localPosition =
            Vector3.Lerp(_camera.transform.localPosition, gameOverCam.transform.localPosition, Time.deltaTime * 0.8f);
    }
}