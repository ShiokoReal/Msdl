namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryDamage
    {
        public static readonly string[] Effects = ["hurt", "thorns", "drowning", "burning", "poking", "freezing"];
        public static readonly string[] Required = [
            "minecraft:generic_kill",
            "minecraft:dragon_breath",
            "minecraft:outside_border",
            "minecraft:freeze",
            "minecraft:stalagmite",
            "minecraft:in_fire",
            "minecraft:wither",
            "minecraft:generic",
            "minecraft:cactus",
            "minecraft:cramming",
            "minecraft:drown",
            "minecraft:fall",
            "minecraft:falling_anvil",
            "minecraft:fireworks",
            "minecraft:in_wall",
            "minecraft:indirect_magic",
            "minecraft:lava",
            "minecraft:lightning_bolt",
            "minecraft:magic",
            "minecraft:mob_attack",
            "minecraft:mob_attack_no_aggro",
            "minecraft:on_fire",
            "minecraft:out_of_world",
            "minecraft:player_explosion",
            "minecraft:starve",
            "minecraft:string",
            "minecraft:sweet_berry_bush",
            "minecraft:dry_out",
            "minecraft:hot_floor",
            "minecraft:fly_into_wall",
            "minecraft:thrown",
            "minecraft:spit",
            "minecraft:player_attack", //TODO: all vanilla types
            ];
        public string Name { get; }
        public string Effect { get; }
        public RegistryDamage(string name, string effect)
        {
            Name = name;
            Effect = effect;
        }
    }
}
