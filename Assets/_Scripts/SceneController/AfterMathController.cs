using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterMathController : MonoBehaviour
{
   [SerializeField] private PlayerUIText _playerUIText;
   [SerializeField] private CameraController _playerCameraController;
   [SerializeField] private Transform _tvCameraPosition;
   [SerializeField] private int _neededToysForTV;
   [SerializeField] private WorldText _cleanupText;

   private bool _throwTutorialTriggered;

   private void OnEnable()
   {
      PickupInteractable.PickupCollected += TriggerThrowTutorial;
      ToyBin.ToyCollected += CheckForTVTrigger;
   }

   private void OnDisable()
   {
      PickupInteractable.PickupCollected -= TriggerThrowTutorial;
      ToyBin.ToyCollected -= CheckForTVTrigger;
   }

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
   
   private void TriggerThrowTutorial(PickupInteractable obj)
   {
      Debug.Log("trigger tutorial");
      if (!_throwTutorialTriggered)
      {
         _throwTutorialTriggered = true;
         _playerUIText.ShowThrowTutorial();
      }
   }

   public void EnterTVRoom()
   {
      _playerUIText.ClearInteractText();
      _cleanupText.ShowTextByLetter();
   }

   private void CheckForTVTrigger(int toyAmount)
   {
      if (toyAmount >= _neededToysForTV)
      {
         TriggerTVSequence();
      }
   }
   
   private void TriggerTVSequence()
   {
      _playerCameraController.ChangeCameraTarget(_tvCameraPosition);
      PlayerController.Instance.BlockMovement();
      Invoke(nameof(EndAfterMathScene), 25f);
   }

   private void EndAfterMathScene()
   {
      SceneManager.LoadScene("TheEvent 1");
   }
}
