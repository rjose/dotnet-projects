using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;

namespace RaytraceUWP
{
    class Ch1Module : Module
    {
        public Ch1Module() : base("Ch1")
        {
            AddWord(new ToClampedIntWord(">CLAMPED-INT"));
            AddWord(new FlipYWord("FLIP-Y"));

            this.Code = @"
            [ linear-algebra ] USE-MODULES

            [ 'projectile' 'env' ] VARIABLES

            # Initialize
            [ 0 1 0 Point  1 1 0 Vector NORMALIZE ] [ 'position' 'velocity' ] REC  projectile !
            [ 0 -0.1 0 Vector  -0.01 0 0 Vector ] [ 'gravity' 'wind' ] REC env !

            : PROJECTILE   projectile @ ;
            : POSITION     PROJECTILE 'position' REC@ ;
            : VELOCITY     PROJECTILE 'velocity' REC@ ;
            : POSITION!    PROJECTILE SWAP 'position' REC! ;
            : VELOCITY!    PROJECTILE SWAP 'velocity' REC! ;
            : Y-POS   POSITION 'y' REC@ ;
            : X-POS   POSITION 'x' REC@ ;

            : ENV          env @ ;
            : GRAVITY      ENV 'gravity' REC@ ;
            : WIND         ENV 'wind' REC@ ;

            : NEXT-POSITION   POSITION VELOCITY + ;
            : NEXT-VELOCITY   VELOCITY GRAVITY +  WIND + ;

            : UPDATE-POSITION   NEXT-POSITION POSITION! ;
            : UPDATE-VELOCITY   PROJECTILE NEXT-VELOCITY 'velocity' REC! ;
            : TICK              UPDATE-POSITION UPDATE-VELOCITY ;
            ";
        }
    }

    class ToClampedIntWord : Word
    {
        public ToClampedIntWord(string name) : base(name) { }

        // ( double max-int -- int )
        public override void Execute(Interpreter interp)
        {
            IntItem maxVal = (IntItem)interp.StackPop();
            DoubleItem value = (DoubleItem)interp.StackPop();
            int intValue = value.IntValue;
            if (intValue < 0) intValue = 0;
            if (intValue > maxVal.IntValue-1) intValue = maxVal.IntValue-1;
            interp.StackPush(new IntItem(intValue));
        }
    }

    class FlipYWord : Word
    {
        public FlipYWord(string name) : base(name) { }

        // ( y max-int -- flipped-y )
        public override void Execute(Interpreter interp)
        {
            IntItem maxVal = (IntItem)interp.StackPop();
            IntItem value = (IntItem)interp.StackPop();
            int result = maxVal.IntValue - 1 - value.IntValue;
            interp.StackPush(new IntItem(result));
        }
    }

}
