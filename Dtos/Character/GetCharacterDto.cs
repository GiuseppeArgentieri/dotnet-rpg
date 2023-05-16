using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Skill;

namespace dotnet_rpg.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id {get; set;}
        public String Name {get; set;} = "Frodo";
        public int HitPoints {get; set;} = 100;
        public int Strength {get; set;} = 10;
        public int Defense {get; set;} = 10;
        public int Intelligence {get; set;} = 10;
        public RpgClass Class { get; set; } = RpgClass.knight;
        public GetWeaponDto? weapon { get; set;}
        public List<GetSkillDto>? Skills {get; set;}
        public int Fight { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}