using System;

namespace SudoEngine.Maths
{
    public static class FMath
    {
        public const float ToRadians = (float)Math.PI / 180.0f;

        public static double CopySign(double receiver, double sender)
        {
            if (double.IsNaN(receiver)) return double.NaN;
            if (receiver < 0.0d)
            {
                if (sender < 0.0d) return receiver;
                return receiver * -1.0d;
            }
            if (sender < 0.0d) return receiver * 1.0d;
            else return receiver;
        }

        public static double DeltaAngle(double current, double target)
        {
            double delta = Repeat(target - current, 360.0f);
            if (delta > 180.0f) delta -= 360.0f;
            return delta;
        }

        public static double Repeat(double t, double length) => Clamp(t - Math.Floor(t / length) * length, 0.0D, length);

        public static double Clamp(double value, double min, double max) => value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : value);
    }
}