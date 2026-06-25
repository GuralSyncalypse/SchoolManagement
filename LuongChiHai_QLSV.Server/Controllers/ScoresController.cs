using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuongChiHai_QLSV.Server.Models;
using LuongChiHai_QLSV.Server.Data;

[Route("api/[controller]")]
[ApiController]
public class ScoresController : ControllerBase
{
    private readonly SchoolContext _context;
    public ScoresController(SchoolContext context)
    {
        _context = context;
    }

    // GET: api/Score
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Score>>> GetScore()
    {
        return await _context.Scores.ToListAsync();
    }

    // GET: api/Score/5
    [HttpGet("{scoreid}")]
    public async Task<ActionResult<Score>> GetScore(int scoreid)
    {
        var score = await _context.Scores.FindAsync(scoreid);

        if (score == null)
        {
            return NotFound();
        }

        return score;
    }

    // PUT: api/Score/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{scoreid}")]
    public async Task<IActionResult> PutScore(int? scoreid, Score score)
    {
        if (scoreid != score.ScoreId)
        {
            return BadRequest();
        }

        _context.Entry(score).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ScoreExists(scoreid))
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

    // POST: api/Score
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Score>> PostScore(Score score)
    {
        _context.Scores.Add(score);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetScore", new { scoreid = score.ScoreId }, score);
    }

    // DELETE: api/Score/5
    [HttpDelete("{scoreid}")]
    public async Task<IActionResult> DeleteScore(int? scoreid)
    {
        var score = await _context.Scores.FindAsync(scoreid);
        if (score == null)
        {
            return NotFound();
        }

        _context.Scores.Remove(score);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ScoreExists(int? scoreid)
    {
        return _context.Scores.Any(e => e.ScoreId == scoreid);
    }
}
