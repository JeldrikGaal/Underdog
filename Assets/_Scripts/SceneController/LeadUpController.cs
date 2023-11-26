using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeadUpController : MonoBehaviour
{
    
    [SerializeField] private Animator _brother;
    [SerializeField] private Animator _mother;
    [SerializeField] private Animator _father;
    [SerializeField] private Animator _soldier1;
    [SerializeField] private Animator _ambassador;
    [SerializeField] private Animator _door1;
    [SerializeField] private Animator _door2;
    [SerializeField] private Animator _drawer;

    [SerializeField] private GameObject _secondRoomWall;
    [SerializeField] private GameObject _invisWallBlockingCarpet;
    [SerializeField] private GameObject _invisWallBlockingExit;
    [SerializeField] private GameObject _invisWallBlockingMom;
    
    [SerializeField] private CameraController _playerCameraController;

    private const float WaitTimeGivePlayerControl = 8f;
    private const float WaitTimeAfterEnterSequenceStart = 7f;
    private const float WaitTimeAfterSecondRoomSequence = 10f;

    private float _enterSequenceStartTime;

    private LeadUpSoundController _soundController;
   

    private void Start()
    {
        // Sound Controller
        _soundController = this.GetComponent<LeadUpSoundController>();
        
        // Player cant move until mom is blocking the door
        DisablePlayerControl();
        
        // Play knock sequence
        KnockSequence();
        
        // Return control to player after wait time
        Invoke(nameof(EnablePlayerControl), WaitTimeGivePlayerControl);
        
    }

    private void Update()
    {
    }

    private void DisablePlayerControl()
    {
        PlayerController.Instance.BlockMovement();
    }

    private void EnablePlayerControl()
    {
        PlayerController.Instance.UnBlockMovement();
    }

    private void KnockSequence()
    {
        _brother.SetTrigger("knock");
        _mother.SetTrigger("knock");
        _father.SetTrigger("knock");
        _door2.SetTrigger("knock");
    }

    // Gets called by in world trigger box
    public void EnterSequence()
    {
        _soundController.PlayFatherAngry();
        _father.SetTrigger("enter");
        _soldier1.SetTrigger("enter");
        _ambassador.SetTrigger("enter");
        _door1.SetTrigger("open");
        
        Invoke(nameof(OpenSecondRoom), WaitTimeAfterEnterSequenceStart);
    }

    private void OpenSecondRoom()
    {
        _mother.SetTrigger("ambassador");
        _secondRoomWall.SetActive(false);
        _invisWallBlockingMom.SetActive(false);
    }

    // Gets called by in world trigger box
    public void SecondRoomSequence()
    {
        _soundController.PlayMotherAngry();
        StartCoroutine(_soundController.PlaySecondRoomAudio());
        
        _brother.SetTrigger("playerEnter");
        _door2.SetTrigger("playerEnter");
        _drawer.SetTrigger("playerEnter");
        _invisWallBlockingCarpet.SetActive(true);
        
        Invoke(nameof(OpenExit), WaitTimeAfterSecondRoomSequence);
    }

    public void SwitchCameraLimit()
    {
        _playerCameraController.xLimit1 = false;
        _playerCameraController.xLimit2 = true;
    }

    private void OpenExit()
    {
        _invisWallBlockingExit.SetActive(false);
    }

    public void PlayerLeave()
    {
        SceneManager.LoadScene("GameEnd");
    }
}
