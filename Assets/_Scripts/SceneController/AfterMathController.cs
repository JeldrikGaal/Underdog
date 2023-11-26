using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterMathController : MonoBehaviour
{
   [SerializeField] private PlayerUIText _playerUIText;

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
      
   }
}
