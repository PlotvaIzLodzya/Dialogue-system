﻿using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueLine
    {
        [field: SerializeField] public string Text { get; private set; }
        
        public DialogueLine(string text)
        {
            Text = text;
        }

        public DialogueLine()
        {
        }

        public void SetText(string text)
        {
            Debug.Log(text);
            Text = text;
        }
    }
}

