using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
//using dotnet_rpg.Models;
//oppure puoi fare un global using dotnet_rpg.Models; nella classe program.cs
namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        //with this constructor we inject the service to the controller
        //this below enable us to send specific http status code back to the client together with the actual data that was requested
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get(){
            //we send the status code 200
            //int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id){
            //we send the status code 200
            //lambda method
            return Ok(await _characterService.GetCharacter(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newChar){
            return Ok(await _characterService.AddCharacter(newChar));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto c){
            var Response = await _characterService.UpdateCharacter(c);
            if(Response.Data is null)
                {return NotFound(Response);}
            return Ok(Response);
    
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id){
            var Response = await _characterService.RemoveCharacter(id);
            if(Response.Data is null)
                {return NotFound(Response);}
            return Ok(Response);
    
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddSkill(AddCharacterSkillDto csk){
            return Ok(await _characterService.AddCharacterSkill(csk));
        }
    }
}