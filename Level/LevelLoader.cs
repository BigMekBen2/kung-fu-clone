namespace KungFuClone.Level;

public static class LevelLoader
{
    public static FloorDefinition Build(int floor) => floor switch
    {
        1 => BuildFloor1(),
        2 => BuildFloor2(),
        3 => BuildFloor3(),
        4 => BuildFloor4(),
        5 => BuildFloor5(),
        _ => BuildFloor1()
    };

    public static FloorDefinition BuildFloor1() => new()
    {
        FloorNumber = 1, TotalWidth = 3200, TimeLimit = 120f,
        Spawns = new SpawnEntry[]
        {
            new() { Type = EnemyType.Gripper,      WorldX = 380  },
            new() { Type = EnemyType.Gripper,      WorldX = 560  },
            new() { Type = EnemyType.Gripper,      WorldX = 720  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 900  },
            new() { Type = EnemyType.Gripper,      WorldX = 1050 },
            new() { Type = EnemyType.Gripper,      WorldX = 1200 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1380 },
            new() { Type = EnemyType.Gripper,      WorldX = 1500 },
            new() { Type = EnemyType.Gripper,      WorldX = 1650, Side = -1 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1800 },
            new() { Type = EnemyType.Gripper,      WorldX = 1950 },
            new() { Type = EnemyType.Gripper,      WorldX = 2100 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2280 },
            new() { Type = EnemyType.Gripper,      WorldX = 2420 },
            new() { Type = EnemyType.Gripper,      WorldX = 2580 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2760 },
            new() { Type = EnemyType.Gripper,      WorldX = 2900, Side = -1 },
        },
        Boss = new BossSpawn { WorldX = 3050, AttackPattern = 1 }
    };

    public static FloorDefinition BuildFloor2() => new()
    {
        FloorNumber = 2, TotalWidth = 3200, TimeLimit = 110f,
        Spawns = new SpawnEntry[]
        {
            new() { Type = EnemyType.Gripper,      WorldX = 360  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 520  },
            new() { Type = EnemyType.StickFighter,  WorldX = 680  },
            new() { Type = EnemyType.Gripper,      WorldX = 840  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1000 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1160 },
            new() { Type = EnemyType.Gripper,      WorldX = 1300 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1460 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1620 },
            new() { Type = EnemyType.Gripper,      WorldX = 1780, Side = -1 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1940 },
            new() { Type = EnemyType.StickFighter,  WorldX = 2100 },
            new() { Type = EnemyType.Gripper,      WorldX = 2260 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2420 },
            new() { Type = EnemyType.StickFighter,  WorldX = 2580 },
            new() { Type = EnemyType.Gripper,      WorldX = 2740 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2900 },
        },
        Boss = new BossSpawn { WorldX = 3050, AttackPattern = 2 }
    };

    public static FloorDefinition BuildFloor3() => new()
    {
        FloorNumber = 3, TotalWidth = 3200, TimeLimit = 100f,
        Spawns = new SpawnEntry[]
        {
            new() { Type = EnemyType.Gripper,      WorldX = 340  },
            new() { Type = EnemyType.Bouncer,      WorldX = 500  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 660  },
            new() { Type = EnemyType.StickFighter,  WorldX = 820  },
            new() { Type = EnemyType.Bouncer,      WorldX = 980  },
            new() { Type = EnemyType.Gripper,      WorldX = 1140 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1300 },
            new() { Type = EnemyType.Bouncer,      WorldX = 1460 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1620 },
            new() { Type = EnemyType.Gripper,      WorldX = 1780, Side = -1 },
            new() { Type = EnemyType.Bouncer,      WorldX = 1940 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2100 },
            new() { Type = EnemyType.StickFighter,  WorldX = 2260 },
            new() { Type = EnemyType.Bouncer,      WorldX = 2420 },
            new() { Type = EnemyType.Gripper,      WorldX = 2580 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2740 },
            new() { Type = EnemyType.Bouncer,      WorldX = 2900 },
        },
        Boss = new BossSpawn { WorldX = 3050, AttackPattern = 3 }
    };

    public static FloorDefinition BuildFloor4() => new()
    {
        FloorNumber = 4, TotalWidth = 3400, TimeLimit = 95f,
        Spawns = new SpawnEntry[]
        {
            new() { Type = EnemyType.Pudgy,        WorldX = 320  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 480  },
            new() { Type = EnemyType.StickFighter,  WorldX = 640  },
            new() { Type = EnemyType.Bouncer,      WorldX = 800  },
            new() { Type = EnemyType.Pudgy,        WorldX = 960, Side = -1 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1120 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1280 },
            new() { Type = EnemyType.Bouncer,      WorldX = 1440 },
            new() { Type = EnemyType.Pudgy,        WorldX = 1600 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1760 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1920 },
            new() { Type = EnemyType.Bouncer,      WorldX = 2080 },
            new() { Type = EnemyType.Pudgy,        WorldX = 2240, Side = -1 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2400 },
            new() { Type = EnemyType.Bouncer,      WorldX = 2560 },
            new() { Type = EnemyType.Pudgy,        WorldX = 2720 },
            new() { Type = EnemyType.StickFighter,  WorldX = 2880 },
            new() { Type = EnemyType.Bouncer,      WorldX = 3040 },
            new() { Type = EnemyType.Pudgy,        WorldX = 3200 },
        },
        Boss = new BossSpawn { WorldX = 3300, AttackPattern = 4 }
    };

    public static FloorDefinition BuildFloor5() => new()
    {
        FloorNumber = 5, TotalWidth = 3600, TimeLimit = 90f,
        Spawns = new SpawnEntry[]
        {
            new() { Type = EnemyType.Gripper,      WorldX = 300  },
            new() { Type = EnemyType.Pudgy,        WorldX = 460  },
            new() { Type = EnemyType.KnifeThrower, WorldX = 620  },
            new() { Type = EnemyType.StickFighter,  WorldX = 780  },
            new() { Type = EnemyType.Bouncer,      WorldX = 940  },
            new() { Type = EnemyType.Pudgy,        WorldX = 1100, Side = -1 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 1260 },
            new() { Type = EnemyType.Gripper,      WorldX = 1420 },
            new() { Type = EnemyType.StickFighter,  WorldX = 1580 },
            new() { Type = EnemyType.Bouncer,      WorldX = 1740 },
            new() { Type = EnemyType.Pudgy,        WorldX = 1900 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2060 },
            new() { Type = EnemyType.Gripper,      WorldX = 2220, Side = -1 },
            new() { Type = EnemyType.Bouncer,      WorldX = 2380 },
            new() { Type = EnemyType.StickFighter,  WorldX = 2540 },
            new() { Type = EnemyType.Pudgy,        WorldX = 2700 },
            new() { Type = EnemyType.KnifeThrower, WorldX = 2860 },
            new() { Type = EnemyType.Bouncer,      WorldX = 3020 },
            new() { Type = EnemyType.Gripper,      WorldX = 3180 },
            new() { Type = EnemyType.Pudgy,        WorldX = 3340, Side = -1 },
        },
        Boss = new BossSpawn { WorldX = 3480, AttackPattern = 5 }
    };
}
