using System;
using Photon.Pun;
using Source.Arena.UnityComponents;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PhotonView), typeof(PhotonTransformViewClassic))]
public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public PhotonView photonView;
    [HideInInspector] public PhotonTransformViewClassic photonTransformViewClassic;

    public bool IsLocal => photonView.IsMine;
    public bool IsRemote => !photonView.IsMine;

    public event Action OnHit;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        photonTransformViewClassic = GetComponent<PhotonTransformViewClassic>();

        if (IsRemote)
            FindObjectOfType<RemotePlayerInstantiationProvider>().OnRemotePlayerInstantiatedInvoke(this);
    }

    public void Hit() => OnHit?.Invoke();
}