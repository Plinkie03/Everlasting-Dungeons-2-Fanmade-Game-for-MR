using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum ShapeType
    {
        SimpleBeam,
        Cross,
        CrossBeam,
        Beam,
        SmallExplosion,
        MediumExplosion,
        LargeExplosion,
        Vortice
    }

    public static class Shapes
    {
        public static readonly ShapeBuilder SimpleBeam = new ShapeBuilder().Repeat(shape => shape.Right(), 5);
        
        public static readonly ShapeBuilder Cross = new ShapeBuilder()
            .DrawAllDiagonal(1)
            .DrawAllDiagonal(2)
            .DrawAllDiagonal(3)
            .DrawAllDiagonal(4);

        public static readonly ShapeBuilder CrossBeam = new ShapeBuilder()
            .DrawAllNonDiagonal(1)
            .DrawAllNonDiagonal(2)
            .DrawAllNonDiagonal(3)
            .DrawAllNonDiagonal(4);

        public static readonly ShapeBuilder Beam = new ShapeBuilder()
            .Repeat(shape => shape.Right(), 4)
            .Left()
            .ResetPosition()
            .Up()
            .ResetPosition()
            .Down();

        public static readonly ShapeBuilder SmallExplosion = new ShapeBuilder()
            .DrawAll(1)
            .DrawAllNonDiagonal(2)
            .DrawAllDiagonal(2);

        public static readonly ShapeBuilder Vortice = new ShapeBuilder()
            .DrawAll(1);

        public static readonly ShapeBuilder MediumExplosion = new ShapeBuilder()
            .DrawAll(1)
            .DrawAll(2)
            .DrawAllNonDiagonal(2)
            .DrawAllNonDiagonal(3);

        public static readonly ShapeBuilder LargeExplosion = new ShapeBuilder()
            .DrawAll(1)
            .DrawAll(2)
            .DrawAll(3)
            .DrawAllNonDiagonal(3)
            .DrawAllNonDiagonal(4);
    }
}
