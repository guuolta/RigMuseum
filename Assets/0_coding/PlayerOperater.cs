using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// プレイヤーの動き
/// </summary>
public class PlayerOperater : MonoBehaviour
{
    [Header("プレイヤーが移動するスピード")]
    [SerializeField]
    private float _speed;
    [Header("プレイヤーのオブジェクト(ex:カメラ)")]
    [SerializeField]
    private GameObject _player;
    [Header("上昇移動のキー")]
    [SerializeField]
    private KeyCode _upMoveKey = KeyCode.Space;
    [Header("下降移動のキー")]
    [SerializeField]
    private KeyCode _downMoveKey = KeyCode.LeftShift;

    private void Awake()
    {
    }

    private void Start()
    {
        SetEventKey();
    }

    private void SetEventKey()
    {
        /*前後移動の処理*/
        this.UpdateAsObservable()
            .Where(_ => Input.anyKey && Input.GetAxis("Vertical") != 0)
            .Select(direction => Input.GetAxis("Vertical"))
            .Subscribe(direction =>
            {
                _player.transform.position += _player.transform.forward * _speed * direction;
            }).AddTo(this);

        /*左右移動の処理*/
        this.UpdateAsObservable()
            .Where(_ => Input.anyKey && Input.GetAxis("Horizontal") != 0)
            .Select(direction => Input.GetAxis("Horizontal"))
            .Subscribe(direction =>
            {
                 _player.transform.position += _player.transform.right * _speed * direction;
                
            }).AddTo(this);

        /*上昇移動の処理*/
        this.UpdateAsObservable()
            .Where(_ => Input.anyKey && Input.GetKey(_upMoveKey))
            .Subscribe(_ => {
                _player.transform.position += _player.transform.up * _speed;
            }).AddTo(this);

        /*下降処理の処理*/
        this.UpdateAsObservable()
            .Where(_ => Input.anyKey && Input.GetKey(_downMoveKey))
            .Subscribe(_ => {
                _player.transform.position -= _player.transform.up * _speed;
            }).AddTo(this);
    }
}
