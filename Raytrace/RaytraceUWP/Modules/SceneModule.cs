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
            AddWord(new CameraWord("Camera"));

            this.Code = @"
            [ intersection shader canvas ] USE-MODULES
            [ 'world' 'ray' ] VARIABLES

            # Words supporting PREPARE-COMPUTATIONS
            [ 'intersection' ] VARIABLES
         {
            [ 't' 'obj' 'point' 'eyev' 'normalv' 'sign' 'inside' 'over_point' ] VARIABLES
            : t!              intersection @ 't' REC@    t ! ;
            : obj!            intersection @ 'obj' REC@ obj ! ;
            : point!          ray @  t @  POSITION point ! ;
            : eyev!           ray @ 'direction' REC@  NEGATE eyev ! ;
            : normalv!        obj @ point @ NORMAL-AT normalv ! ;
            : sign!           normalv @ eyev @ DOT SIGN sign ! ;
            : inside!         sign @  0 < inside ! ;
            : ADJUST-NORMALV  ; # normalv @ sign @ *  normalv ! ;
            : over_point!     point @  normalv @ EPSILON * +  over_point ! ;
            : INIT            t! obj! point! eyev! normalv! sign! inside! ADJUST-NORMALV over_point! ;
            : PREPARE-COMPUTATIONS   ( ray ! intersection ! ) INIT
                [  t @  obj @  point @  over_point @  eyev @  normalv @  inside @ ]
                [ 't'  'obj'  'point'  'over_point'  'eyev'  'normalv'  'inside'  ] REC
            ;

            [ 'PREPARE-COMPUTATIONS' ] PUBLISH
        }

            # Words supporting SHADE-HIT
            [ 'comps' ] VARIABLES
        {
            : VAL        comps @ SWAP REC@ ;  # ( field -- value )
            : MATERIAL   'obj' VAL  'material' REC@ ;
            : LIGHT      world @ 'light' REC@ ;
            : POINT      'point' VAL ;
            : OVER-POINT  'over_point' VAL ;
            : EYEV       'eyev' VAL ;
            : NORMALV    'normalv' VAL ;
            : IN-SHADOW   world @  OVER-POINT IS-SHADOWED ;
            : SHADE-HIT   ( comps ! world ! ) MATERIAL LIGHT POINT EYEV NORMALV IN-SHADOW   LIGHTING ;

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
            [ 'from' 'up' 'to' 'forward' '-forward' 'upn' 'left' 'true_up' ] VARIABLES
         {
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

            # Words supporting RAY-FOR-PIXEL
            [ 'camera' 'x' 'y' 'origin' 'inv_transform' ] VARIABLES
         {
            : INV-TRANSFORM!   camera @ 'transform' REC@ INVERSE   inv_transform ! ;
            : ORIGIN!          inv_transform @  0 0 0 Point *      origin ! ;
            : INIT             INV-TRANSFORM! ORIGIN! ;

            : CAM-VAL          camera @ SWAP REC@ ;  # ( field -- value )
            : OFFSET           @ 0.5 +  'pixel_size' CAM-VAL * ;  # ( x/y -- offset )
            : WORLD-X          ( 'half_width'  CAM-VAL ) x OFFSET - ;
            : WORLD-Y          ( 'half_height' CAM-VAL ) y OFFSET - ;
            : PIXEL            inv_transform @  WORLD-X WORLD-Y -1 Point * ;
            : DIRECTION        PIXEL origin @ - NORMALIZE ;

            : RAY-FOR-PIXEL   ( y ! x ! camera ! )  INIT  origin @ DIRECTION Ray ;

            [ 'RAY-FOR-PIXEL' ] PUBLISH
        }

            # Words supporting RENDER
            [ 'camera' 'world' 'image' 'x' 'y' ] VARIABLES
         {
            : HSIZE          camera @ 'hsize' REC@ ;
            : VSIZE          camera @ 'vsize' REC@ ;
            : INIT           HSIZE VSIZE Canvas   image ! ; 
            : IMAGE          image @ ;
            : RAY            camera @  x @  y @  RAY-FOR-PIXEL  ;
            : COLOR          world @ RAY  COLOR-AT ;
            : RENDER-PIXEL   ( y ! x ! ) IMAGE x @ y @ COLOR  WRITE-PIXEL ;
            : RENDER         ( world ! camera ! ) INIT ( [ HSIZE VSIZE ] 'RENDER-PIXEL' FOR ) IMAGE ;

            [ 'RENDER-PIXEL' 'RENDER' ] PUBLISH
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

    class CameraWord : Word
    {
        public CameraWord(string name) : base(name) { }

        // ( hsize vsize field_of_view -- Camera )
        public override void Execute(Interpreter interp)
        {
            dynamic field_of_view = interp.StackPop();
            dynamic vsize = interp.StackPop();
            dynamic hsize = interp.StackPop();
            interp.StackPush(new CameraItem(hsize, vsize, field_of_view));
        }
    }

}
