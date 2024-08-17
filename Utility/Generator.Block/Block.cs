using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Net.Myzuc.PurpleStainedGlass.Generator
{
    public sealed class Block
    {
        public readonly string Name;
        public readonly int Id;
        public readonly int LightEmission;
        public readonly int LightFilter;
        public readonly (double xa, double ya, double za, double xb, double yb, double zb)[][] Collisions;
        public readonly IReadOnlyList<Property> Properties;
        public readonly int Range;
        public Block(string name, int id, IReadOnlyList<Property> properties, int lightEmission, int lightFilter, (double xa, double ya, double za, double xb, double yb, double zb)[][] collisions)
        {
            Name = name;
            Id = id;
            Properties = properties;
            LightEmission = lightEmission;
            LightFilter = lightFilter;
            Collisions = collisions;
        }
        public string Build()
        {
            StringBuilder build = new();
            Property[] properties = [.. Properties];
            if (properties.Any(property => property.Range.HasValue)) build.Append("using System.ComponentModel.DataAnnotations;\n");
            build.Append("namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks\n");
            build.Append("{\n");
            build.Append($"    public sealed class Block{Utility.ToCamelCase(Name, true)} : Block\n");
            build.Append("    {\n");
            for (int i = 0; i < properties.Length; i++)
            {
                Property property = properties[i];
                if (property.Values is null) continue;
                build.Append($"        public enum Enum{Utility.ToCamelCase(property.Name, true)} : int\n");
                build.Append("        {\n");
                string[] values = [.. property.Values];
                for (int e = 0; e < values.Length; e++)
                {
                    build.Append($"            {Utility.ToCamelCase(values[e], true)} = {e}");
                    if (e < values.Length - 1) build.Append(',');
                    build.Append('\n');
                }
                build.Append("        }\n");
            }
            if (Collisions.Length > 1)
            {
                build.Append("        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [\n");
                for (int i = 0; i < Collisions.Length; i++)
                {
                    (double xa, double ya, double za, double xb, double yb, double zb)[] collisions = Collisions[i];
                    if (collisions.Length > 0)
                    {
                        build.Append($"            [\n");
                        for (int e = 0; e < collisions.Length; e++)
                        {
                            (double xa, double ya, double za, double xb, double yb, double zb) collision = collisions[e];
                            build.Append($"                ({collision.xa.ToString(CultureInfo.InvariantCulture)}, {collision.ya.ToString(CultureInfo.InvariantCulture)}, {collision.za.ToString(CultureInfo.InvariantCulture)}, {collision.xb.ToString(CultureInfo.InvariantCulture)}, {collision.yb.ToString(CultureInfo.InvariantCulture)}, {collision.zb.ToString(CultureInfo.InvariantCulture)})");
                            if (e < collisions.Length - 1) build.Append(",");
                            build.Append("\n");
                        }
                        build.Append($"            ]");
                    }
                    else
                    {
                        build.Append($"            []");
                    }
                    if (i < Collisions.Length - 1) build.Append(",");
                    build.Append("\n");
                }
                build.Append("        ];\n");
            }
            build.Append($"        public override int BlockId => {Id}");
            int states = 1;
            for (int i = 0; i < properties.Length; i++)
            {
                Property property = properties[i];
                build.Append(" + ");
                if (property.Values is not null)
                {
                    build.Append($"(int){Utility.ToCamelCase(property.Name, true)} * {states}");
                    states *= property.Values.Count;
                }
                else if (property.Range.HasValue)
                {
                    build.Append($"{Utility.ToCamelCase(property.Name, true)} * {states}");
                    states *= property.Range.Value;
                }
                else
                {
                    build.Append($"({Utility.ToCamelCase(property.Name, true)} ? 0 : {states})");
                    states *= 2;
                }
            }
            build.Append(";\n");
            build.Append($"        public override int LiquidId => ");
            if (Name == "water") build.Append("1");
            else if (Name == "lava") build.Append("2");
            else if (properties.Any(property => property.Name == "waterlogged")) build.Append("Waterlogged ? 1 : 0");
            else build.Append("0");
            build.Append(";\n");
            build.Append($"        public override int LightEmission => {LightEmission};\n");
            build.Append($"        public override int LightFilter => {LightFilter};\n");
            build.Append("        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => ");
            if (Collisions.Length > 1) build.Append($"[.. InternalCollisions[BlockId - {Id}]]");
            else
            {
                (double xa, double ya, double za, double xb, double yb, double zb)[] collisions = Collisions[0];
                if (collisions.Length > 0)
                {
                    build.Append($"[\n");
                    for (int e = 0; e < collisions.Length; e++)
                    {
                        (double xa, double ya, double za, double xb, double yb, double zb) collision = collisions[e];
                        build.Append($"            ({collision.xa.ToString(CultureInfo.InvariantCulture)}, {collision.ya.ToString(CultureInfo.InvariantCulture)}, {collision.za.ToString(CultureInfo.InvariantCulture)}, {collision.xb.ToString(CultureInfo.InvariantCulture)}, {collision.yb.ToString(CultureInfo.InvariantCulture)}, {collision.zb.ToString(CultureInfo.InvariantCulture)})");
                        if (e < collisions.Length - 1) build.Append(",");
                        build.Append("\n");
                    }
                    build.Append($"        ]");
                }
                else
                {
                    build.Append($"[]");
                }
            }
            build.Append(";\n");
            for (int i = 0; i < properties.Length; i++)
            {
                Property property = properties[i];
                if (property.Range.HasValue) build.Append($"        [Range(0, {property.Range - 1})]\n");
                build.Append("        public ");
                if (property.Values is not null) build.Append($"Enum{Utility.ToCamelCase(property.Name, true)}");
                else if (property.Range.HasValue) build.Append("int");
                else build.Append("bool");
                build.Append($" {Utility.ToCamelCase(property.Name, true)} = ");
                if (property.Values is not null) build.Append($"Enum{Utility.ToCamelCase(property.Name, true)}.{Utility.ToCamelCase(property.Values[0], true)}");
                else if (property.Range.HasValue) build.Append("0");
                else build.Append("false");
                build.Append(";\n");
            }
            build.Append($"        public Block{Utility.ToCamelCase(Name, true)}()\n");
            build.Append("        {\n");
            build.Append("            \n");
            build.Append("        }\n");
            build.Append($"        public override Block{Utility.ToCamelCase(Name, true)} Clone()\n");
            build.Append("        {\n");
            build.Append("            return new()");
            if (properties.Length > 0)
            {
                build.Append("\n");
                build.Append("            {\n");
                for (int i = 0; i < properties.Length; i++)
                {
                    Property property = properties[i];
                    build.Append($"                {Utility.ToCamelCase(property.Name, true)} = {Utility.ToCamelCase(property.Name, true)}");
                    if (i < properties.Length - 1) build.Append(",");
                    build.Append("\n");
                }
                build.Append("            }");
            }
            build.Append(";\n");
            build.Append("        }\n");
            build.Append($"        public override Block Break()\n");
            build.Append("        {\n");
            build.Append("            return ");
            if (properties.Any(property => property.Name == "waterlogged")) build.Append("Waterlogged ? new BlockWater() : new BlockAir()");
            else build.Append("new BlockAir()");
            build.Append(";\n");
            build.Append("        }\n");
            build.Append("    }\n");
            build.Append("}\n");
            return build.ToString().ReplaceLineEndings();
        }
    }
}
