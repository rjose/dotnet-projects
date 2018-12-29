using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;
using System.Diagnostics;

namespace RaytraceUWP
{
    public class SceneModule : Module
    {
        public SceneModule(string name) : base(name)
        {
            AddWord(new WorldWord("World"));
            AddWord(new AddObjectWord("ADD-OBJECT"));
            AddWord(new ContainsWord("CONTAINS"));
            AddWord(new ColorHitMissWord("COLOR-HIT/MISS"));

            this.Code = @"
            [ intersection shader canvas ] USE-MODULES
            [ 'world' 'ray' ] VARIABLES

            # Words supporting PREPARE-COMPUTATIONS
         {
            [ 'intersection' ] VARIABLES
            : T         intersection @ 't' REC@ ;
            : OBJ       intersection @ 'obj' REC@ ;
            : POINT     ray @  intersection @ 't' REC@  POSITION ;
            : EYEV      ray @ 'direction' REC@  NEGATE ;
            : NORMALV   OBJ POINT NORMAL-AT ;
            : SIGN      NORMALV EYEV DOT SIGN ;
            : INSIDE    SIGN 0 < ;
            : NORMALV   NORMALV SIGN * ;
            : PREPARE-COMPUTATIONS   ( ray ! intersection ! )
                [  T   OBJ   POINT   EYEV   NORMALV   INSIDE ]
                [ 't' 'obj' 'point' 'eyev' 'normalv' 'inside' ] REC
            ;

            [ 'PREPARE-COMPUTATIONS' ] PUBLISH
        }

            # Words supporting SHADE-HIT
        {
            [ 'comps' ] VARIABLES
            : VAL        comps @ SWAP REC@ ;  # ( field -- value )
            : MATERIAL   'obj' VAL  'material' REC@ ;
            : LIGHT      world @ 'light' REC@ ;
            : POINT      'point' VAL ;
            : EYEV       'eyev' VAL ;
            : NORMALV    'normalv' VAL ;
            : SHADE-HIT   ( comps ! world ! ) MATERIAL LIGHT POINT EYEV NORMALV   LIGHTING ;

            [ 'SHADE-HIT' ] PUBLISH
        }

            # Words supporting COLOR-AT and INTERSECT-WORLD
        {
            : OBJECTS   world @ 'objects' REC@ ;
            : INTERSECT-WORLD   ( ray ! world ! ) OBJECTS  'ray @ INTERSECTS' MAP FLATTEN  't' SORT-BY-FIELD ;

            : COLOR-MISS   POP 0 0 0 Color ;  # ( hit -- color )
            : COLOR-HIT   ray @ PREPARE-COMPUTATIONS world @ SWAP SHADE-HIT ;  # ( hit -- color )
            : COLOR-AT    INTERSECT-WORLD HIT COLOR-HIT/MISS ;  # ( world ray -- color )

            [ 'INTERSECT-WORLD' 'COLOR-HIT' 'COLOR-MISS' 'COLOR-AT' ] PUBLISH
        }

            # Words supporting VIEW-TRANSFORM
         {
            [ 'from' 'up' 'to' 'forward' '-forward' 'upn' 'left' 'true_up' ] VARIABLES
            : X@   @ 'x' REC@ ; 
            : Y@   @ 'y' REC@ ; 
            : Z@   @ 'z' REC@ ; 
            : FORWARD!       to @ from @ - NORMALIZE   forward ! ;
            : -FORWARD!      forward @ NEGATE   -forward ! ;
            : UPN!           up @ NORMALIZE upn ! ;
            : LEFT!          forward @ upn @  CROSS   left ! ;
            : TRUE-UP!       left @ forward @ CROSS   true_up ! ;
            : INIT           FORWARD! -FORWARD! UPN! LEFT! TRUE-UP! ;
            : ORIENTATION    [     left X@      left Y@      left Z@  0
                                true_up X@   true_up Y@   true_up Z@  0
                               -forward X@  -forward Y@  -forward Z@  0
                                         0            0            0  1 ]  Matrix ;
            : V-TRANSLATION    [ from X@  from Y@ from Z@ ] 'NEGATE' MAP UNPACK TRANSLATION ;
            : VIEW-TRANSFORM   ( up ! to ! from ! ) INIT ORIENTATION V-TRANSLATION * ;

            [ 'VIEW-TRANSFORM' ] PUBLISH
        }
            ";

        }
    }

    class WorldWord : Word
    {
        public WorldWord(string name) : base(name) { }

        // ( -- World )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new WorldItem());
        }
    }

    class AddObjectWord : Word
    {
        public AddObjectWord(string name) : base(name) { }

        // ( world object -- )
        public override void Execute(Interpreter interp)
        {
            dynamic obj = interp.StackPop();
            dynamic world = interp.StackPop();
            world.AddObject(obj);
        }
    }

    class ContainsWord : Word
    {
        public ContainsWord(string name) : base(name) { }

        // ( world object -- bool )
        public override void Execute(Interpreter interp)
        {
            dynamic obj = interp.StackPop();
            dynamic world = interp.StackPop();
            interp.StackPush(new BoolItem(world.Contains(obj)));
        }
    }

    class ColorHitMissWord : Word
    {
        public ColorHitMissWord(string name) : base(name) { }

        // ( hit -- color )
        public override void Execute(Interpreter interp)
        {
            dynamic hit = interp.StackPop();
            if (hit.GetType() == typeof(NullItem))
            {
                interp.StackPush(hit);
                interp.Run("COLOR-MISS");
            }
            else
            {
                interp.StackPush(hit);
                interp.Run("COLOR-HIT");
            }
        }
    }
}
