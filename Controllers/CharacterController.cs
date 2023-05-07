using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;
//oppure puoi fare un global using dotnet_rpg.Models; nella classe program.cs
namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
            new Character{Id = 1, Name = "Sam"}
        };
        //this below enable us to send specific http status code back to the client together with the actual data that was requested
        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get(){
            //we send the status code 200
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetSingle(int id){
            //we send the status code 200
            //lambda method
            return Ok(characters.FirstOrDefault(c => c.Id == id));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character newChar){
            characters.Add(newChar);
            return Ok(characters);
        }
    }
}