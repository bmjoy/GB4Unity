﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class DisassemblyView : MonoBehaviour
    {
        [SerializeField] GameObject linePrefab;
        [SerializeField] ScrollRect scroll;
        [SerializeField] InputField maxLines;

        Disassembler disassembler;

        public void Setup (Disassembler _)
        {
            disassembler = _;
        }

        public void Disassemble ()
        {
            int overflow = AdjustLines ();

            if (overflow == 0) {
                RemoveLine ();
            }

            string line = disassembler.Disassemble ();

            AddLine (line);
        }

        public void Refresh ()
        {
            AdjustLines ();
        }

        void AddLine (string value)
        {
            // spawn
            var instance = Instantiate (linePrefab);
            instance.transform.SetParent (scroll.content);
            instance.transform.SetAsLastSibling ();
            instance.SetActive (true);

            // fix transform
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            // set text
            var line = instance.GetComponent<Text> ();
            line.text = value;

            // scroll down after a single frame
            StartCoroutine (ScrollDown ());
        }

        IEnumerator ScrollDown ()
        {
            yield return 0;
            scroll.verticalNormalizedPosition = 0f;
        }

        void RemoveLine ()
        {
            var line = scroll.content.GetChild (0);
            GameObject.DestroyImmediate (line.gameObject);
        }

        int AdjustLines ()
        {
            int max = Convert.ToInt32 (maxLines.text);

            int overflow = scroll.content.childCount - max;

            if (overflow > 0) {
                for (int i = overflow; i > 0; i--) {
                    RemoveLine ();
                }
            }

            return overflow;
        }
    }
}