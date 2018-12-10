using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;

namespace Raytrace
{
    class Ch1Module : Module
    {
        public Ch1Module() : base("Raytrace.Ch1")
        {
            this.Code = @"
            [ Raytrace.linear-algebra ] USE-MODULES

            [ 'projectile' 'env' '_pos' ] VARIABLES

            # Initialize
            [ 0 1 0 POINT  1 1 0 VECTOR NORMALIZE ] [ 'position' 'velocity' ] REC  projectile !
            [ 0 -0.1 0 VECTOR  -0.01 0 0 VECTOR ] [ 'gravity' 'wind' ] REC env !

            : PROJECTILE   projectile @ ;
            : POSITION     PROJECTILE 'position' REC@ ;
            : VELOCITY     PROJECTILE 'velocity' REC@ ;
            : POSITION!    ( _pos ! ) PROJECTILE _pos @  'position' REC! ;
            : Y-POS   POSITION Y ;
            : X-POS   POSITION X ;

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
}
