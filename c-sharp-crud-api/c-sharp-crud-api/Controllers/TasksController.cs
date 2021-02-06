using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace c_sharp_crud_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly Data.TaskBoard _board;

        public TasksController(Data.TaskBoard board)
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
        [HttpPost, Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Models.Task), 201)]
        public IActionResult Post([FromBody] Models.Task task)
        {
            task.Id = _board.NextId();

            _board.Tasks.Add(task);
            _board.SaveChanges();

            return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.PathBase}/tasks/{task.Id}"), task);
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
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

            return Ok(checkTask);
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult Delete(int id)
        {
            var task = _board.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task is null)
                return NotFound();

            _board.Remove(task);
            _board.SaveChanges();

            return NoContent();
        }
    }
}
