using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        /*private static List<Character> characters = new List<Character>{
            new Character(),
            new Character{Id = 1, Name = "Sam"}
        };*/
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }
        private int GetUserId(){
                return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
        //List<Character> ICharacterService.AddCharacter(Character c)
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto c)
        {
            var Sr = new ServiceResponse<List<GetCharacterDto>>();
            var cr = _mapper.Map<Character>(c);
            cr.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            //cr.Id = characters.Max(m => m.Id) + 1; il campo nel DB è stato definito con Identity(1,1)
            _context.Characters.Add(cr);
            await _context.SaveChangesAsync(); //this method writes the changes to the DB
            //characters.Add(cr);
            Sr.Data = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(crt => _mapper.Map<GetCharacterDto>(crt))
                    .ToListAsync();
            return Sr;
        }

        //List<Character> ICharacterService.GetAllCharacters()
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()

        {   var Sr = new ServiceResponse<List<GetCharacterDto>>();
            var DbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            Sr.Data = DbCharacters.Select(cr => _mapper.Map<GetCharacterDto>(cr)).ToList();
            return Sr;
        }

        //Character ICharacterService.GetCharacter(int id)
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacter(int id)
        {
            var Sr = new ServiceResponse<GetCharacterDto>();
            var DbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(cr => cr.Id == id && cr.User!.Id == GetUserId());
            /*
            if(character is not null)
                {return character;}
            throw new Exception("Charachter not found"); */
            Sr.Data = _mapper.Map<GetCharacterDto>(DbCharacters);
            return Sr;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> RemoveCharacter(int id)
        {
            var Sr = new ServiceResponse<List<GetCharacterDto>>();
            try
                {
                    var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                    if(character is null)
                        {throw new Exception($"Character with id '{id}' is not found");}
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    Sr.Data = await _context.Characters
                        .Where(crt => crt.User!.Id == GetUserId())
                        .Select(crt => _mapper.Map<GetCharacterDto>(crt)).ToListAsync();
                }
            catch(Exception ex)
                {
                    Sr.Message = ex.Message;
                    Sr.Success = false;
                }
            return Sr;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto c)
        {   var Sr = new ServiceResponse<GetCharacterDto>();
            try {
                var character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(cr => cr.Id == c.Id);
                if(character is null || character.User!.Id != GetUserId())
                    {throw new Exception($"Character with Id '{c.Id}' not found");}
                character.Name = c.Name;
                character.HitPoints = c.HitPoints;
                character.Strength = c.Strength;
                character.Defense = c.Defense;
                character.Intelligence = c.Intelligence;
                character.Class = c.Class;
                await _context.SaveChangesAsync();
                Sr.Data = _mapper.Map<GetCharacterDto>(character);}
            catch(Exception exc){
                Sr.Message = exc.Message;
                Sr.Success = false;
            }
            return Sr;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var sr = new ServiceResponse<GetCharacterDto>();
            try{
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(
                    c => c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId()
                );

                if(character is null){
                sr.Success = false;
                sr.Message = "Character not found.";                 
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

                if(skill is null){
                sr.Success = false;
                sr.Message = "Skill not found.";      
                }
                character!.Skills!.Add(skill!);
                await _context.SaveChangesAsync();
                sr.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex){
                sr.Success = false;
                sr.Message = ex.Message;
            }
            return sr;
        }
    }
}