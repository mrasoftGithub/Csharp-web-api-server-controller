using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace APIController.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CRUDController : ControllerBase
{
    private readonly DbContextClass _DbContextClass;

    public CRUDController(DbContextClass dbContextClass)
    {
        _DbContextClass = dbContextClass;
    }

    [HttpGet]
    [Route("HaalOpEigenaren")]
    public async Task<ActionResult<List<EIGENAAR>>> HaalOpEigenaren()
    {
        try
        {
            return Ok(await _DbContextClass.EIGENAAR.ToListAsync());
        }
        catch (Exception e)
        {
            return Problem(
               statusCode: (int)HttpStatusCode.InternalServerError,
               detail: "Er is iets fout gegaan met action method " + "HaalOpEigenaren van CRUDController: " + e.Message);
        }
    }

    [HttpGet]
    [Route("HaalOpEigenaar/{ID}")]
    public async Task<ActionResult<EIGENAAR>> HaalopEigenaar(int ID)
    {
        try
        {
            var resultaat = await _DbContextClass.EIGENAAR.FindAsync(ID);
            if (resultaat == null)
            {
                return Problem(
                   statusCode: (int)HttpStatusCode.NotFound,
                   detail: "Er is iets fout gegaan met action method " + "HaalOpEigenaar van CRUDController: Niks gevonden");
            }
            return Ok(resultaat);
        }
        catch (Exception e)
        {
            return Problem(
               statusCode: (int)HttpStatusCode.InternalServerError,
               detail: "Er is iets fout gegaan met action method " + "HaalOpEigenaar van CRUDController: " + e.Message);
        }
    }

    [HttpPost]
    [Route("VoegToe")]
    public async Task<ActionResult<EIGENAAR>> VoegToe([FromBody] EIGENAAR eigenaar)
    {
        try
        {
            _DbContextClass.EIGENAAR.Add(eigenaar);
            await _DbContextClass.SaveChangesAsync();
            var resultaat = await _DbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == eigenaar.ID);
            return Ok(resultaat);
        }
        catch (Exception e)
        {
            return Problem(
                statusCode: (int)HttpStatusCode.InternalServerError,
                detail: "Er is iets fout gegaan met action method " +
                         "VoegToe van CRUDController: " + e.Message);
        }
    }

    [HttpPut]
    [Route("Muteer")]
    public async Task<ActionResult<EIGENAAR>> Muteer([FromBody] EIGENAAR eigenaar)
    {
        try
        {
            var eigenaarOrg = await _DbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == eigenaar.ID);
            if (eigenaarOrg == null)
            {
                return Problem(
                   statusCode: (int)HttpStatusCode.NotFound,
                   detail: "Er is iets fout gegaan met action method " + "HaalOpEigenaar van CRUDController: Niks gevonden");
            }
            eigenaarOrg.Omschrijving = eigenaar.Omschrijving;
            eigenaarOrg.Voornaam = eigenaar.Voornaam;
            eigenaarOrg.Achternaam = eigenaar.Achternaam;
            eigenaarOrg.Regio = eigenaar.Regio;

            await _DbContextClass.SaveChangesAsync();

            var resultaat = await _DbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == eigenaar.ID);
            if (resultaat == null)
                return Problem(
                    statusCode: (int)HttpStatusCode.NotFound,
                    detail: "Er is iets fout gegaan met action method " +
                             "Muteer van CRUDController: de eigenaar kan niet gevonden worden. ");

            return Ok(resultaat);
        }
        catch (Exception e)
        {
            return Problem(
                statusCode: (int)HttpStatusCode.InternalServerError,
                detail: "Er is iets fout gegaan met action method " +
                        "Muteer van CRUDController: " + e.Message);
        }
    }

    [HttpDelete]
    [Route("Verwijder/{ID}")]
    public async Task<bool> Verwijder(int ID)
    {
        try
        {
            var eigenaarOrg = await _DbContextClass.EIGENAAR.FirstOrDefaultAsync(x => x.ID == ID);

            if (eigenaarOrg == null) return false;

            _DbContextClass.EIGENAAR.Remove(eigenaarOrg);
            await _DbContextClass.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

}

