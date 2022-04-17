using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : GravitySource
{
    [SerializeField]
    private bool _destroysBall;

    [SerializeField]
    private bool _ballCheckpoint;

    public bool DestroysBall => _destroysBall;

    public bool BallCheckpoint => _ballCheckpoint;

    private void OnCollisionEnter(Collision collision)
    {
        bool ballCollision = collision.gameObject == LevelController.instance.Ball.gameObject;
        if (DestroysBall && ballCollision)
        {
            LevelController.instance.Lose();
        }
    }
}
