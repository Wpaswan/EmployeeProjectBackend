using EmployeeApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblEmployee>>> GetTblEmployee()
        {
            try
            {
                return await _context.TblEmployees.ToListAsync();

                var employees = (from e in _context.TblEmployees
                                 join d in _context.TblDesignations
                                 on e.DesignationID equals d.Id

                                 select new TblEmployee
                                 {
                                     Id = e.Id,
                                     Name = e.Name,
                                     LastName = e.LastName,
                                     Email = e.Email,
                                     Age = e.Age,
                                     DesignationID = e.DesignationID,
                                     Designation = d.Designation,
                                     Doj = e.Doj,
                                     Gender = e.Gender,
                                     IsActive = e.IsActive,
                                     IsMarried=e.IsMarried

                                 }
                                ).ToListAsync();






                return await employees;
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblEmployee>> GetTblEmployee(int id)
        {
            var tblEmployee = await _context.TblEmployees.FindAsync(id);

            if (tblEmployee == null)
            {
                return NotFound();
            }

            return tblEmployee;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblEmployee(int id, TblEmployee tblEmployee)
        {
            if (id != tblEmployee.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblEmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TblEmployee>> AddEmployee(TblEmployee tblEmployee)
        {
            _context.TblEmployees.Add(tblEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblEmployee", new { id = tblEmployee.Id }, tblEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblEmployee>> DeleteTblEmployee(int id)
        {
            var tblEmployee = await _context.TblEmployees.FindAsync(id);
            if (tblEmployee == null)
            {
                return NotFound();
            }

            _context.TblEmployees.Remove(tblEmployee);
            await _context.SaveChangesAsync();

            return tblEmployee;
        }

        private bool TblEmployeeExists(int id)
        {
            return _context.TblEmployees.Any(e => e.Id == id);
        }
    }
}
