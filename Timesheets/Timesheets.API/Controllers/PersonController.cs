using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timesheets.BD.Interfaces;
using Timesheets.BD.Models;
using Timesheets.API.DTO;

namespace Timesheets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IDbRepository<Person> _repository;

        public PersonController(IDbRepository<Person> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PersonDto>> ReadAsync([FromRoute] int id, CancellationToken cts)
        {
            Person result = await _repository.GetAsync(id, cts);
            if (result.Id == 0)
            {
                return NotFound("Пользователя с указанным id не существует.");
            }

            return DtoFromPerson(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PersonDto>> FindByNameAsync([FromQuery] string Term, CancellationToken cts)
        {
            Person result = await _repository.FindByNameAsync(Term, cts);
            if (result.Id == 0)
            {
                return NotFound("Пользователя с указанным id не существует.");
            }
            return DtoFromPerson(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPaginationAsync([FromQuery] int skip, [FromQuery] int take,
            CancellationToken cts)
        {
            var resultList = (await _repository.GetPaginationAsync(skip, take, cts)).ToList();
            if (!resultList.Any())
            {
                return NotFound("Ничего не найдено");
            }

            List<PersonDto> finallyList = new List<PersonDto>();

            foreach (var result in resultList)
            {
                finallyList.Add(DtoFromPerson(result));
            }

            return finallyList;
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] PersonDto newPerson, CancellationToken cts)
        {
            await _repository.AddAsync(PersonFromDto(newPerson), cts);
        }

        [HttpPut]
        public async Task UpdateAsync([FromBody] PersonDto newPerson, CancellationToken cts)
        {
            await _repository.UpdateAsync(PersonFromDto(newPerson), cts);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync([FromRoute] int id, CancellationToken cts)
        {
            await _repository.DeleteAsync(id, cts);
            return;
        }

        private static Person PersonFromDto(PersonDto dto)
        {
            return new Person()
            {
                Id = dto.Id,
                Age = dto.Age,
                Company = dto.Company,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
        }

        private static PersonDto DtoFromPerson(Person person)
        {
            return new PersonDto()
            {
                Id = person.Id,
                Age = person.Age,
                Company = person.Company,
                Email = person.Email,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
        }
    }
}