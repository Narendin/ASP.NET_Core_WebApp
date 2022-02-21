using System;
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
            if (result == null)
            {
                return NotFound("Пользователя с указанным id не существует.");
            }

            return DtoFromPerson(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<PersonDto>>> FindByNameAsync([FromQuery] string Term, CancellationToken cts)
        {
            var result = await _repository.FindByNameAsync(Term, cts);
            if (!result.Any())
            {
                return NotFound("Пользователей с указанным именем не существует.");
            }

            return ListPersonFromDto(result);
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

            return resultList.Select(DtoFromPerson).ToList();
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] PersonDto newPerson, CancellationToken cts)
        {
            await _repository.AddAsync(PersonFromDto(newPerson), cts);
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] PersonDto newPerson, CancellationToken cts)
        {
            return await _repository.UpdateAsync(PersonFromDto(newPerson), cts);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteAsync([FromRoute] int id, CancellationToken cts)
        {
            return await _repository.DeleteAsync(id, cts);
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

        private static List<PersonDto> ListPersonFromDto(List<Person> inputList)
        {
            return inputList.Select(DtoFromPerson).ToList();
        }
    }
}