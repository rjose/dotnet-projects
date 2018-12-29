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
            [ '_world' '_ray' '_intersection' '_comps' ] VARIABLES

            : OBJECTS   _world @ 'objects' REC@ ;
            : INTERSECT-WORLD   ( _ray ! _world ! ) OBJECTS  '_ray @ INTERSECTS' MAP FLATTEN  't' SORT-BY-FIELD ;

            # Words supporting PREPARE-COMPUTATIONS
            : C-T         _intersection @ 't' REC@ ;
            : C-OBJ       _intersection @ 'obj' REC@ ;
            : C-POINT     _ray @  _intersection @ 't' REC@  POSITION ;
            : C-EYEV      _ray @ 'direction' REC@  NEGATE ;
            : C-NORMALV   C-OBJ C-POINT NORMAL-AT ;
            : C-SIGN      C-NORMALV C-EYEV DOT SIGN ;
            : C-INSIDE    C-SIGN 0 < ;
            : C-NORMALV   C-NORMALV C-SIGN * ;
            : PREPARE-COMPUTATIONS   ( _ray ! _intersection ! )
                [ C-T C-OBJ C-POINT C-EYEV C-NORMALV C-INSIDE ]
                [ 't' 'obj' 'point' 'eyev' 'normalv' 'inside' ] REC
            ;

            # Words supporting SHADE-HIT
            : C-VAL        _comps @ SWAP REC@ ;  # ( field -- value )
            : S-MATERIAL   'obj' C-VAL  'material' REC@ ;
            : S-LIGHT      _world @ 'light' REC@ ;
            : S-POINT      'point' C-VAL ;
            : S-EYEV       'eyev' C-VAL ;
            : S-NORMALV    'normalv' C-VAL ;
            : SHADE-HIT   ( _comps ! _world ! ) S-MATERIAL S-LIGHT S-POINT S-EYEV S-NORMALV   LIGHTING ;

            # Words supporting COLOR-AT
            : COLOR-MISS   POP 0 0 0 Color ;  # ( hit -- color )
            : COLOR-HIT   _ray @ PREPARE-COMPUTATIONS _world @ SWAP SHADE-HIT ;  # ( hit -- color )
            : COLOR-AT    INTERSECT-WORLD HIT COLOR-HIT/MISS ;  # ( world ray -- color )

            # Words supporting VIEW-TRANSFORM
            [ '_from' '_up' '_to' '_forward' '_-forward' '_upn' '_left' '_true_up' ] VARIABLES
            : X@   @ 'x' REC@ ; 
            : Y@   @ 'y' REC@ ; 
            : Z@   @ 'z' REC@ ; 
            : FORWARD!         _to @ _from @ - NORMALIZE   _forward ! ;
            : -FORWARD!        _forward @ NEGATE   _-forward ! ;
            : UPN!             _up @ NORMALIZE _upn ! ;
            : LEFT!            _forward @ _upn @  CROSS   _left ! ;
            : TRUE-UP!         _left @ _forward @ CROSS   _true_up ! ;
            : V-INIT           FORWARD! -FORWARD! UPN! LEFT! TRUE-UP! ;
            : V-ORIENTATION    [   _left X@      _left Y@      _left Z@  0
                                _true_up X@   _true_up Y@   _true_up Z@  0
                               _-forward X@  _-forward Y@  _-forward Z@  0
                                          0             0             0  1 ]  Matrix ;
            : V-TRANSLATION    [ _from X@  _from Y@ _from Z@ ] 'NEGATE' MAP UNPACK TRANSLATION ;
            : VIEW-TRANSFORM   ( _up ! _to ! _from ! ) V-INIT V-ORIENTATION V-TRANSLATION * ;
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
