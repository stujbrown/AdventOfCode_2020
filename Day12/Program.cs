using System;
using System.IO;
using System.Numerics;

namespace Day12
{
    class Program
    {
        static float DegreesToRadians(float degrees) { return (float)Math.PI / 180f * degrees; }

        static void Main(string[] args)
        {
            string[] instructions = File.ReadAllLines("input.txt");

            var part1FerryTransform = Matrix4x4.Identity;
            var part2FerryTransform = Matrix4x4.Identity;
            var waypointTransform = Matrix4x4.CreateTranslation(new Vector3(10, 1, 0));

            foreach (var instruction in instructions)
            {
                int magnitude = int.Parse(instruction.Substring(1));
                switch (instruction[0])
                {
                    case 'N': part1FerryTransform.Translation += new Vector3(0, magnitude, 0); break;
                    case 'E': part1FerryTransform.Translation += new Vector3(magnitude, 0, 0); break;
                    case 'S': part1FerryTransform.Translation += new Vector3(0, -magnitude, 0); break;
                    case 'W': part1FerryTransform.Translation += new Vector3(-magnitude, 0, 0); break;
                    case 'L': part1FerryTransform = Matrix4x4.Multiply(part1FerryTransform, Matrix4x4.CreateRotationZ(DegreesToRadians(magnitude), part1FerryTransform.Translation)); break;
                    case 'R': part1FerryTransform = Matrix4x4.Multiply(part1FerryTransform, Matrix4x4.CreateRotationZ(DegreesToRadians(-magnitude), part1FerryTransform.Translation)); break;
                    case 'F': part1FerryTransform = Matrix4x4.Multiply(Matrix4x4.CreateTranslation(new Vector3(magnitude, 0, 0)), part1FerryTransform); break;
                }

                switch (instruction[0])
                {
                    case 'N': waypointTransform.Translation += new Vector3(0, magnitude, 0); break;
                    case 'E': waypointTransform.Translation += new Vector3(magnitude, 0, 0); break;
                    case 'S': waypointTransform.Translation += new Vector3(0, -magnitude, 0); break;
                    case 'W': waypointTransform.Translation += new Vector3(-magnitude, 0, 0); break;
                    case 'L': waypointTransform = Matrix4x4.Multiply(waypointTransform, Matrix4x4.CreateRotationZ(DegreesToRadians(magnitude), part2FerryTransform.Translation)); break;
                    case 'R': waypointTransform = Matrix4x4.Multiply(waypointTransform, Matrix4x4.CreateRotationZ(DegreesToRadians(-magnitude), part2FerryTransform.Translation)); break;
                    case 'F': 
                        var translation = (waypointTransform.Translation - part2FerryTransform.Translation) * magnitude;
                        part2FerryTransform.Translation += translation;
                        waypointTransform.Translation += translation;
                        break;
                }

                part2FerryTransform.Translation = new Vector3( MathF.Round(part2FerryTransform.Translation.X), MathF.Round(part2FerryTransform.Translation.Y), 0);
                waypointTransform.Translation = new Vector3(MathF.Round(waypointTransform.Translation.X), MathF.Round(waypointTransform.Translation.Y), 0);

                Console.WriteLine("ferry {0},{1}", part2FerryTransform.Translation.X, part2FerryTransform.Translation.Y);

                Console.WriteLine("waypoint {0},{1}", waypointTransform.Translation.X, waypointTransform.Translation.Y);
                Console.WriteLine();
            }

            Console.WriteLine("Part 1 distance = {0}", Math.Round(Math.Abs(part1FerryTransform.Translation.X) + Math.Abs(part1FerryTransform.Translation.Y)));
            Console.WriteLine("Part 2 distance = {0}", Math.Round(Math.Abs(part2FerryTransform.Translation.X) + Math.Abs(part2FerryTransform.Translation.Y)));
        }
    }
}
