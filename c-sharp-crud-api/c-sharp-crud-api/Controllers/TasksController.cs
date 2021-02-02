using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using c_sharp_crud_api.Data;

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
            var tasks = _board.Tasks.ToList();

            return Ok(tasks);
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tasks = _board.Tasks.ToList();

            var task = tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task is null) return NotFound();

            return Ok(task);
        }

        // POST api/<TasksController>
        [HttpPost]
        public IActionResult Post([FromBody] Models.Task task)
        {
            var tasks = _board.Tasks.ToList();

            if (tasks.Any(t => t.Id == task.Id))
                return BadRequest("Id already exists");

            _board.Tasks.Add(task);
            _board.SaveChanges();

            return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.PathBase}/tasks/{task.Id}"), task);
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Models.Task task)
        {
            var tasks = _board.Tasks.ToList();

            var checkTask = tasks.Where(t => t.Id == id).FirstOrDefault();
            if (checkTask is null) 
                return NotFound();


            checkTask.Name = task.Name;
            checkTask.Description = task.Description;
            checkTask.Status = task.Status;

            _board.SaveChanges();

            return Ok(task);
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tasks = _board.Tasks.ToList();

            var checkTask = tasks.Where(t => t.Id == id).FirstOrDefault();
            if (checkTask is null)
                return NotFound();

            _board.Remove(checkTask);
            _board.SaveChanges();

            return Ok();
        }
    }
}
