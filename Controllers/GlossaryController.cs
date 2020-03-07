using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Glossary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlossaryController : ControllerBase
    {
        private static List<GlossaryItem> Glossary = new List<GlossaryItem>()
        {
            new GlossaryItem()
            {
                Term = "Access Token",
                Definition = "A credential that can be used by an application to access an API. " +
                             "It informs the API that the bearer of the token has been authorized " +
                             "to access the API and perform specific actions specified by the scope " +
                             "that has been granted."
            },
            new GlossaryItem()
            {
                Term = "JWT",
                Definition = "An open, industry standard RFC 7519 method for representing claims securely " +
                             "between two parties. "
            },
            new GlossaryItem()
            {
                Term = "OpenID",
                Definition = "An open standard for authentication that allows applications to verify" +
                             " users are who they say they are without needing to collect, store, " +
                             "and therefore become liable for a user’s login information."
            }
        };
        
        [HttpGet]
        public ActionResult<List<GlossaryItem>> Get()
        {
            return Ok(Glossary);
        }
        
        [HttpGet]
        [Route("{term}")]
        public ActionResult<List<GlossaryItem>> Get(string term)
        {
            var glossaryItem = Glossary.Find(item => 
                item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (glossaryItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(glossaryItem);   
            }
        }

        [HttpPost]
        public ActionResult Post(GlossaryItem glossaryItem)
        {
            var existingGlossaryItem = Glossary.Find(item =>
                item.Term.Equals(glossaryItem.Term, StringComparison.CurrentCultureIgnoreCase));

            if (existingGlossaryItem != null)
            {
                return Conflict("Cannot create term; it already exists!");
            }
            else
            {
                Glossary.Add(glossaryItem);
                var resourceUrl = Path.Combine(Request.Path.ToString(), 
                    Uri.EscapeUriString(glossaryItem.Term));
                return Created(resourceUrl, glossaryItem);
            }
        }
    }
}