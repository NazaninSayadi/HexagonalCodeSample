using ApiGateway.Models.Group;
using AutoMapper;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        public readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupDTO>> Get()
        {
            return await _groupService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDTO>> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("GroupId can not be empty");

            return await _groupService.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add(CreateGroupInputModel group)
        {
            if (group == null)
                return BadRequest("Group can not be null");

            return await _groupService.Add(group.Name, group.Capacity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateGroupInputModel group,Guid id)
        {
            if (group == null)
                return BadRequest("Group can not be null");

            if (id == Guid.Empty)
                return BadRequest("Please send valid Id");

            await _groupService.Update(id, group.Name,group.Capacity);

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Please send valid Id");

            await _groupService.Remove(id);

            return Ok();

        }
    }
}