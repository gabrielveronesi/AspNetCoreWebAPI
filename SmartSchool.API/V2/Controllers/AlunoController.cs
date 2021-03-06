using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.V1.Dtos;
using SmartSchool.API.Models;
using SmartSchool.WebAPI.Data;

namespace SmartSchool.API.V2.Controllers
{
    ///<sumary>
    /// Versão 2 do meu controlador de alunos
    ///</sumary>

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlunoController : ControllerBase
    {
        //quando eu instanciar uma controller do tipo aluno vou passar como parametro o meu contexto -> _context

        public readonly IRepository _repo;

        private readonly IMapper _mapper;

        public AlunoController(IRepository repo,
                               IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var alunos = _repo.GetAllAlunos(true);

            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }
        //api/aluno/1
        ///<sumary>
        ///Método responsavel para retornar 1 aluno pelo ID
        ///</sumary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest("O Aluno não foi encontrado");
            
            var alunoDto = _mapper.Map<AlunoDto>(aluno);
            return Ok(alunoDto);
        }

        
        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("Aluno não cadastrado");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("O Aluno não foi encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }

            return BadRequest("Aluno não atualizado");
        }

        //Escolha o PUT se o que você pretende é fazer uma atualização completa do seu recurso ou o PATCH se você quiser atualizar apenas um subconjunto dos dados do seu recurso.


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest("O Aluno não foi encontrado");


            _repo.Delete(aluno);
            if (_repo.SaveChanges())
            {
                return Ok("Aluno deletado!");
            }

            return BadRequest("Aluno não deletado");
        }


    }
}