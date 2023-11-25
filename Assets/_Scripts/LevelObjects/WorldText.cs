using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorldText : MonoBehaviour
{
   [SerializeField] private TMP_Text _textField;
   [SerializeField] private string _textToDisplay;
   [SerializeField] private float _timeToDisplay;
   [SerializeField] private float _spaceWaitTime;

   private void Start()
   {
      ShowTextByLetter();
   }

   public void ShowTextByLetter()
   {
      StartCoroutine(DisplayTextLetterForLetter(_timeToDisplay, _textToDisplay));
   }

   public void SetTimeAndText()
   {
      
   }

   private void SetTextToDisplay(string newText)
   {
      _textToDisplay = newText;
   }

   private void SetTimeToDisplay(float newTime)
   {
      _timeToDisplay = newTime;
   }
   
   private void AddLetterToText(char character)
   {
      _textField.text = _textField.text + character;
   }
   
   private IEnumerator DisplayTextLetterForLetter(float time, string text)
   {
      float timestep = time / text.Length;
      foreach (var character in text)
      {
         AddLetterToText(character);
         // Double wait time for spaces for better flow
         if (character == ' ')
         {
            yield return new WaitForSeconds(_spaceWaitTime);
         }
         else
         {
            yield return new WaitForSeconds(timestep);
         }
      }
   }
   
}