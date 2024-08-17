using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Generator
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Output", "Blocks"));
            using JsonDocument jsonBlocksCollisionShapes = JsonDocument.Parse(await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Input", "BlockCollisionShapes.json")));
            using JsonDocument jsonBlocks = JsonDocument.Parse(await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Input", "Blocks.json")));
            await Task.WhenAll(jsonBlocks.RootElement.EnumerateArray().Select(async (JsonElement jsonBlock) =>
            {
                int blockId = jsonBlock.GetProperty("minStateId").GetInt32();
                string blockNameId = jsonBlock.GetProperty("name").GetString()!;
                int blockLightFilter = jsonBlock.GetProperty("filterLight").GetInt32();
                int blockLightEmission = jsonBlock.GetProperty("emitLight").GetInt32();
                List<Property> properties = [];
                foreach (JsonElement jsonProperty in jsonBlock.GetProperty("states").EnumerateArray().Reverse())
                {
                    string propertyName = jsonProperty.GetProperty("name").GetString()!;
                    string propertyType = jsonProperty.GetProperty("type").GetString()!;
                    Property property;
                    switch (propertyType)
                    {
                        case "bool":
                            {
                                property = new(propertyName, null);
                                break;
                            }
                        case "int":
                            {
                                property = new(propertyName, jsonProperty.GetProperty("num_values").GetInt32());
                                break;
                            }
                        case "enum":
                            {
                                property = new(propertyName, jsonProperty.GetProperty("values").EnumerateArray().Select(value => value.GetString()!).ToList());
                                break;
                            }
                        default: throw new NotSupportedException();
                    }
                    properties.Add(property);
                };
                JsonElement jsonBlocksCollisionShapesBlock = jsonBlocksCollisionShapes.RootElement.GetProperty("blocks").GetProperty(blockNameId);
                JsonElement jsonBlocksCollisionShapesShapes = jsonBlocksCollisionShapes.RootElement.GetProperty("shapes");
                (double xa, double ya, double za, double xb, double yb, double zb)[][] blockCollisionShapes = [.. (jsonBlocksCollisionShapesBlock.ValueKind == JsonValueKind.Number ? [jsonBlocksCollisionShapesBlock] : jsonBlocksCollisionShapesBlock.EnumerateArray().ToList()).Select<JsonElement, (double xa, double ya, double za, double xb, double yb, double zb)[]>(blockCollisionShapesShape => [.. jsonBlocksCollisionShapesShapes.GetProperty(blockCollisionShapesShape.GetInt32().ToString()).EnumerateArray().Select(data => (data[0].GetDouble(), data[1].GetDouble(), data[2].GetDouble(), data[3].GetDouble(), data[4].GetDouble(), data[5].GetDouble()))])];
                await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Output", "Blocks", $"Block{Utility.ToCamelCase(blockNameId, true)}.cs"), new Block(blockNameId, blockId, properties, blockLightEmission, blockLightFilter, blockCollisionShapes).Build());
            }));
            Console.WriteLine("Done!");
        }
    }
}
