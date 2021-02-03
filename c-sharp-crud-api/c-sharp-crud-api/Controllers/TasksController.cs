using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using c_sharp_crud_api.Data;
using Microsoft.AspNetCore.Authorization;

namespace c_sharp_crud_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskBoard _board;

        public TasksController(TaskBoard board)
        {
            _board = board;
        }

        // GET: api/<TasksController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_board.Tasks);
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult Get(int id)
        {
            var task = _board.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task is null) 
                return NotFound();

            return Ok(task);
        }

        // POST api/<TasksController>
        [HttpPost, Authorize]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Models.Task), 202)]
        public IActionResult Post([FromBody] Models.Task task)
        {
            if (_board.Tasks.Any(t => t.Id == task.Id))
                return BadRequest("Id already exists");

            _board.Tasks.Add(task);
            _board.SaveChanges();

            return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.PathBase}/tasks/{task.Id}"), task);
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}"), Authorize]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult Put(int id, [FromBody] Models.Task task)
        {
            var checkTask = _board.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (checkTask is null)
                return NotFound();

            checkTask.Name = task.Name;
            checkTask.Description = task.Description;
            checkTask.Status = task.Status;

            _board.SaveChanges();

            return Ok(task);
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
        {
            var task = _board.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task is null)
                return NotFound();

            _board.Remove(task);
            _board.SaveChanges();

            return Ok();
        }
    }
}
