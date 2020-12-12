using System;
using System.IO;
using System.Numerics;

namespace Day12
{
    class Program
    {
        static float DegreesToRadians(float degrees) { return (float)Math.PI / 180f * degrees; }

        static void RunInstruction(ref Matrix4x4 ferryTransform, ref Matrix4x4 waypointTransform, char instruction, int magnitude, bool usingWaypoint)
        {
            ref var transformToMove = ref ferryTransform;
            if (usingWaypoint) transformToMove = ref waypointTransform;

            switch (instruction)
            {
                case 'N': transformToMove.Translation += new Vector3(0, magnitude, 0); break;
                case 'E': transformToMove.Translation += new Vector3(magnitude, 0, 0); break;
                case 'S': transformToMove.Translation += new Vector3(0, -magnitude, 0); break;
                case 'W': transformToMove.Translation += new Vector3(-magnitude, 0, 0); break;
                case 'L': transformToMove = Matrix4x4.Multiply(transformToMove, Matrix4x4.CreateRotationZ(DegreesToRadians(magnitude), ferryTransform.Translation)); break;
                case 'R': transformToMove = Matrix4x4.Multiply(transformToMove, Matrix4x4.CreateRotationZ(DegreesToRadians(-magnitude), ferryTransform.Translation)); break;
                case 'F':
                    if (usingWaypoint == false)
                    {
                        ferryTransform = Matrix4x4.Multiply(Matrix4x4.CreateTranslation(new Vector3(magnitude, 0, 0)), ferryTransform);
                    }
                    else
                    {
                        var translation = (waypointTransform.Translation - ferryTransform.Translation) * magnitude;
                        ferryTransform.Translation += translation;
                        waypointTransform.Translation += translation;
                    }
                    break;
            }

            ferryTransform.Translation = new Vector3(MathF.Round(ferryTransform.Translation.X), MathF.Round(ferryTransform.Translation.Y), 0);
            waypointTransform.Translation = new Vector3(MathF.Round(waypointTransform.Translation.X), MathF.Round(waypointTransform.Translation.Y), 0);
        }

        static void Main(string[] args)
        {
            string[] instructions = File.ReadAllLines("input.txt");

            var ferryTransforms = new Matrix4x4[] { Matrix4x4.Identity, Matrix4x4.Identity };
            var waypointTransforms = new Matrix4x4[] { Matrix4x4.Identity, Matrix4x4.CreateTranslation(new Vector3(10, 1, 0)) };

            foreach (var instruction in instructions)
            {
                int magnitude = int.Parse(instruction.Substring(1));
                RunInstruction(ref ferryTransforms[0], ref waypointTransforms[0], instruction[0], magnitude, false);
                RunInstruction(ref ferryTransforms[1], ref waypointTransforms[1], instruction[0], magnitude, true);
            }

            Console.WriteLine("Part 1 distance = {0}", Math.Round(Math.Abs(ferryTransforms[0].Translation.X) + Math.Abs(ferryTransforms[0].Translation.Y)));
            Console.WriteLine("Part 2 distance = {0}", Math.Round(Math.Abs(ferryTransforms[1].Translation.X) + Math.Abs(ferryTransforms[1].Translation.Y)));
        }
    }
}
