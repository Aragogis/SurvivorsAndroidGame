﻿using UnityEngine;
using System.Collections;
using Zenject;

public class WaveManager : MonoBehaviour
{
    [Inject] private Camera _mainCamera;
    [Inject] private DiContainer _container;

    [SerializeField] private float _timeCurrentWave;
    [SerializeField] private WaveItem waveItem;

    public float offset = 3f;
    private void Start()
    {
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        while (true)
        {
            RandomPositionOutsideScreen();
            yield return new WaitForSeconds(waveItem.timeOneWave);
        }
    }
    private void RandomPositionOutsideScreen()
    {
        for (int i = 0; i < waveItem.countEnemyPerWave; i++)
        {
            Vector3 spawnPosition = GetRandomSidePosition(Random.Range(0, 4), _mainCamera);
            _container.InstantiatePrefab(waveItem.typeEnemy, spawnPosition, Quaternion.identity,null);
        }
    }
    private Vector3 GetRandomSidePosition(int choice,Camera mainCamera)
    {

        float cameraLeftBorderPoint = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane)).x;
        float cameraTopBorderPoint = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, mainCamera.nearClipPlane)).y;
        float cameraDownBorderPoint = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane)).y;
        float cameraRightBorderPoint = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, mainCamera.nearClipPlane)).x;

        float x = 0, y = 0;
        if (choice == 0) // spawn left
        {
            x = Random.Range(cameraLeftBorderPoint - offset, cameraLeftBorderPoint);
            y = Random.Range(cameraDownBorderPoint - offset, cameraTopBorderPoint + offset);
        }
        else if (choice == 1) // spawn top
        {
            x = Random.Range(cameraLeftBorderPoint - offset, cameraRightBorderPoint + offset);
            y = Random.Range(cameraTopBorderPoint, cameraTopBorderPoint + offset);
        }
        else if (choice == 2) // spawn right
        {
            x = Random.Range(cameraRightBorderPoint, cameraRightBorderPoint + offset);
            y = Random.Range(cameraDownBorderPoint - offset, cameraTopBorderPoint + offset);
        }
        else if (choice == 3) // spawn down
        {
            x = Random.Range(cameraLeftBorderPoint - offset, cameraRightBorderPoint + offset);
            y = Random.Range(cameraDownBorderPoint - offset, cameraDownBorderPoint);
        }

        return new Vector3(x, y, 0f);
    }
}
