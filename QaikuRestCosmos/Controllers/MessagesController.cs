using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace QaikuRestCosmos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DocumentClient _client;
        private const string _dbName = "qaikumessages";
        private const string _collectionName = "Collection1";

        public MessagesController(IConfiguration configuration)
        {
            _configuration = configuration;
            var endpointuri = _configuration["ConnectionStrings:CosmosDBConnection:EndpointUri"];
            var key = _configuration["ConnectionStrings:CosmosDBConnection:PrimaryKey"];
            _client = new DocumentClient(new Uri(endpointuri), key);
            _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _dbName }).Wait();
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_dbName), new DocumentCollection { Id = _collectionName });

        }

        //GET api/messages/ping
        [HttpGet]
        public string Ping()
        {
            return "yhteys luotu";
        }

        // GET api/messages/getallmessages
        [HttpGet]
        public ActionResult<List<Message>> GetAllMessages()
      {
            FeedOptions queryoptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> query = _client.CreateDocumentQuery<Message>(UriFactory
                .CreateDocumentCollectionUri(_dbName, _collectionName), $"SELECT * FROM C");

            return Ok(query.ToList());
        }

        // GET api/messages/getbysenderid
        [HttpGet]
        public ActionResult<List<Message>> GetBySenderId(string id)
        {
            FeedOptions queryoptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> query = _client.CreateDocumentQuery<Message>(UriFactory
                .CreateDocumentCollectionUri(_dbName, _collectionName), $"SELECT * FROM c  WHERE CONTAINS(c.SenderId, '{id}')");
    

            return Ok(query.ToList());
        }
        // GET api/messages/getbyrecipientid
        [HttpGet]
        public ActionResult<List<Message>> GetByRecipientId(string id)
        {
            FeedOptions queryoptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> query = _client.CreateDocumentQuery<Message>(UriFactory
                .CreateDocumentCollectionUri(_dbName, _collectionName), $"SELECT * FROM c  WHERE CONTAINS(c.RecipientsIdCsv, '{id}')");

            return Ok(query.ToList());
        }

        // GET api/messages/getbythreadid
        [HttpGet]
        public ActionResult<List<Message>> GetByThreadId(string threadId)
        {
            FeedOptions queryoptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> query = _client.CreateDocumentQuery<Message>(UriFactory
                .CreateDocumentCollectionUri(_dbName, _collectionName), $"SELECT * FROM c  WHERE CONTAINS(c.ThreadId, '{threadId}')");

            return Ok(query.ToList());
        }



        //GET api/messages/getbydocumentid
        [HttpGet]
        public async Task<ActionResult<Message>> GetByDocumentId(string documentid)
        {
            try
            {
                Message message = await _client.ReadDocumentAsync<Message>(UriFactory.CreateDocumentUri(_dbName, _collectionName, documentid));
                return Ok(message);
            }
            catch (DocumentClientException de)
            {

                switch (de.StatusCode.Value)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return NotFound();
                }

            }
                return BadRequest();
        }
    
      

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Message value)
        {
            Document document = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), value);
            return Ok(document.Id);
        }

      

        //PUT api/messages/5
      
        [HttpPut]
        public async Task<ActionResult<string>> PutState(string documentid, [FromBody]Message value)
        {

            try
            {
                Message msg = _client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName))
                            .Where(r => r.id == documentid)
                            .AsEnumerable()
                            .SingleOrDefault();

                msg.State = value.State; 
                Document document = await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_dbName, _collectionName, documentid), msg);

                return Ok(document);
            }
            catch (DocumentClientException de)
            {

                switch (de.StatusCode.Value)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return NotFound();
                }

            }
            return BadRequest();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(string documentid)
        {
            try
            {
                await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_dbName, _collectionName, documentid));
                return Ok($"Deleted document {documentid}");
            }
            catch (DocumentClientException de)
            {

                switch (de.StatusCode.Value)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return NotFound();
                }
            }
            return BadRequest();
        }
    }
}
