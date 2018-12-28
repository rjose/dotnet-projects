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
            [ intersection ] USE-MODULES
            [ '_world' '_ray' ] VARIABLES

            : OBJECTS   _world @ 'objects' REC@ ;
            : INTERSECT-WORLD   ( _ray ! _world ! ) OBJECTS  '_ray @ INTERSECTS' MAP FLATTEN  't' SORT-BY-FIELD ;
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
