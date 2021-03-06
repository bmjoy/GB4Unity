﻿using System;
using System.Collections;

namespace StudioKurage.Emulator.Gameboy
{
    // central processing unit (sharp LR35902)
    public partial class Cpu
    {
        // memory management unit
        public Mmu mmu;

        public Cpu (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            af = 0x01B0;
            bc = 0x0013;
            de = 0x00D8;
            hl = 0x014D;
            pc = (ushort)(mmu.biosActive ? 0x0000 : 0x0100);
            sp = 0xFFFE;
            mc = 0;
            cc = 0;
            imc = 0;
            icc = 0;
        }

        public ushort ReadOpcode ()
        {
            return mmu.rw (pc);
        }

        public void ExecNextOpcode ()
        {
            byte opcode = mmu.rb (pc++);
            ExecOpcode (opcode);
        }

        public void ExecOpcode (byte opcode)
        {
            var instr = map [opcode];
            instr (this);
            mc += imc;
            cc += icc;
            if (mmu.biosActive && pc == 0x0100) {
                mmu.biosActive = false;
            }
        }

        public ushort opcode {
            get {
                return mmu.rw (pc);
            }
        }

        delegate void Instruction (Cpu Cpu);
    }
}
