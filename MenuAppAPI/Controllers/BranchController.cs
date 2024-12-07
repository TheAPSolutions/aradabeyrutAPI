using AradaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : Controller
    {
        private readonly AppDbContext appDbContext;

        public BranchController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            return Ok(await appDbContext.Branch.ToListAsync());
        }
    }
}
