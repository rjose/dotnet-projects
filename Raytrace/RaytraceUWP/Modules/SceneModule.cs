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

            this.Code = @"
            [ intersection shader ] USE-MODULES
            [ '_world' '_ray' '_intersection' '_comps' ] VARIABLES

            : OBJECTS   _world @ 'objects' REC@ ;
            : INTERSECT-WORLD   ( _ray ! _world ! ) OBJECTS  '_ray @ INTERSECTS' MAP FLATTEN  't' SORT-BY-FIELD ;

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

            : C-VAL        _comps @ SWAP REC@ ;  # ( field -- value )
            : S-MATERIAL   'obj' C-VAL  'material' REC@ ;
            : S-LIGHT      _world @ 'light' REC@ ;
            : S-POINT      'point' C-VAL ;
            : S-EYEV       'eyev' C-VAL ;
            : S-NORMALV    'normalv' C-VAL ;
            : SHADE-HIT   ( _comps ! _world ! ) S-MATERIAL S-LIGHT S-POINT S-EYEV S-NORMALV   LIGHTING ;
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
}
