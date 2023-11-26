using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterMathController : MonoBehaviour
{
   [SerializeField] private PlayerUIText _playerUIText;
   [SerializeField] private CameraController _playerCameraController;
   [SerializeField] private Transform _tvCameraPosition;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         TriggerTVSequence();
      }
   }

   private void Start()
   {
      _playerUIText.ShowWalkTutorial();
   }

   public void TriggerThrowTutorial()
   {
      _playerUIText.ShowThrowTutorial();
   }

   public void TriggerTVSequence()
   {
      _playerCameraController.ChangeCameraTarget(_tvCameraPosition);
      PlayerController.Instance.BlockMovement();
   }
}
